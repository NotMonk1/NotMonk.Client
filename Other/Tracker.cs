using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Puppy.Notifications;
using StupidTemplate;
using UnityEngine;  

namespace NotMonk_Client.Other
{
	public abstract class RoomTracker
	{
		public static readonly List<string> ccIds = new List<string>()
		{
			"PUT_A_PLAYFAB_ID_HERE",
			"ANOTHER_PLAYER_ID_HERE"
		};

		/*public static void CheckRoomForIds()
		{
			if (!PhotonNetwork.InRoom)
			{
				Debug.LogWarning("Cannot check room: Not currently connected to a Photon room.");
				return;
			}

			foreach (Player targetPlayer in PhotonNetwork.PlayerList)
			{
				string targetId = targetPlayer.UserId;

				if (ccIds.Contains(targetId))
				{
					Debug.Log($"Found a match! {targetPlayer.NickName} has a matching ID: {targetId}");

					NotifiLib.SendNotification($"CONTENT CREATOR DETECTED: {targetPlayer.NickName}. HAS MATCHED ID: {targetId}");
				}

				if (GetAdmins.AdminIDs.Contains(targetId))
				{
					Debug.Log($"Found an admin! {targetPlayer.NickName} is an admin with ID: {targetId}");
					NotifiLib.SendNotification($"MENU ADMIN DETECTED: {targetPlayer.NickName}!");
				}
				else
				{
					Debug.Log($"No match for {targetPlayer.NickName} with ID: {targetId}");
					if (Plugin.isInDebug == true)
					{
						NotifiLib.SendNotification("No ID Matches Found");
					}
				}
			}
		}*/
	}
}
