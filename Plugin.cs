using BepInEx;
using BepInEx.Logging;
using NotMonk_Client.Other;
using NotMonk_Client.Other.DiscordRPC;
using Puppy.Notifications;
using NotMonk_Client.Classes;
using NotMonk_Client.Patches;
using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using UnityEngine;

namespace NotMonk_Client;

[Description(Constants.Description)]
[BepInPlugin(
        Constants.Guid,
        Constants.Name,
        Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    public static bool isInDebug = false;
    public static string debugType = "";
	internal static new ManualLogSource Logger;
	private void Awake()
    {
        Preferences.Load();
        
        gameObject.AddComponent<CoroutineManager>();
        
        GorillaTagger.OnPlayerSpawned(
                OnPlayerSpawned);
        
        Debug.Log(@$"

█▄  █        █   █▄ ▄█             ▀█         ▄▀▀▀▄ ▀█   ▀               █  
█ ▀▄█ ▄▀▀▀▄ ▀█▀▀ █ ▀ █ ▄▀▀▀▄ █▄▀▀▄  █ ▄▀      █      █  ▀█  ▄▀▀▀▄ █▄▀▀▄ ▀█▀▀
█   █ █   █  █ ▄ █   █ █   █ █   █  █▀▄       █   ▄  █   █  █▀▀▀▀ █   █  █ ▄
▀   ▀  ▀▀▀    ▀  ▀   ▀  ▀▀▀  ▀   ▀ ▀▀  ▀       ▀▀▀  ▀▀▀ ▀▀▀  ▀▀▀  ▀   ▀   ▀ 
Made by NotMonk (@notmonk.idi0t) <3
                                                     https://nmv.skyehub.org
                      The Worst Menu Ever - {Constants.Version}

"
            );
		GetLatest.Load();
		CustomBoards.Refresh();
		StartCoroutine(CheckForUpdate());

		Logger.LogInfo("Creating PlayerIdAfterLogin watcher.");

		GameObject loginWatcher = new GameObject("NotMonk Player ID Watcher");
		DontDestroyOnLoad(loginWatcher);

		loginWatcher.AddComponent<PlayerIdAfterLogin>();

		Logger.LogInfo("PlayerIdAfterLogin watcher created.");

		DiscordRPCManager.Enable();
	}

    private void OnApplicationQuit() => Preferences.Save();

	private void OnPlayerSpawned()
	{
		PatchHandler.PatchAll();

		Preferences.ApplyButtonStates();

		NotMonk_Client.Classes.Fineh.GetNick();
	}

	public static void LogConstants()
	{
		Debug.Log($"------------------------" +
			$"NotMonk Client Constants:" +
			$"Name: {Constants.Name}" +
			$"GUID: {Constants.Guid}" +
			$"Discription: {Constants.Description}" +
			$"Version: {Constants.Version}" +
			$"------------------------");
	}
	
	private void Update()
	{
		DiscordRPCManager.Update();
	}

	private void OnDestroy()
	{
		DiscordRPCManager.Disable();
	}
	
	// Lazy ass upd checker
	public const string CurrentVersion = Constants.Version;
	public string UpdateType = GetLatest.LiveType;
	private string VersionURL = GetLatest.LiveVer;

	private IEnumerator CheckForUpdate()
	{
		string latestVersion = GetLatest.LiveVer;

		using (WebClient client = new WebClient())
		{
			try
			{
				latestVersion = client.DownloadString(VersionURL).Trim();
			}
			catch (Exception ex)
			{
				Debug.LogError("[NotMonk] Update check failed: " + ex.Message);
				yield break;
			}
		}

		if (!Version.TryParse(CurrentVersion, out Version current))
		{
			Debug.LogError("[NotMonk] Invalid current version.");
			yield break;
		}

		if (!Version.TryParse(latestVersion, out Version latest))
		{
			Debug.LogError("[NotMonk] Invalid online version.");
			yield break;
		}

		if (latest > current)
		{
			Debug.LogWarning(
				$"[NotMonk] Update available! v{current} -> v{latest}"
			);

			NotifiLib.SendNotification($"Update available!\nv{current} -> v{latest}");

			if (UpdateType == "Major")
			{
				Application.OpenURL("https://pastebin.com/raw/BicZn9x1");
				Application.Quit();
			}
		}
		else
		{
			Debug.Log(
				$"[NotMonk] Up to date! v{current}"
			);
		}
	}
}