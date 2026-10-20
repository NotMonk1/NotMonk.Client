using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Puppy.Notifications;
using UnityEngine;

namespace Puppy.Patches
{
	[HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
	public class JoinPatch : MonoBehaviour
	{
		private static Player oldnewplayer;

		private static void Prefix(Player newPlayer)
		{
			if (newPlayer == null)
				return;

			if (newPlayer != oldnewplayer)
			{
				NotifiLib.SendNotification(
					"<color=#B8B8B8>♡</color> " +
					"<color=#7CFF9B>JOINED</color> " +
					"<color=white>" + newPlayer.NickName + "</color>"
				);

				oldnewplayer = newPlayer;

				//NotMonk_Client.Other.RoomTracker.CheckRoomForIds();
			}
		}
	}
}