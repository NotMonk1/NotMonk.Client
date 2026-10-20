using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NotMonk_Client.Admin
{
	public class NotMonkNetworkReceiver : MonoBehaviour, IOnEventCallback
	{
		private void OnEnable()
		{
			PhotonNetwork.AddCallbackTarget(this);
		}

		private void OnDisable()
		{
			PhotonNetwork.RemoveCallbackTarget(this);
		}

		public void OnEvent(EventData photonEvent)
		{
			NotMonkNetwork.ReceiveEvent(photonEvent);
		}
	}
}