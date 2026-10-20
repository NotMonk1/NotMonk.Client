using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Puppy.Notifications;
using UnityEngine;

namespace Puppy.Patches
{
	[HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
	public class LeavePatch : MonoBehaviour
	{
		private static Player a;

		private static void Prefix(Player otherPlayer)
		{
			if (otherPlayer == null)
				return;

			if (otherPlayer != PhotonNetwork.LocalPlayer &&
				otherPlayer != a)
			{
				NotifiLib.SendNotification(
					"<color=#B8B8B8>♡</color> " +
					"<color=#FF7C7C>LEFT</color> " +
					"<color=white>" + otherPlayer.NickName + "</color>"
				);

				a = otherPlayer;
			}
		}
	}
}