/*
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NotMonk_Client.Other
{
	internal class GetUsername
	{
		public static string PlayerName
		{
			get
			{
				if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.LocalPlayer != null && !string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName))
				{
					return PhotonNetwork.LocalPlayer.NickName;
				}

				return PlayerPrefs.GetString("GorillaPlayerName", "Gorilla" + UnityEngine.Random.Range(1000, 9999));
			}
			set
			{
				PlayerPrefs.SetString("GorillaPlayerName", value);
				PlayerPrefs.Save();

				if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.LocalPlayer != null)
				{
					PhotonNetwork.LocalPlayer.NickName = value;
				}
			}
		}

		void Start()
		{
			Debug.Log("Current Player Name Variable: " + PlayerName);
		}
	}
}
*/
