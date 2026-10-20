using GorillaGameModes;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace NotMonk_Client.Other.DiscordRPC
{
	public static class DiscordRPCManager
	{
		private const string ClientId = "1557965770103849051";

		public static readonly List<string> stateSplash = new List<string>()
		{
			"Holy Crap. IM AN RPC!!!",
			"Sub 2 NotMonk?",
			"nmv.skyehub.org!! (skyehub.org peak)",
			"Wifiskeleton Is Peak",
			"Overtonight Is Top 1 Human",
			"ミク、ミク、ミク、ゴー！",
			"AAAAAAAAAAAAAAAAAAAAH",
		};

		private static NamedPipeClientStream pipe;
		private static Thread worker;
		private static readonly object pipeLock = new object();
		private static readonly object writeLock = new object();
		private static readonly object dataLock = new object();

		private static string pendingActivity;
		private static string lastActivity;
		private static bool ready;
		private static DateTime sessionStart;
		private static float nextUpdate;
		private static float nextSendAllowed;
		private static int cachedPid;
		private static int generation;

		private static string uDetails = "Using NotMonk Client : Gorilla Tag";
		private static string uState = "Not in a room!";
		private static bool uInRoom = false;

		public static bool Enabled { get; private set; }
		private static string randomstate;

		private static void Awake()
		{
			Random rand = new Random();
			randomstate = stateSplash[rand.Next(stateSplash.Count)];
		}

		public static void Enable()
		{
			if (Enabled)
				return;

			Enabled = true;
			sessionStart = DateTime.UtcNow;
			nextUpdate = 0f;
			nextSendAllowed = 0f;
			lastActivity = null;
			cachedPid = Process.GetCurrentProcess().Id;

			lock (pipeLock)
			{
				ready = false;
			}

			int myGen = Interlocked.Increment(ref generation);

			worker = new Thread(() => WorkerLoop(myGen));
			worker.IsBackground = true;
			worker.Name = "NotMonk Discord RPC";
			worker.Start();

			RoomEventWatcher.Create();

			UpdateUnityData();
			UpdatePresence();

			Debug.Log("[NotMonk Client] Discord RPC enabled.");
		}

		public static void Disable()
		{
			if (!Enabled)
				return;

			Enabled = false;
			Interlocked.Increment(ref generation);

			RoomEventWatcher.Remove();

			NamedPipeClientStream p;
			bool wasReady;
			lock (pipeLock)
			{
				p = pipe;
				wasReady = ready;
				pipe = null;
				ready = false;
				pendingActivity = null;
			}

			string clearRequest = BuildRequest("null");

			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					if (p != null && wasReady && p.IsConnected)
						WriteFrameTo(p, 1, clearRequest);
				}
				catch
				{
				}

				try
				{
					if (p != null)
						p.Dispose();
				}
				catch
				{
				}
			});

			Debug.Log("[NotMonk Client] Discord RPC disabled.");
		}

		public static void Toggle()
		{
			if (Enabled)
				Disable();
			else
				Enable();
		}

		public static void Refresh()
		{
			if (!Enabled)
				return;

			nextUpdate = Time.unscaledTime + 2f;
			UpdateUnityData();
			UpdatePresence();
		}

		public static void Update()
		{
			if (!Enabled)
				return;

			if (Time.unscaledTime >= nextUpdate)
			{
				nextUpdate = Time.unscaledTime + 2f;
				UpdateUnityData();
				UpdatePresence();
			}
		}

		private static void UpdateUnityData()
		{
			bool inRoom = false;
			string roomName = "Not in a room";
			string details = "Using NotMonk Client : Gorilla Tag";
			string state = "Not in a room!";
			string gamemode = "Unknown";

			int players = 0;
			int maxPlayers = 0;

			try
			{
				inRoom = NetworkSystem.Instance != null && NetworkSystem.Instance.InRoom;

				if (inRoom)
				{
					roomName = NetworkSystem.Instance.RoomName ?? "Unknown room";
					players = PhotonNetwork.PlayerList.Length;

					if (PhotonNetwork.CurrentRoom != null)
						maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

					if (GameMode.instance != null)
						gamemode = GorillaGameManager.instance.GameType().ToString();

					if (GorillaGameManager.instance != null)
					{
						if (NotMonk_Client.Menu.Settings.RPChideRoomCode)
						{
							details = "In a room - " + players + "/" + maxPlayers + " - Playing " + gamemode;
						}
						else
						{
							details = "ROOM " + roomName + " - " + players + "/" + maxPlayers + " - Playing " + gamemode;
						}
					}

					state = randomstate;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NotMonk Client] Couldn't read room info: " + ex.Message);
			}

			lock (dataLock)
			{
				uDetails = details;
				uState = state;
				uInRoom = inRoom;
			}
		}

		private static void UpdatePresence()
		{
			string details;
			string state;
			bool inRoom;

			lock (dataLock)
			{
				details = uDetails;
				state = uState;
				inRoom = uInRoom;
			}

			long startTimestamp = new DateTimeOffset(sessionStart).ToUnixTimeSeconds();

			string activity =
				"{"
				+ "\"details\":\"" + EscapeJson(details) + "\","
				+ "\"state\":\"" + EscapeJson(state) + "\","
				+ "\"assets\":{"
					+ "\"large_image\":\"image\","
					+ "\"large_text\":\"NotMonk Client\","
					+ "\"small_image\":\"gtaglogo\","
					+ "\"small_text\":\"" + EscapeJson(inRoom ? "Gorilla Tag : In a room" : "Gorilla Tag : Not in a room") + "\""
				+ "},"
				+ "\"timestamps\":{"
					+ "\"start\":" + startTimestamp
				+ "},"
				+ "\"buttons\":[{"
					+ "\"label\":\"Join The Server\","
					+ "\"url\":\"https://discord.gg\""
				+ "}]"
				+ "}";

			if (activity == lastActivity)
				return;

			if (Time.unscaledTime < nextSendAllowed)
				return;

			lastActivity = activity;
			nextSendAllowed = Time.unscaledTime + 4f;

			SendActivity(activity);
		}

		private static void WorkerLoop(int myGen)
		{
			while (Enabled && myGen == generation)
			{
				NamedPipeClientStream candidate = null;

				try
				{
					for (int i = 0; i < 10 && Enabled && myGen == generation; i++)
					{
						try
						{
							candidate = new NamedPipeClientStream(
								".",
								"discord-ipc-" + i,
								PipeDirection.InOut,
								PipeOptions.Asynchronous);

							candidate.Connect(500);

							if (candidate.IsConnected)
								break;

							candidate.Dispose();
							candidate = null;
						}
						catch
						{
							if (candidate != null)
							{
								candidate.Dispose();
								candidate = null;
							}
						}
					}

					if (!Enabled || myGen != generation || candidate == null || !candidate.IsConnected)
					{
						if (candidate != null)
						{
							try
							{
								candidate.Dispose();
							}
							catch
							{
							}
							candidate = null;
						}

						Thread.Sleep(1500);
						continue;
					}

					string handshake = "{\"v\":1,\"client_id\":\"" + ClientId + "\"}";

					WriteFrameTo(candidate, 0, handshake);

					bool accepted = false;

					lock (pipeLock)
					{
						if (Enabled && myGen == generation)
						{
							pipe = candidate;
							ready = false;
							accepted = true;
						}
					}

					if (!accepted)
					{
						try
						{
							candidate.Dispose();
						}
						catch
						{
						}
						candidate = null;
						continue;
					}

					Debug.Log("[NotMonk Client] Discord IPC connected, waiting for ready.");

					bool gotReady = false;

					while (Enabled && myGen == generation && candidate.IsConnected)
					{
						int opcode;
						byte[] payload;

						if (!ReadFrame(candidate, out opcode, out payload))
							break;

						if (opcode == 3)
						{
							string payloadString = Encoding.UTF8.GetString(payload);
							WriteFrameTo(candidate, 4, payloadString);
						}
						else if (opcode == 2)
						{
							break;
						}
						else if (opcode == 1)
						{
							string text = Encoding.UTF8.GetString(payload);

							if (!gotReady && text.Contains("\"READY\""))
							{
								gotReady = true;

								lock (pipeLock)
								{
									if (Enabled && myGen == generation && pipe == candidate)
										ready = true;
								}

								Debug.Log("[NotMonk Client] Discord IPC ready.");

								FlushPendingActivity();
							}
							else if (text.Contains("\"ERROR\""))
							{
								Debug.LogWarning("[NotMonk Client] Discord error: " + text);
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (Enabled && myGen == generation)
					{
						Debug.LogWarning("[NotMonk Client] Discord IPC: " + ex.Message);
					}
				}
				finally
				{
					lock (pipeLock)
					{
						if (candidate != null && pipe == candidate)
						{
							pipe = null;
							ready = false;
						}
					}

					if (candidate != null)
					{
						try
						{
							candidate.Dispose();
						}
						catch
						{
						}
					}
				}

				if (Enabled && myGen == generation)
					Thread.Sleep(1500);
			}
		}

		private static string BuildRequest(string activity)
		{
			return
				"{"
				+ "\"cmd\":\"SET_ACTIVITY\","
				+ "\"args\":{"
					+ "\"pid\":" + cachedPid + ","
					+ "\"activity\":" + activity
				+ "},"
				+ "\"nonce\":\"" + Guid.NewGuid().ToString() + "\""
				+ "}";
		}

		private static void SendActivity(string activity)
		{
			if (!Enabled)
				return;

			string request = BuildRequest(activity);

			NamedPipeClientStream p;
			bool isReady;
			lock (pipeLock)
			{
				p = pipe;
				isReady = ready;
				pendingActivity = request;
			}

			if (p == null || !isReady)
				return;

			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					if (!p.IsConnected)
						return;

					WriteFrameTo(p, 1, request);

					lock (pipeLock)
					{
						if (pendingActivity == request)
							pendingActivity = null;
					}
				}
				catch
				{
				}
			});
		}

		private static void FlushPendingActivity()
		{
			NamedPipeClientStream p;
			string request;

			lock (pipeLock)
			{
				p = pipe;
				request = pendingActivity;
			}

			if (p == null || string.IsNullOrEmpty(request))
				return;

			try
			{
				if (!p.IsConnected)
					return;

				WriteFrameTo(p, 1, request);

				lock (pipeLock)
				{
					if (pendingActivity == request)
						pendingActivity = null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NotMonk Client] Couldn't send presence: " + ex.Message);
			}
		}

		private static void WriteFrameTo(NamedPipeClientStream stream, int opcode, string json)
		{
			byte[] payload = Encoding.UTF8.GetBytes(json);
			byte[] header = new byte[8];

			Array.Copy(BitConverter.GetBytes(opcode), 0, header, 0, 4);
			Array.Copy(BitConverter.GetBytes(payload.Length), 0, header, 4, 4);

			lock (writeLock)
			{
				stream.Write(header, 0, header.Length);

				if (payload.Length > 0)
					stream.Write(payload, 0, payload.Length);

				stream.Flush();
			}
		}

		private static bool ReadFrame(NamedPipeClientStream stream, out int opcode, out byte[] payload)
		{
			opcode = 0;
			payload = null;

			byte[] header = new byte[8];

			if (!ReadExact(stream, header, header.Length))
				return false;

			opcode = BitConverter.ToInt32(header, 0);
			int length = BitConverter.ToInt32(header, 4);

			if (length < 0 || length > 1024 * 1024)
				return false;

			payload = new byte[length];

			return length == 0 || ReadExact(stream, payload, length);
		}

		private static bool ReadExact(Stream stream, byte[] buffer, int length)
		{
			int offset = 0;

			while (offset < length)
			{
				int read = stream.Read(buffer, offset, length - offset);

				if (read <= 0)
					return false;

				offset += read;
			}

			return true;
		}

		private static string EscapeJson(string input)
		{
			if (string.IsNullOrEmpty(input))
				return "";

			StringBuilder sb = new StringBuilder();
			foreach (char c in input)
			{
				switch (c)
				{
					case '\\': sb.Append("\\\\"); break;
					case '"': sb.Append("\\\""); break;
					case '\n': sb.Append("\\n"); break;
					case '\r': sb.Append("\\r"); break;
					case '\t': sb.Append("\\t"); break;
					default: sb.Append(c); break;
				}
			}
			return sb.ToString();
		}
	}
}