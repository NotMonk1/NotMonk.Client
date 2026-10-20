using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;
using static NotMonk_Client.Menu.Settings;

namespace Puppy.Notifications;

[BepInPlugin("org.gorillatag.lars.notifications2", "NotificationLibrary", "1.0.5")]
public class NotifiLib : BaseUnityPlugin
{
	public static int NoticationThreshold = 30;

	public static string PreviousNotification;

	private static Text notificationText;

	public static bool IsEnabled = true;

	private readonly Material alertText =
		new(Shader.Find("GUI/Text Shader"));

	private const float NotificationAnimationSpeed = 0.010f;

	private const float NotificationDisplayTime = 5f;

	private static float notificationAnimationTimer;

	private static float notificationDisplayTimer;

	private static string targetNotificationText = "";

	private static string displayedNotificationText = "";

	private static NotificationAnimationState animationState =
		NotificationAnimationState.Hidden;

	private enum NotificationAnimationState
	{
		Hidden,
		Appearing,
		Displaying,
		Disappearing
	}

	private bool hasInit;

	private GameObject hudObj;

	private GameObject hudObj2;

	private GameObject mainCamera;

	private void Awake()
	{
		Logger.LogInfo(
			"Plugin NotificationLibrary is loaded!"
		);
	}

	private void LateUpdate()
	{
		if (!hasInit)
		{
			if (GTPlayer.Instance?.mainCamera == null)
				return;

			Init();

			hasInit = true;
		}

		if (hudObj2 == null ||
			mainCamera == null ||
			notificationText == null)
		{
			return;
		}

		hudObj2.transform.position =
			mainCamera.transform.position;

		hudObj2.transform.rotation =
			mainCamera.transform.rotation;

		UpdateNotificationAnimation();
	}

	private void UpdateNotificationAnimation()
	{
		if (notificationText == null)
			return;

		switch (animationState)
		{
			case NotificationAnimationState.Hidden:

				notificationText.text = "";

				break;

			case NotificationAnimationState.Appearing:

				UpdateAppearingAnimation();

				break;

			case NotificationAnimationState.Displaying:

				UpdateDisplayingAnimation();

				break;

			case NotificationAnimationState.Disappearing:

				UpdateDisappearingAnimation();

				break;
		}
	}

	private void UpdateAppearingAnimation()
	{
		if (string.IsNullOrEmpty(
				targetNotificationText))
		{
			animationState =
				NotificationAnimationState.Hidden;

			notificationText.text = "";

			return;
		}

		notificationAnimationTimer +=
			Time.deltaTime;

		if (notificationAnimationTimer <
			NotificationAnimationSpeed)
		{
			return;
		}

		notificationAnimationTimer = 0f;

		int visibleLength =
			GetVisibleCharacterCount(
				targetNotificationText
			);

		int currentVisibleLength =
			GetVisibleCharacterCount(
				displayedNotificationText
			);

		if (currentVisibleLength <
			visibleLength)
		{
			currentVisibleLength++;

			displayedNotificationText =
				BuildRichTextPrefix(
					targetNotificationText,
					currentVisibleLength
				);

			notificationText.text =
				displayedNotificationText;
		}

		if (currentVisibleLength >=
			visibleLength)
		{
			displayedNotificationText =
				targetNotificationText;

			notificationText.text =
				displayedNotificationText;

			notificationAnimationTimer = 0f;
			notificationDisplayTimer = 0f;

			animationState =
				NotificationAnimationState.Displaying;
		}
	}

	private void UpdateDisplayingAnimation()
	{
		notificationDisplayTimer +=
			Time.deltaTime;

		if (notificationDisplayTimer >=
			NotificationDisplayTime)
		{
			notificationAnimationTimer = 0f;

			animationState =
				NotificationAnimationState.Disappearing;
		}
	}

	private void UpdateDisappearingAnimation()
	{
		if (string.IsNullOrEmpty(
				displayedNotificationText))
		{
			FinishNotification();

			return;
		}

		notificationAnimationTimer +=
			Time.deltaTime;

		if (notificationAnimationTimer <
			NotificationAnimationSpeed)
		{
			return;
		}

		notificationAnimationTimer = 0f;

		int currentVisibleLength =
			GetVisibleCharacterCount(
				displayedNotificationText
			);

		if (currentVisibleLength > 0)
		{
			currentVisibleLength--;

			displayedNotificationText =
				BuildRichTextPrefix(
					targetNotificationText,
					currentVisibleLength
				);

			notificationText.text =
				displayedNotificationText;
		}

		if (currentVisibleLength <= 0)
		{
			FinishNotification();
		}
	}

