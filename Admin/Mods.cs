using System;

namespace NotMonk_Client.Admin
{
	internal class Mods
	{
		public static void Awake()
		{
			Action command = OnKickAll;
			NotMonkNetwork.RegisterCommand("KickAllMenuUsers", command);
		}

		public static void OnKickAll()
		{
			NetworkSystem.Instance.ReturnToSinglePlayer();
		}
	}
}