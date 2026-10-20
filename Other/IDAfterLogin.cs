using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NotMonk_Client.Other
{
	public class PlayerIdAfterLogin : MonoBehaviour
	{
		public static PlayerIdAfterLogin Instance { get; private set; }

		public string PlayerId { get; private set; } = string.Empty;
		public bool IsReady { get; private set; }

		public event Action<string> OnLoginReady;

		private void Awake()
		{
			UnityEngine.Debug.Log("[NotMonk] PlayerIdAfterLogin Awake() fired.");

			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			InvokeRepeating(nameof(CheckForLogin), 0.5f, 0.5f);
		}

		private void CheckForLogin()
		{
			if (IsReady)
				return;

			Player localPlayer = PhotonNetwork.LocalPlayer;

			string photonUserId = localPlayer?.UserId;
			string authUserId = PhotonNetwork.AuthValues?.UserId;

			UnityEngine.Debug.Log(
				$"[NotMonk] Login check | " +
				$"Connected={PhotonNetwork.IsConnected} | " +
				$"LocalPlayer={(localPlayer != null)} | " +
				$"PhotonUserId={photonUserId ?? "NULL"} | " +
				$"AuthUserId={authUserId ?? "NULL"}"
			);

			if (!PhotonNetwork.IsConnected)
				return;

			string playerId = photonUserId;

			if (string.IsNullOrWhiteSpace(playerId))
			{
				playerId = authUserId;
			}

			if (string.IsNullOrWhiteSpace(playerId))
				return;

			PlayerId = playerId.Trim();
			IsReady = true;

			UnityEngine.Debug.Log(
				$"[NotMonk] Photon login complete! Player ID: {PlayerId}"
			);

			OnLoginReady?.Invoke(PlayerId);

			NotMonk_Client.Menu.Buttons.RefreshAdminButtons();

			CancelInvoke(nameof(CheckForLogin));
		}

		private void OnDestroy()
		{
			CancelInvoke(nameof(CheckForLogin));

			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}