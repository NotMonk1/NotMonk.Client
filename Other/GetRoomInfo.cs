using System.Text;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NotMonk_Client.Other
{
	public class GetRoomInfo : MonoBehaviour
	{
		public void LogAllRoomInfo()
		{
			if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null)
			{
				Debug.Log("[RoomInfoLogger] Not currently in a Photon room.");
				return;
			}

			Room currentRoom = PhotonNetwork.CurrentRoom;
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("=== GORILLA TAG ROOM INFO ===");
			sb.AppendLine($"Room Name (Code): {currentRoom.Name}");
			sb.AppendLine($"Player Count: {currentRoom.PlayerCount} / {currentRoom.MaxPlayers}");
			sb.AppendLine($"Is Visible in Lobby: {currentRoom.IsVisible}");
			sb.AppendLine($"Is Open to Join: {currentRoom.IsOpen}");
			sb.AppendLine($"Master Client ID: {currentRoom.MasterClientId}");

			if (currentRoom.CustomProperties.TryGetValue("gameMode", out object gameModeValue))
			{
				sb.AppendLine($"Game Mode (Raw): {gameModeValue}");
			}

			sb.AppendLine("\n--- Custom Room Properties ---");
			if (currentRoom.CustomProperties != null && currentRoom.CustomProperties.Count > 0)
			{
				foreach (var property in currentRoom.CustomProperties)
				{
					sb.AppendLine($"[{property.Key}]: {property.Value}");
				}
			}
			else
			{
				sb.AppendLine("No custom properties found.");
			}

			sb.AppendLine("\n--- Player Roster ---");
			if (PhotonNetwork.PlayerList != null)
			{
				foreach (Player player in PhotonNetwork.PlayerList)
				{
					string masterTag = player.IsMasterClient ? " [Master Client]" : "";
					string localTag = player.IsLocal ? " [You]" : "";
					sb.AppendLine($"- ID #{player.ActorNumber}: {player.NickName}{masterTag}{localTag}");
				}
			}

			sb.AppendLine("=============================");

			// Print the complete report to the console/LogOutput.log
			Debug.Log(sb.ToString());
		}
	}
}