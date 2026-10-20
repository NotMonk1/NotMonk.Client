using NotMonk_Client.Other;
using Photon.Pun;
using Puppy.Notifications;

namespace NotMonk_Client.Admin
{
	public class WelcomeAdmin : MonoBehaviourPunCallbacks
	{
		/*
		 * Dev Note: Next update try and fix this so it doesn't send on every
		 * Player Join/Leave and try to make it ignore self
		 */
		public static void WelcomeAdminNotif()
		{
			int Tries = 10;
			if (Tries != 0)
			{
				if (GetAdmins.AdminIDs.Contains(PlayerIdAfterLogin.Instance.PlayerId))
				{
					int index = GetAdmins.AdminIDs.IndexOf(PlayerIdAfterLogin.Instance.PlayerId);
					string adminName = GetAdmins.AdminNames[index];
					NotifiLib.SendNotification($"Welcome back, {adminName}! Your mods have been loaded!");
				}
				else
				{
					Tries--;
					WelcomeAdminNotif();
				}
			}
		}
	}
}