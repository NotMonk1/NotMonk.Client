using Photon.Pun;
using Puppy.Notifications;
using NotMonk_Client.Classes;

namespace NotMonk_Client.Classes
{
	internal class Fineh
	{
		public static void GetNick()
		{
			if (CustomBoards.PlayerName == "placeholder_name")
			{
				CustomBoards.PlayerName = PhotonNetwork.NickName;
				NotifiLib.SendNotification("Loaded Nickname:" + PhotonNetwork.NickName);
			}
			CustomBoards.Refresh();
		}
	}
}
