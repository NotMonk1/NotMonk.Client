using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;
using UnityEngine.Networking;

public class MenuSoundRunner : MonoBehaviour
{
}

public static class OpenMenuSound
{
	private const int RequestTimeoutSeconds = 10;

	// Guards against two overlapping downloads racing on the same temp file
	// (e.g. rapid open/close toggling before the first download finishes).
	private static readonly HashSet<string> downloadsInProgress = new HashSet<string>();

	private static readonly string SoundsFolder =
		Path.Combine(
			Paths.GameRootPath,
			"NotMonk Client",
			"Sounds");

	private const string MenuOpenFileName =
		"menu_open.wav";

	private const string MenuCloseFileName =
		"menu_close.wav";

	private const string MenuOpenDownloadUrl =
		"https://raw.githubusercontent.com/NotMonk1/notmonk-client-assets/main/menu_open.wav";

	private const string MenuCloseDownloadUrl =
		"https://raw.githubusercontent.com/NotMonk1/notmonk-client-assets/main/menu_close.wav";

	private static MenuSoundRunner runner;
	private static AudioSource audioSource;

	public static void PlaySound(string soundFileName)
	{
		if (string.IsNullOrWhiteSpace(soundFileName))
			return;

		CreateRunner();

		runner.StartCoroutine(
			EnsureAndPlaySound(soundFileName));
	}

	private static void CreateRunner()
	{
		if (runner != null)
			return;

		GameObject soundObject =
			new GameObject("NotMonk_MenuSoundPlayer");

		soundObject.SetActive(true);

		UnityEngine.Object.DontDestroyOnLoad(
			soundObject);

		runner =
			soundObject.AddComponent<MenuSoundRunner>();

		runner.enabled =
			true;

		audioSource =
			soundObject.AddComponent<AudioSource>();

		audioSource.enabled =
			true;

		audioSource.playOnAwake =
			false;

		// 0 = 2D sound, so it is not affected by menu/player position.
		audioSource.spatialBlend =
			0f;

		audioSource.volume =
			1f;

		audioSource.mute =
			false;

		audioSource.loop =
			false;

		audioSource.bypassEffects =
			true;

		audioSource.bypassListenerEffects =
			true;

		audioSource.bypassReverbZones =
			true;

		audioSource.outputAudioMixerGroup =
			null;
	}

	private static IEnumerator EnsureAndPlaySound(
		string soundFileName)
	{
		string localSoundPath =
			GetLocalSoundPath(soundFileName);

		if (string.IsNullOrEmpty(localSoundPath))
		{
			Debug.LogError(
				$"[NotMonk Client] Unknown menu sound requested: {soundFileName}");

			yield break;
		}

		if (!File.Exists(localSoundPath))
		{
			// If another call is already downloading this exact file, wait for it
			// instead of racing a second download onto the same temp path.
			if (downloadsInProgress.Contains(localSoundPath))
			{
				float waitedSeconds = 0f;
				while (downloadsInProgress.Contains(localSoundPath) && waitedSeconds < RequestTimeoutSeconds)
				{
					yield return null;
					waitedSeconds += Time.unscaledDeltaTime;
				}
			}
			else
			{
				downloadsInProgress.Add(localSoundPath);

				Debug.Log(
					$"[NotMonk Client] Menu sound missing; downloading: {soundFileName}");

				yield return DownloadSound(
					soundFileName,
					localSoundPath);

				downloadsInProgress.Remove(localSoundPath);
			}
		}

		if (!File.Exists(localSoundPath))
		{
			Debug.LogError(
				$"[NotMonk Client] Menu sound could not be found or downloaded: {localSoundPath}");

			yield break;
		}

		yield return LoadAndPlayLocalWav(
			localSoundPath);
	}

	private static string GetLocalSoundPath(
		string soundFileName)
	{
		switch (soundFileName.ToLowerInvariant())
		{
			case "menu_open.wav":
				return Path.Combine(
					SoundsFolder,
					MenuOpenFileName);

			case "menu_close.wav":
				return Path.Combine(
					SoundsFolder,
					MenuCloseFileName);

			default:
				return null;
		}
	}

