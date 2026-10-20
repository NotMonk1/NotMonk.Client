using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NotMonk_Client.Mods
{
	abstract class Important
	{
		public static void ChangeName(string playerName)
		{
			if (string.IsNullOrWhiteSpace(playerName))
				return;

			var computer = GorillaComputer.instance;
			var localPlayer = PhotonNetwork.LocalPlayer;

			if (computer == null || localPlayer == null)
				return;

			computer.currentName = playerName;
			computer.SetLocalNameTagText(playerName);
			computer.savedName = playerName;

			PlayerPrefs.SetString("playerName", playerName);
			PlayerPrefs.Save();

			localPlayer.NickName = playerName;
		}

		public static void SetColor(Color color)
		{
			var rig = VRRig.LocalRig;

			if (rig == null)
				return;

			foreach (var text in rig.GetComponentsInChildren<Text>(true))
			{
				if (text != null)
					text.color = color;
			}

			foreach (var text in rig.GetComponentsInChildren<TMP_Text>(true))
			{
				if (text != null)
					text.color = color;
			}
		}

		public static void ResetColor()
		{
			SetColor(Color.white);
		}
	}
}