	private static void FinishNotification()
	{
		if (notificationText != null)
			notificationText.text = "";

		targetNotificationText = "";

		displayedNotificationText = "";

		notificationAnimationTimer = 0f;

		notificationDisplayTimer = 0f;

		animationState =
			NotificationAnimationState.Hidden;
	}

	private static int GetVisibleCharacterCount(
		string text)
	{
		if (string.IsNullOrEmpty(text))
			return 0;

		int count = 0;

		bool insideTag = false;

		for (int i = 0; i < text.Length; i++)
		{
			char character = text[i];

			if (character == '<')
			{
				insideTag = true;
				continue;
			}

			if (character == '>' &&
				insideTag)
			{
				insideTag = false;
				continue;
			}

			if (!insideTag)
				count++;
		}

		return count;
	}
	private static string BuildRichTextPrefix(
		string text,
		int visibleCharacters)
	{
		if (string.IsNullOrEmpty(text) ||
			visibleCharacters <= 0)
		{
			return "";
		}

		StringBuilder result =
			new StringBuilder();

		Stack<string> openTags =
			new Stack<string>();

		int visibleCount = 0;

		for (int i = 0;
			 i < text.Length;
			 i++)
		{
			char character = text[i];

			if (character == '<')
			{
				int tagEnd =
					text.IndexOf(
						'>',
						i
					);

				if (tagEnd == -1)
					break;

				string tag =
					text.Substring(
						i,
						tagEnd - i + 1
					);

				string tagInfo =
					tag.Substring(
						1,
						tag.Length - 2
					);

				bool isClosingTag =
					tagInfo.StartsWith("/");

				bool isSelfClosingTag =
					tagInfo.EndsWith("/");

				if (isClosingTag)
				{
					result.Append(tag);

					if (openTags.Count > 0)
						openTags.Pop();
				}
				else
				{
					result.Append(tag);

					if (!isSelfClosingTag)
					{
						string tagName =
							GetTagName(tagInfo);

						openTags.Push(tagName);
					}
				}

				i = tagEnd;

				continue;
			}

			if (visibleCount >=
				visibleCharacters)
			{
				break;
			}

			result.Append(character);

			visibleCount++;
		}

		while (openTags.Count > 0)
		{
			string tagName =
				openTags.Pop();

			result.Append("</");
			result.Append(tagName);
			result.Append(">");
		}

		return result.ToString();
	}

	private static string GetTagName(
		string tagInfo)
	{
		if (string.IsNullOrEmpty(tagInfo))
			return "";

		int equalsIndex =
			tagInfo.IndexOf('=');

		if (equalsIndex >= 0)
		{
			return tagInfo
				.Substring(0, equalsIndex)
				.Trim();
		}

		int spaceIndex =
			tagInfo.IndexOf(' ');

		if (spaceIndex >= 0)
		{
			return tagInfo
				.Substring(0, spaceIndex)
				.Trim();
		}

		return tagInfo.Trim();
	}

