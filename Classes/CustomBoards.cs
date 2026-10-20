using BepInEx;
using TMPro;
using UnityEngine;
using NotMonk_Client.Menu;
using Photon.Pun;

namespace NotMonk_Client.Classes
{
	[BepInPlugin(
		"org.notmonkstudios.notmonkclient.customboards",
		"CustomBoards",
		"1.3.0"
	)]
	internal class CustomBoards : BaseUnityPlugin
	{
		public static string PlayerName = "placeholder_name";
		public static string Heading =
			"Thank's For Using NotMonk Client!";

		public static string Body =
			//$"Welcome, {PlayerName}! Thank you for choosing NotMonk Client!\n" +
			$"You are using build {Constants.Version} of the menu. " +
			"This menu is free. If you paid for this menu you have been scammed.\n\n" +
			"<color=red>By using this menu you agree that you are responsible for any bans using this menu</color>\n\n" +
			$"CURRENT VERSION: {Constants.Version}\n" +
			$"LIVE VERSION: {GetLatest.LiveVer}";

		public static string COCHeading =
			"CODE OF CONDUCT";

		public static string COCBody =
			"code of conduct desc";

		private const string HeadingPath =
			"Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText";

		private const string BodyPath =
			"Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText";

		private const string COCHeadingPath =
			"Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText";

		private const string COCBodyPath =
			"Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductBodyText";

		private GameObject originalHeading;
		private GameObject originalBody;
		private GameObject originalCOCHeading;
		private GameObject originalCOCBody;

		private GameObject customHeading;
		private GameObject customBody;
		private GameObject customCOCHeading;
		private GameObject customCOCBody;

		private bool boardsCreated;

		private void Awake()
		{
			Logger.LogInfo("CustomBoards loaded!");
		}

		private void Update()
		{
			if (!boardsCreated)
			{
				CreateBoards();
				return;
			}

			if (Settings.disableCustomBoards)
			{
				SetCustomBoardsActive(false);
				SetOriginalBoardsActive(true);
			}
			else
			{
				SetOriginalBoardsActive(false);
				SetCustomBoardsActive(true);
			}
		}

		private void CreateBoards()
		{
			originalHeading = GameObject.Find(HeadingPath);
			originalBody = GameObject.Find(BodyPath);
			originalCOCHeading = GameObject.Find(COCHeadingPath);
			originalCOCBody = GameObject.Find(COCBodyPath);

			if (originalHeading == null || originalBody == null)
				return;

			if (customHeading != null || customBody != null)
				return;

			customHeading = Instantiate(
				originalHeading,
				originalHeading.transform.parent
			);

			customHeading.name = "NotMonk_CustomMOTD_Heading";

			SetTMPText(
				customHeading,
				Heading
			);

			DisablePlayFabTextDisplay(
				customHeading
			);

			customBody = Instantiate(
				originalBody,
				originalBody.transform.parent
			);

			customBody.name = "NotMonk_CustomMOTD_Body";

			SetTMPText(
				customBody,
				Body
			);

			DisablePlayFabTextDisplay(
				customBody
			);

			if (originalCOCHeading != null)
			{
				customCOCHeading = Instantiate(
					originalCOCHeading,
					originalCOCHeading.transform.parent
				);

				customCOCHeading.name =
					"NotMonk_CustomCOC_Heading";

				SetTMPText(
					customCOCHeading,
					COCHeading
				);

				DisablePlayFabTextDisplay(
					customCOCHeading
				);
			}

			if (originalCOCBody != null)
			{
				customCOCBody = Instantiate(
					originalCOCBody,
					originalCOCBody.transform.parent
				);

				customCOCBody.name =
					"NotMonk_CustomCOC_Body";

				SetTMPText(
					customCOCBody,
					COCBody
				);

				DisablePlayFabTextDisplay(
					customCOCBody
				);
			}

			SetOriginalBoardsActive(false);
			SetCustomBoardsActive(true);

			boardsCreated = true;

			Logger.LogInfo(
				"Custom MOTD created successfully!"
			);
		}

		private void SetTMPText(
			GameObject obj,
			string text
		)
		{
			if (obj == null)
				return;

			TMP_Text tmp =
				obj.GetComponent<TMP_Text>();

			if (tmp == null)
			{
				Logger.LogWarning(
					$"No TMP_Text found on {obj.name}"
				);

				return;
			}

			tmp.text = text;
			tmp.ForceMeshUpdate();
		}

		private void DisablePlayFabTextDisplay(
			GameObject obj
		)
		{
			if (obj == null)
				return;

			Component component =
				obj.GetComponent(
					"PlayFabTitleDataTextDisplay"
				);

			if (component == null)
				return;

			Behaviour behaviour =
				component as Behaviour;

			if (behaviour != null)
			{
				behaviour.enabled = false;
			}
		}

		private void SetCustomBoardsActive(
			bool active
		)
		{
			if (customHeading != null)
				customHeading.SetActive(active);

			if (customBody != null)
				customBody.SetActive(active);

			//if (customCOCHeading != null)
				//customCOCHeading.SetActive(active);

			//if (customCOCBody != null)
				//customCOCBody.SetActive(active);
		}

		private void SetOriginalBoardsActive(
			bool active
		)
		{
			if (originalHeading != null)
				originalHeading.SetActive(active);

			if (originalBody != null)
				originalBody.SetActive(active);

			if (originalCOCHeading != null)
				originalCOCHeading.SetActive(active);

			if (originalCOCBody != null)
				originalCOCBody.SetActive(active);
		}

		public static void Refresh()
		{
			CustomBoards instance =
				FindObjectOfType<CustomBoards>();

			if (instance == null)
				return;

			instance.ApplyText();
		}

		private void ApplyText()
		{
			if (!boardsCreated)
				return;

			SetTMPText(
				customHeading,
				Heading
			);

			SetTMPText(
				customBody,
				Body
			);

			SetTMPText(
				customCOCHeading,
				COCHeading
			);

			SetTMPText(
				customCOCBody,
				COCBody
			);

			DisablePlayFabTextDisplay(
				customHeading
			);

			DisablePlayFabTextDisplay(
				customBody
			);

			DisablePlayFabTextDisplay(
				customCOCHeading
			);

			DisablePlayFabTextDisplay(
				customCOCBody
			);
		}

		private void OnDestroy()
		{
			SetCustomBoardsActive(false);
			SetOriginalBoardsActive(true);

			if (customHeading != null)
				Destroy(customHeading);

			if (customBody != null)
				Destroy(customBody);

			if (customCOCHeading != null)
				Destroy(customCOCHeading);

			if (customCOCBody != null)
				Destroy(customCOCBody);
		}
	}
}