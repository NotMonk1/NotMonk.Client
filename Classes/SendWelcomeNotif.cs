using Photon.Pun;
using Puppy.Notifications;
using System.Collections;
using UnityEngine;

namespace NotMonk_Client.Classes
{
	internal class SendWelcomeNotif : MonoBehaviour
	{
		private bool hasRun;

		private void Start()
		{
			StartCoroutine(WaitForMainCamera());
		}

		private IEnumerator WaitForMainCamera()
		{
			while (Camera.main == null)
			{
				yield return null;
			}

			yield return new WaitForSeconds(2f);

			if (hasRun)
				yield break;

			hasRun = true;

			DoSomething();
		}

		private void DoSomething()
		{
			NotifiLib.SendNotification("Welcome, " + PhotonNetwork.LocalPlayer.NickName + "! Thank you for choosing NotMonk Client");
		}
	}
}