	private static string GetDownloadUrl(
		string soundFileName)
	{
		switch (soundFileName.ToLowerInvariant())
		{
			case "menu_open.wav":
				return MenuOpenDownloadUrl;

			case "menu_close.wav":
				return MenuCloseDownloadUrl;

			default:
				return null;
		}
	}

	private static IEnumerator DownloadSound(
		string soundFileName,
		string destinationPath)
	{
		string downloadUrl =
			GetDownloadUrl(soundFileName);

		if (string.IsNullOrWhiteSpace(downloadUrl))
		{
			Debug.LogError(
				$"[NotMonk Client] No download URL exists for: {soundFileName}");

			yield break;
		}

		string destinationDirectory =
			Path.GetDirectoryName(destinationPath);

		try
		{
			if (!Directory.Exists(destinationDirectory))
			{
				Directory.CreateDirectory(
					destinationDirectory);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(
				$"[NotMonk Client] Could not create sound directory '{destinationDirectory}': {exception}");

			yield break;
		}

		string temporaryPath =
			destinationPath + ".download";

		try
		{
			if (File.Exists(temporaryPath))
				File.Delete(temporaryPath);
		}
		catch (Exception exception)
		{
			Debug.LogError(
				$"[NotMonk Client] Could not remove old temporary sound file: {exception}");

			yield break;
		}

		using (UnityWebRequest request =
			   new UnityWebRequest(
				   downloadUrl,
				   UnityWebRequest.kHttpVerbGET))
		{
			DownloadHandlerFile downloadHandler =
				new DownloadHandlerFile(
					temporaryPath);

			downloadHandler.removeFileOnAbort =
				true;

			request.downloadHandler =
				downloadHandler;

			request.timeout =
				RequestTimeoutSeconds;

			yield return request.SendWebRequest();

			if (request.result !=
				UnityWebRequest.Result.Success)
			{
				Debug.LogError(
					$"[NotMonk Client] Failed to download {soundFileName}: " +
					$"{request.error} | URL: {downloadUrl}");

				TryDeleteFile(
					temporaryPath);

				yield break;
			}
		}

		try
		{
			if (File.Exists(destinationPath))
				File.Delete(destinationPath);

			File.Move(
				temporaryPath,
				destinationPath);

			Debug.Log(
				$"[NotMonk Client] Downloaded menu sound: {destinationPath}");
		}
		catch (Exception exception)
		{
			Debug.LogError(
				$"[NotMonk Client] Could not save downloaded sound '{soundFileName}': {exception}");

			TryDeleteFile(
				temporaryPath);
		}
	}

	private static IEnumerator LoadAndPlayLocalWav(
		string localSoundPath)
	{
		if (audioSource == null)
		{
			Debug.LogError(
				"[NotMonk Client] Menu AudioSource was null.");

			yield break;
		}

		audioSource.gameObject.SetActive(
			true);

		audioSource.enabled =
			true;

		audioSource.mute =
			false;

		string fileUri =
			new Uri(localSoundPath).AbsoluteUri;

		using (UnityWebRequest request =
			   UnityWebRequestMultimedia.GetAudioClip(
				   fileUri,
				   AudioType.WAV))
		{
			request.timeout =
				RequestTimeoutSeconds;

			yield return request.SendWebRequest();

			if (request.result !=
				UnityWebRequest.Result.Success)
			{
				Debug.LogError(
					$"[NotMonk Client] Could not load WAV file '{localSoundPath}': {request.error}");

				yield break;
			}

			AudioClip audioClip =
				DownloadHandlerAudioClip.GetContent(
					request);

			if (audioClip == null)
			{
				Debug.LogError(
					$"[NotMonk Client] WAV became a null AudioClip: {localSoundPath}");

				yield break;
			}

			Debug.Log(
				$"[NotMonk Client] Playing menu sound: {localSoundPath} | " +
				$"Length: {audioClip.length:F2}s | " +
				$"Channels: {audioClip.channels} | " +
				$"Frequency: {audioClip.frequency}Hz");

			audioSource.PlayOneShot(
				audioClip);
		}
	}

	private static void TryDeleteFile(
		string filePath)
	{
		try
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
		catch
		{
		}
	}
}