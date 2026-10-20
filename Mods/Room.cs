using BepInEx;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace NotMonk_Client.Mods
{
	public class RoomMods
	{
		public static void LobbyHop()
		{
			NetworkSystem.Instance.ReturnToSinglePlayer();
			GorillaNetworkJoinTrigger trigger = PhotonNetworkController.Instance.currentJoinTrigger ?? GorillaComputer.instance.GetJoinTriggerForZone("forest");
			PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger);
		}

		public static void RoomFile()
		{
			if (!PhotonNetwork.InRoom)
			{
				Debug.Log("Not in a room, nothing to log");
				return;
			}

			string roomName = PhotonNetwork.CurrentRoom.Name;
			string fileContents = GenerateRoomFile(roomName);

			try
			{
				string directory = Path.Combine(Paths.PluginPath, "Player Info");
				Directory.CreateDirectory(directory);

				string fileName = SanitizeFileName(roomName) + ".txt";
				string filePath = Path.Combine(directory, fileName);

				File.WriteAllText(filePath, fileContents);

				Process.Start(new ProcessStartInfo
				{
					FileName = filePath,
					UseShellExecute = true
				});
			}
			catch (Exception exception)
			{
				Debug.Log("Failed to save player info: " + exception.Message);
			}
		}

		private static string GenerateRoomFile(string roomName)
		{
			const string divider = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

			StringBuilder builder = new StringBuilder();

			builder.AppendLine("Room Info Grabbed By NotMonk Client");
			builder.AppendLine(".gg/qAW5Py7p4f");
			builder.AppendLine(divider);

			builder.AppendLine($"Room: {roomName}");

			foreach (Player player in PhotonNetwork.PlayerList)
			{
				AppendPlayerInfo(builder, player, divider);
			}

			builder.AppendLine(divider);

			return builder.ToString();
		}

		private static void AppendPlayerInfo(
			StringBuilder builder,
			Player player,
			string divider)
		{
			try
			{
				(int r, int g, int b) = GetPlayerColor(player);

				builder.AppendLine(divider);
				builder.AppendLine(
					$"Player Name: \"{player.NickName}\", " +
					$"Player ID: \"{player.UserId}\", " +
					$"Player Color: (R: {r}, G: {g}, B: {b})");
			}
			catch (Exception exception)
			{
				Debug.Log("Failed to log player: " + exception.Message);
			}
		}

		private static (int r, int g, int b) GetPlayerColor(Player player)
		{
			try
			{
				VRRig rig =
					NotMonk_Client.Other.RigUtils.GetVRRigFromPlayer(player);

				if (rig == null)
				{
					throw new Exception("Player rig does not exist");
				}

				Color color = rig.playerColor;

				return (
					Mathf.RoundToInt(color.r * 255f),
					Mathf.RoundToInt(color.g * 255f),
					Mathf.RoundToInt(color.b * 255f)
				);
			}
			catch
			{
				Debug.Log("Failed to log colors, rig most likely nonexistent");
				return (0, 0, 0);
			}
		}

		private static string SanitizeFileName(string fileName)
		{
			char[] invalidCharacters = Path.GetInvalidFileNameChars();

			return new string(
				fileName.Select(character =>
					invalidCharacters.Contains(character) ? '_' : character
				).ToArray()
			);
		}
	}
}
