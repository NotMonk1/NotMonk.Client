using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using NotMonk_Client.Classes;
using UnityEngine;
using UnityEngine.XR;
using static NotMonk_Client.Menu.Main;

namespace NotMonk_Client.Mods
{
	public class DebugMods
	{
		public static void GetSelfID()
		{
			Debug.Log(Constants.Name + ".Debug: Player ID Got: " + PhotonNetwork.LocalPlayer.UserId);
			GUIUtility.systemCopyBuffer = PhotonNetwork.LocalPlayer.UserId;
			Debug.Log(Constants.Name + ".Debug: Copied ID");
		}
	}
}