	private void Init()
	{
		mainCamera =
			GTPlayer.Instance.mainCamera.gameObject;

		hudObj = new GameObject();

		hudObj2 = new GameObject();

		hudObj2.name =
			"NOTIFICATIONLIB_HUD_OBJ";

		hudObj.name =
			"NOTIFICATIONLIB_HUD_OBJ";

		hudObj.AddComponent<Canvas>();

		hudObj.AddComponent<CanvasScaler>();

		hudObj.AddComponent<GraphicRaycaster>();

		hudObj.GetComponent<Canvas>().enabled =
			true;

		hudObj.GetComponent<Canvas>().renderMode =
			RenderMode.WorldSpace;

		hudObj.GetComponent<Canvas>().worldCamera =
			mainCamera.GetComponent<Camera>();

		hudObj.GetComponent<RectTransform>()
			.sizeDelta =
			new Vector2(
				5f,
				5f
			);

		hudObj.GetComponent<RectTransform>()
			.position =
			new Vector3(
				mainCamera.transform.position.x,
				mainCamera.transform.position.y,
				mainCamera.transform.position.z
			);

		hudObj.GetComponent<CanvasScaler>()
			.dynamicPixelsPerUnit =
			10f;

		hudObj2.transform.position =
			new Vector3(
				mainCamera.transform.position.x,
				mainCamera.transform.position.y,
				mainCamera.transform.position.z - 4.6f
			);

		hudObj.transform.parent =
			hudObj2.transform;

		hudObj.GetComponent<RectTransform>()
			.localPosition =
			new Vector3(
				0f,
				0f,
				1.6f
			);

		Vector3 eulerAngles =
			hudObj.GetComponent<RectTransform>()
				.rotation.eulerAngles;

		eulerAngles.y = -270f;

		hudObj.transform.localScale =
			new Vector3(
				1f,
				1f,
				1f
			);

		hudObj.GetComponent<RectTransform>()
			.rotation =
			Quaternion.Euler(
				eulerAngles
			);

		notificationText =
			new GameObject
			{
				transform =
				{
					parent = hudObj.transform,
				},
			}
			.AddComponent<Text>();

		notificationText.text = "";

		notificationText.fontSize = 17;

		notificationText.font =
			currentFont;

		notificationText.rectTransform
			.sizeDelta =
			new Vector2(
				450f,
				210f
			);

		notificationText.alignment =
			TextAnchor.MiddleCenter;

		notificationText.supportRichText =
			true;

		notificationText.rectTransform
			.localScale =
			new Vector3(
				0.00333333333f,
				0.00333333333f,
				0.33333333f
			);

		notificationText.rectTransform
			.localPosition =
			new Vector3(
				0f,
				0f,
				-0.5f
			);

		notificationText.material =
			alertText;
	}

	public static void SendNotification(
		string notification)
	{
		if (disableNotifications)
			return;

		try
		{
			if (!IsEnabled)
				return;

			if (notificationText == null)
				return;

			if (PreviousNotification ==
				notification)
			{
				return;
			}

			if (!notification.Contains(
					Environment.NewLine))
			{
				notification +=
					Environment.NewLine;
			}

			PreviousNotification =
				notification;

			notificationText.supportRichText =
				true;

			if (string.IsNullOrEmpty(
					targetNotificationText))
			{
				targetNotificationText =
					notification;

				displayedNotificationText =
					"";

				notificationAnimationTimer =
					0f;

				notificationDisplayTimer =
					0f;

				animationState =
					NotificationAnimationState.Appearing;

				return;
			}


			targetNotificationText +=
				notification;

			if (animationState ==
				NotificationAnimationState.Disappearing)
			{
				notificationAnimationTimer =
					0f;

				animationState =
					NotificationAnimationState.Appearing;
			}

			else if (animationState ==
					 NotificationAnimationState.Displaying)
			{
				notificationAnimationTimer =
					0f;

				animationState =
					NotificationAnimationState.Appearing;
			}
		}
		catch
		{
			Debug.LogError(
				"Notification failed, object probably nil due to third person ; " +
				notification
			);
		}
	}

	public static void ClearAllNotifications()
	{
		if (notificationText == null)
			return;

		notificationText.text = "";

		targetNotificationText = "";

		displayedNotificationText = "";

		PreviousNotification = "";

		notificationAnimationTimer = 0f;

		notificationDisplayTimer = 0f;

		animationState =
			NotificationAnimationState.Hidden;
	}

	public static void ClearPastNotifications(
		int amount)
	{
		if (notificationText == null)
			return;

		if (string.IsNullOrEmpty(
				targetNotificationText))
		{
			return;
		}

		string text =
			targetNotificationText
				.Split(
					Environment.NewLine.ToCharArray()
				)
				.Skip(amount)
				.Where(
					text2 =>
						!string.IsNullOrEmpty(text2)
				)
				.Aggregate(
					"",
					(
						current,
						text2
					) =>
						current +
						text2 +
						"\n"
				);

		targetNotificationText = text;

		if (string.IsNullOrEmpty(
				targetNotificationText))
		{
			ClearAllNotifications();

			return;
		}

		displayedNotificationText = "";

		notificationAnimationTimer = 0f;

		notificationDisplayTimer = 0f;

		animationState =
			NotificationAnimationState.Appearing;
	}
}