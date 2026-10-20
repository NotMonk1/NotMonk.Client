using UnityEngine;

namespace NotMonk_Client.Other.DiscordRPC
{
	public class RoomEventWatcher : MonoBehaviour
	{
		private static GameObject host;

		private bool subscribed;
		private float refreshAt = -1f;

		public static void Create()
		{
			if (host != null)
				return;

			host = new GameObject("NotMonk RoomEventWatcher");
			DontDestroyOnLoad(host);
			host.AddComponent<RoomEventWatcher>();
		}

		public static void Remove()
		{
			if (host == null)
				return;

			Destroy(host);
			host = null;
		}

		private void Update()
		{
			if (!subscribed)
				TrySubscribe();

			if (refreshAt > 0f && Time.unscaledTime >= refreshAt)
			{
				refreshAt = -1f;
				DiscordRPCManager.Update();
			}

			DiscordRPCManager.Update();
		}

		private void TrySubscribe()
		{
			if (NetworkSystem.Instance == null)
				return;

			NetworkSystem.Instance.OnJoinedRoomEvent += OnRoomChanged;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += OnRoomChanged;
			NetworkSystem.Instance.OnPlayerJoined += OnPlayerChanged;
			NetworkSystem.Instance.OnPlayerLeft += OnPlayerChanged;

			subscribed = true;
		}

		private void OnDestroy()
		{
			if (!subscribed || NetworkSystem.Instance == null)
				return;

			NetworkSystem.Instance.OnJoinedRoomEvent -= OnRoomChanged;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= OnRoomChanged;
			NetworkSystem.Instance.OnPlayerJoined -= OnPlayerChanged;
			NetworkSystem.Instance.OnPlayerLeft -= OnPlayerChanged;

			subscribed = false;
		}

		private void OnRoomChanged()
		{
			refreshAt = Time.unscaledTime + 0.25f;
		}

		private void OnPlayerChanged(NetPlayer player)
		{
			refreshAt = Time.unscaledTime + 0.25f;
		}
	}
}