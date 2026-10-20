using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace NotMonk_Client.Admin
{
	public static class NotMonkNetwork
	{
		private const byte EventCode = 69;

		private static readonly Dictionary<string, Action> Commands =
			new Dictionary<string, Action>();

		public static void SendMessage(string message)
		{
			if (!PhotonNetwork.InRoom)
				return;

			object[] data =
			{
				message
			};

			RaiseEventOptions options = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others
			};

			PhotonNetwork.RaiseEvent(
				EventCode,
				data,
				options,
				SendOptions.SendReliable
			);
		}

		public static void RegisterCommand(string name, Action command)
		{
			if (string.IsNullOrEmpty(name) || command == null)
				return;

			Commands[name] = command;
		}

		public static void ExecuteCommand(string name)
		{
			if (!PhotonNetwork.InRoom)
				return;

			object[] data =
			{
				name
			};

			RaiseEventOptions options = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others
			};

			PhotonNetwork.RaiseEvent(
				EventCode,
				data,
				options,
				SendOptions.SendReliable
			);
		}

		public static void ReceiveEvent(EventData eventData)
		{
			if (eventData.Code != EventCode)
				return;

			object[] data = eventData.CustomData as object[];

			if (data == null || data.Length == 0)
				return;

			string value = data[0]?.ToString();

			if (string.IsNullOrEmpty(value))
				return;

			if (Commands.TryGetValue(value, out Action command))
			{
				try
				{
					command.Invoke();
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(
						"[NotMonk's GUI] Command '" +
						value +
						"' failed: " +
						ex
					);
				}

				return;
			}
			// Log all received network requests
			UnityEngine.Debug.Log(
				"[NotMonk's GUI] Received: " + value
			);
		}
	}
}