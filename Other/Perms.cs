using Photon.Pun;
using Puppy.Notifications;

namespace NotMonk_Client.Other
{
	internal class Perms
	{
		public static void CheckPerms()
		{
			if (GetAdmins.AdminIDs.Contains(PlayerIdAfterLogin.Instance.PlayerId))
			{
				NotifiLib.SendNotification($"Hello, {PhotonNetwork.LocalPlayer.NickName}! You are an admin.");
			}
			else
			{
				NotifiLib.SendNotification($"Hello, {PhotonNetwork.LocalPlayer.NickName}! Your a member. Thank you for your support!");
			}
		}
	}
}
