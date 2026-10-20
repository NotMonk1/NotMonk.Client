using GorillaNetworking;
using NotMonk_Client.Other;
using NotMonk_Client.Other.DiscordRPC;
using NotMonk_Client.Mods;
using Photon.Pun;
using Puppy.Notifications;
using NotMonk_Client.Classes;
using NotMonk_Client.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static NotMonk_Client.Menu.Main;
using static NotMonk_Client.Menu.Settings;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;
using Movement = NotMonk_Client.Mods.Settings.Movement;

namespace NotMonk_Client.Menu;

public abstract class Buttons
{
	#region bs 
	/*
     * Here is where all of your buttons are located.
     *
     * Move to Category:
     *   new(){buttonText = "Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Opens the main settings page for the menu."},
     *
     * Togglable Mod:
     *   new(){buttonText = "Platforms", method = () => Mods.Movement.Platforms(), toolTip = "Spawns platforms on your hands when pressing grip."},
     *
     * Action:
     *   new(){buttonText = "Disconnect", method = () => NetworkSystem.Instance.ReturnToSinglePlayer(), mode = ButtonMode.Action, toolTip = "Disconnects you from the room."},
     *
     * Incremental:
     *   new(){buttonText = "Change Fly Speed", mode = ButtonMode.Incremental, incrementMethod = Movement.ChangeFlySpeed, displayText = () => $"Change Fly Speed [{Movement.FlySpeedName}]", toolTip = "Changes the speed of the fly mod."},
     */
	#endregion

	private static ButtonInfo[] MainButtons =
	{
		new(){buttonText = "Join Our Discord!", method = () => Application.OpenURL("https://discord.gg/qAW5Py7p4f"), mode = ButtonMode.Action, toolTip = "Opens the discord invite"},
		new(){buttonText = "Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Opens the main settings page for the menu."},
		new(){buttonText = "Room Mods", method = () => SetCategory("Room Mods"), mode = ButtonMode.Action, toolTip = "Opens the room mods tab."},
		new(){buttonText = "Movement Mods", method = () => SetCategory("Movement Mods"), mode = ButtonMode.Action, toolTip = "Opens the movement mods tab."},
		new(){buttonText = "Safety Mods", method = () => SetCategory("Safety Mods"), mode = ButtonMode.Action, toolTip = "Opens the safety mods tab."},
		new(){buttonText = "Visual Mods", method = () => SetCategory("Visual Mods"), mode = ButtonMode.Action, toolTip = "Opens visual mods"},
		new(){buttonText = "Fun Mods", method = () => SetCategory("Fun Mods"), mode = ButtonMode.Action, toolTip = "Opens the fun mods tab."},
		new(){buttonText = "Advantage Mods", method = () => SetCategory("Advantage Mods"), mode = ButtonMode.Action, toolTip = "Opens the advantage mods tab."},
		new(){buttonText = "Overpowered Mods", method = () => SetCategory("Overpowered Mods"), mode = ButtonMode.Action, toolTip = "Opens the overpowered mods tab."},
		new(){buttonText = "<color=red>MASTER</color> Mods", method = () => SetCategory("Master Mods"), mode = ButtonMode.Action, toolTip = "Opens the master mods tab."},
		new(){buttonText = "Debug", method = () => SetCategory("Debug"), mode = ButtonMode.Action, toolTip = "Opens the debug tab."},
		new(){buttonText = "Credits", method = () => SetCategory("Credits"), mode = ButtonMode.Action, toolTip = "Opens the credits tab."},
	};

	private static readonly ButtonInfo[] SettingsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu."},
		new(){buttonText = "Menu", method = () => SetCategory("Menu Settings"), mode = ButtonMode.Action, toolTip = "Opens the settings for the menu."},
		new(){buttonText = "Movement", method = () => SetCategory("Movement Settings"), mode = ButtonMode.Action, toolTip = "Opens the movement settings for the menu."},
		new(){buttonText = "Discord RPC", method = () => SetCategory("RPC Settings"), mode = ButtonMode.Action, toolTip = "Opens the rpc settings for the discord presance."},
	};

	private static readonly ButtonInfo[] DRPCSettingsButtons =
	{
		new(){buttonText = "Return to Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Returns to the main settings page for the menu."},
		new(){buttonText = "Hide Room Code in RPC", enableMethod = () => RPChideRoomCode = true, disableMethod = () => RPChideRoomCode = false, toolTip = "Hides the room you're in on the Discord RPC"},
	};

	private static readonly ButtonInfo[] MenuSettingsButtons =
	{
		new(){buttonText = "Return to Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Returns to the main settings page for the menu."},
		new(){buttonText = "Right Hand", enableMethod = () => rightHanded = true, disableMethod = () => rightHanded = false, enabled = rightHanded, toolTip = "Puts the menu on your right hand."},
		new(){buttonText = "Notifications", enableMethod = () => disableNotifications = false, disableMethod = () => disableNotifications = true, enabled = !disableNotifications, toolTip = "Toggles the notifications."},
		new(){buttonText = "Clear Notifications", method = NotifiLib.ClearAllNotifications, toolTip = "Clears all menu notifications"},
		new(){buttonText = "FPS Counter", enableMethod = () => fpsCounter = true, disableMethod = () => fpsCounter = false, enabled = fpsCounter, toolTip = "Toggles the FPS counter."},
		new(){buttonText = "Discord RPC", enableMethod = DiscordRPCManager.Enable, disableMethod = DiscordRPCManager.Disable, toolTip = "Toggles the menu's rich presance on Discord."},
		new(){buttonText = "Disconnect Button", enableMethod = () => disconnectButton = true, disableMethod = () => disconnectButton = false, enabled = disconnectButton, toolTip = "Toggles the disconnect button."},
		new(){buttonText = "Change Menu Font", method = () => SetCategory("MenuFonts"), mode = ButtonMode.Action, toolTip = "Opens the font changer page"},
		new(){buttonText = "Disable Menu Sounds", enableMethod = () => disableMenuSound = true, disableMethod = () => disableMenuSound = false, enabled = disableMenuSound, toolTip = "Toggles the menus open/close sound."},
		new(){buttonText = "Incremental Buttons", enableMethod = () => incrementalButtons = true, disableMethod = () => incrementalButtons = false, enabled = incrementalButtons, toolTip = "Shows separate minus and plus controls for incremental settings."},
		new(){buttonText = "Rounded Menu", enableMethod = () => roundedButtons = true, disableMethod = () => roundedButtons = false, enabled = roundedButtons, toolTip = "Rounds the menu, buttons, search keyboard, and other menu objects."},
		new(){buttonText = "Drop Menu", enableMethod = () => dropMenu = true, disableMethod = () => dropMenu = false, enabled = dropMenu, toolTip = "Makes the menu open while held and drop when released."},
		new(){buttonText = "Animated Menu", enableMethod = () => animateMenu = true, disableMethod = () => animateMenu = false, enabled = animateMenu, toolTip = "Adds a simple grow and shrink animation to the menu."},
		new(){buttonText = "Gradient Animation", mode = ButtonMode.Incremental, incrementMethod = ChangeGradientAnimation, displayText = () => $"Gradient Animation [{GradientAnimationName}]", toolTip = "Changes how animated gradients move."},
		new(){buttonText = "Color Scheme", mode = ButtonMode.Incremental, incrementMethod = ChangeTheme, displayText = () => $"Color Scheme [{ThemeName}]", toolTip = "Changes the color scheme used by the menu."},
		new(){buttonText = "Rainbow Colors", enableMethod =() => SetRainbowColors(true), disableMethod =() => SetRainbowColors(false), enabled = rainbowColors, toolTip = "Uses animated rainbow colors instead of the selected color scheme."},
		new(){buttonText = "<color=red><b>Epileptic Colors</b></color>", enableMethod =() => new ExtGradient { epileptic = true }, disableMethod =() => new ExtGradient { epileptic = false }, toolTip = "Makes the menu color flash" },
		new(){buttonText = "Rig Colors", enableMethod =() => new ExtGradient { copyRigColor = true }, disableMethod =() => new ExtGradient { copyRigColor = false }, toolTip = "Sets the menus colors to your color code"},
		new(){buttonText = "Disable Gradient", enableMethod = () => gradientSpeed = 0f, disableMethod = () => gradientSpeed = 0.5f, toolTip = "Disables the menu gradient"},
		new(){buttonText = "Outlines", enableMethod = () => outlines = true, disableMethod = () => outlines = false, enabled = outlines, toolTip = "Adds outlines around the menu and its buttons."},
		new(){buttonText = "Button Gradients", enableMethod = () => buttonGradients = true, disableMethod = () => buttonGradients = false, enabled = buttonGradients, toolTip = "Toggles spatial gradients on buttons."},
		new(){buttonText = "Vertical Gradients", enableMethod = () => verticalButtonGradients = true, disableMethod = () => verticalButtonGradients = false, enabled = verticalButtonGradients, toolTip = "Changes button gradients between horizontal and vertical."},
		new(){buttonText = "Top Page Buttons", enableMethod = () => pageButtonsAtTop = true, disableMethod = () => pageButtonsAtTop = false, enabled = pageButtonsAtTop, toolTip = "Moves the page buttons into the first two normal button slots."},
	};

	private static readonly ButtonInfo[] FontButtons =
	{
		new(){buttonText = "Return To Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Returns to the main settings page for the menu."},
		new(){buttonText = "REMINDER", mode = ButtonMode.Action, overlapText = "THESE DONT SAVE!!"},
		new(){buttonText = "Reset Font", method = () => currentFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font, mode = ButtonMode.Action},
		new(){buttonText = "Comic Sans", method = () => currentFont = Font.CreateDynamicFontFromOSFont("Comic Sans MS", 24), mode = ButtonMode.Action},
		new(){buttonText = "Verdana", method = () => currentFont = Font.CreateDynamicFontFromOSFont("Verdana", 24), mode = ButtonMode.Action},
		new(){buttonText = "Times New Roman", method = () => currentFont = Font.CreateDynamicFontFromOSFont("Times New Roman", 24), mode = ButtonMode.Action},
		new(){buttonText = "Ink Free", method = () => currentFont = Font.CreateDynamicFontFromOSFont("Ink Free", 16), mode = ButtonMode.Action},
	};

	private static readonly ButtonInfo[] MovementSettingsButtons =
	{
		new(){buttonText = "Return to Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, toolTip = "Returns to the main settings page for the menu."},
		new(){buttonText = "Change Fly Speed", mode = ButtonMode.Incremental, incrementMethod = Movement.ChangeFlySpeed, displayText = () => $"Change Fly Speed [{Movement.FlySpeedName}]", toolTip = "Changes the speed of the fly mod."},
		new(){buttonText = "Change Speedboost Speed", mode = ButtonMode.Incremental, incrementMethod = Movement.ChangeSpeedBoost, displayText = () => $"Change Speedboost Speed [{Movement.SpeedBoostName}]", toolTip = "Changes the speed of the speedboost mod"},
	};

	private static readonly ButtonInfo[] RoomModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu."},
		new(){buttonText = "Disconnect", method = () => NetworkSystem.Instance.ReturnToSinglePlayer(), mode = ButtonMode.Action, toolTip = "Disconnects you from the room."},
		new(){buttonText = "Lobby Hop", method = () => RoomMods.LobbyHop(), mode = ButtonMode.Action, toolTip = "Join new lobby"},
		new(){buttonText = "Join Menu Code", method = () => PhotonNetworkController.Instance.AttemptToAutoJoinSpecificRoom("!-NotMonk Client-!", JoinType.Solo), mode = ButtonMode.Action, toolTip = "Attempts to join the menus code"},
		new(){buttonText = "AntiAFK", method = () => PhotonNetworkController.Instance.disableAFKKick = true, mode = ButtonMode.Toggle, toolTip = "Disabled kicking if AFK"},
		new(){buttonText = "Log Room", method = RoomMods.RoomFile, mode = ButtonMode.Action, toolTip = "Logs the room info and players to a file [<b>INSPO POISON</b>"},
	};

	private static readonly ButtonInfo[] MovementModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu."},
		new(){buttonText = "Platforms", method = Mods.Movement.Platforms, toolTip = "Spawns platforms on your hands when pressing grip."},
		new(){buttonText = "Trigger Platforms", method = Mods.Movement.TriggerPlatforms, toolTip = "Spawns platforms on your hands when pressing your triggers."},
		new(){buttonText = "Speedboost", enableMethod = () => Mods.Movement.SpeedBoostClahhh.SpeedBoost(), disableMethod = () => Mods.Movement.SpeedBoostClahhh.Disable(), toolTip = "Makes you faster"},
		new(){buttonText = "Fly", method = Mods.Movement.Fly, toolTip = "Sends you forward when holding A."},
		new(){buttonText = "Trigger Fly", method = Mods.Movement.TriggerFly, toolTip = "Sends you forward when holding right trigger."},
		new(){buttonText = "Teleport Gun", method = Mods.Movement.TeleportGun, toolTip = "Teleports you to wherever your pointer is when pressing trigger."},
		new(){buttonText = "No-Clip", method = Mods.Movement.Noclip, toolTip = "Makes your gravity like the moon"},
		new(){buttonText = "Long arms <color=green><b>[</b></color> STICK <color=green><b>]</b></color>", enableMethod = Mods.Movement.StickArms, disableMethod = Mods.Movement.DisableLongArms, toolTip = "Makes your arms longer"},
		new(){buttonText = "Long arms <color=green><b>[</b></color> STEAM <color=green><b>]</b></color>", enableMethod = Mods.Movement.LongArms, disableMethod = Mods.Movement.DisableLongArms, toolTip = "Makes your arms longer"},
		new(){buttonText = "WASD Fly", method = Mods.Movement.WASDFly, toolTip = "Makes you fly with W A S D"},
	};

	private static readonly ButtonInfo[] SafetyModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu."},
		new(){buttonText = "Anti Report", method = Safety.AntiReportDisconnect, toolTip = "Disconnects you when someone tries to report you."},
	};

	private static readonly ButtonInfo[] WhatsNewButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
	};

	private static readonly ButtonInfo[] FunModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Set Name \"NMClientOnTop\"", method = () => Important.ChangeName("NMClientOnTop"), mode = ButtonMode.Action, toolTip = "Sets name to \"NMClientOnTop\""},
		new(){buttonText = "Set Name \"Sub2NotMonk\"", method = () => Important.ChangeName("Sub2NotMonk"), mode = ButtonMode.Action, toolTip = "Sets name to \"Sub2NotMonk\""},
		new(){buttonText = "Set Name \"Sub2NotMonk\"", method = () => Important.ChangeName("NMClientUser"), mode = ButtonMode.Action, toolTip = "Sets name to \"NMClientUser\""},
		new(){buttonText = "Change Name Color", incrementMethod = Movement.ChangeNameTagColor, mode = ButtonMode.Incremental, displayText = () => $"NameTag Color [{Movement.NameTagColorName}]", toolTip = "Changes your nametags color"},
	};

	private static readonly ButtonInfo[] OPModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Party Kick Modded Code", method =	Mods.OPMods.PartyKick, mode = ButtonMode.Action, toolTip = "Kicks your party to a modded code"},
	};

	private static readonly ButtonInfo[] MasterModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Spawn Lucy", method = OPMods.SpawnLucy, mode = ButtonMode.Action, toolTip = "Spawns a blue lucy [<b>INSPIRED</b> BY POISON]"},
		new(){buttonText = "Spawn Red Lucy", method = OPMods.SpawnRedLucy, mode = ButtonMode.Action, toolTip = "Spawns a red lucy [<b>INSPIRED</b> BY POISON]"},
		new(){buttonText = "Despawn Lucy", method = OPMods.DespawnLucy, mode = ButtonMode.Action, toolTip = "Despawns lucy [<b>INSPIRED</b> BY POISON]"},
	};

	private static readonly ButtonInfo[] AdvantageModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Tag All", method =	Mods.Advantage.TagAll, mode = ButtonMode.Action, toolTip = "Tags everyone"},
		new(){buttonText = "Tag Self", method =	Mods.Advantage.TagSelf, mode = ButtonMode.Action, toolTip = "Tags you"},
	};

	private static readonly ButtonInfo[] VisualModsButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu" },
		new(){buttonText = "BoxESP", method = Mods.Visual.BoxESP, mode = ButtonMode.Toggle, toolTip = "Shows where people are with a box"},
		new(){buttonText = "Morning Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(1), mode = ButtonMode.Action, toolTip = "Sets your time of day to morning"},
		new(){buttonText = "Day Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(3), mode = ButtonMode.Action, toolTip = "Sets your time of day to daytime"},
		new(){buttonText = "Evening Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(7), mode = ButtonMode.Action, toolTip = "Sets your time of day to evening"},
		new(){buttonText = "Night Time", method =() => BetterDayNightManager.instance.SetTimeOfDay(0), mode = ButtonMode.Action, toolTip = "Sets your time of day to night",},
		new(){buttonText = "Fullbright", enableMethod = Visual.Fullbright.Enable, disableMethod = Visual.Fullbright.Disable, mode = ButtonMode.Action, toolTip = "Eliminates most darkness [<b>INSPIRED</b> BY POISON]"}
	};

	private static readonly ButtonInfo[] DebugButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Get Player ID Self", method = () => DebugMods.GetSelfID(), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Log Constants Stuff", method = Plugin.LogConstants}
	};

	private static readonly ButtonInfo[] CreditButtons =
	{
		new(){buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "<color=cyan><b>NotMonk</b></color>", method = () => SetCategory("NMLinks"), mode = ButtonMode.Action, toolTip = "Owner and main/only dev of NotMonk Client"},
		new(){buttonText = "<color=#f5b5ff><b>SKYE</b></color>", method = () => SetCategory("SKYELinks"), mode = ButtonMode.Action, toolTip = "Let me skid his modified notification lib [insert sob emoji]"},
		new(){buttonText = "<color=#3DB612><b>Poison</b></color>", method = () => Application.OpenURL("https://github.com/heycanihavethis/Poison"), mode = ButtonMode.Action, toolTip = "Used some of their code as inspo :D"},
	};

	private static readonly ButtonInfo[] NMLinksBTNS =
	{
		new(){buttonText = "Return to Credits", method = () => SetCategory("Credits"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = "Project NMV Discord", method = () => Application.OpenURL("https://nmv.skyehub.org"), mode = ButtonMode.Action, toolTip = "NotMonk's main server"},
		new(){buttonText = "GTAG.LOL", method = () => Application.OpenURL("https://gtag.lol/notmonk"), mode = ButtonMode.Action, toolTip = "Social links"},
	};

	private static readonly ButtonInfo[] SKYELinksBTNS =
	{
		new(){buttonText = "Return to Credits", method = () => SetCategory("Credits"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu"},
		new(){buttonText = $"Skyes Main Server", method = () => Application.OpenURL("https://discord.gg/vqTr7gTBrx"), mode = ButtonMode.Action, toolTip = "Skye's main server"},
		new(){buttonText = "GitHub", method = () => Application.OpenURL("https://github.com/skyedotnet"), mode = ButtonMode.Action, toolTip = "GitHub for Skye"},
	};

	private static ButtonInfo[] AdminBS()
	{
		if (!IsAdmin())
			return Array.Empty<ButtonInfo>();

		return new[]
		{
			new ButtonInfo{buttonText = "Return to Main", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu."},
		};
	}

	private static bool IsAdmin()
	{
		if (PlayerIdAfterLogin.Instance == null)
			return false;

		if (!PlayerIdAfterLogin.Instance.IsReady)
			return false;

		string playerId = PlayerIdAfterLogin.Instance.PlayerId;

		if (string.IsNullOrEmpty(playerId))
			return false;

		return GetAdmins.AdminIDs.Contains(playerId);
	}
	private static ButtonInfo[] AdminCategoryButton()
	{
		if (!IsAdmin())
			return Array.Empty<ButtonInfo>();

		return new[]
		{
			new ButtonInfo{buttonText = "Admin", method = () => SetCategory("Admin"), mode = ButtonMode.Action, toolTip = "Opens the admin tab."},
		};
	}

	public static void RefreshAdminButtons()
{
    bool isAdmin = IsAdmin();

    Debug.Log(
        $"[NotMonk] Refreshing admin buttons. " +
        $"IsAdmin={isAdmin}, " +
        $"PlayerId={PlayerIdAfterLogin.Instance?.PlayerId ?? "NULL"}"
    );

    for (int i = 0; i < Categories.Length; i++)
    {
        switch (Categories[i].name)
        {
            case "Main":
                Categories[i].buttons = GetMainButtons();

                Debug.Log(
                    $"[NotMonk] Main buttons refreshed: " +
                    $"{Categories[i].buttons.Length} buttons."
                );
                break;

            case "Admin":
                Categories[i].buttons = AdminBS();

                Debug.Log(
                    $"[NotMonk] Admin buttons refreshed: " +
                    $"{Categories[i].buttons.Length} buttons."
                );
                break;
        }
    }

    RebuildButtonLookups();

    AllButtons = BuildButtonArray();

    Debug.Log(
        $"[NotMonk] AllButtons rebuilt: {AllButtons.Length} total buttons."
    );
}

	private static void RebuildButtonLookups()
	{
		CategoryLookup.Clear();
		ButtonLookup.Clear();
		SavedToggleButtonLookup.Clear();

		foreach (ButtonCategory category in Categories)
		{
			if (!string.IsNullOrWhiteSpace(category.name))
			{
				CategoryLookup[category.name] = category;
			}

			foreach (ButtonInfo button in category.buttons)
			{
				if (string.IsNullOrWhiteSpace(button.buttonText))
					continue;

				ButtonLookup.TryAdd(button.buttonText, button);

				if (button.mode == ButtonMode.Toggle)
				{
					string key = $"{category.name}/{button.buttonText}";
					SavedToggleButtonLookup.TryAdd(key, button);
				}
			}
		}
	}

	private static ButtonInfo[] GetMainButtons()
	{
		List<ButtonInfo> buttons = new(MainButtons);

		ButtonInfo[] adminButtons = AdminCategoryButton();

		if (adminButtons.Length > 0)
		{
			buttons.InsertRange(2, adminButtons);
		}

		return buttons.ToArray();
	}

	private static ButtonInfo[] AdminButtons =
		AdminBS();

	// The order here does not matter, setting categories is done by name rather than position in the array
	private static ButtonCategory[] Categories =
	{
		new(){name = "Main", buttons = GetMainButtons()},
		new(){name = "Settings", buttons = SettingsButtons},
		new(){name = "MenuFonts", buttons = FontButtons},
		new(){name = "Menu Settings", buttons = MenuSettingsButtons},
		new(){name = "RPC Settings", buttons = DRPCSettingsButtons},
		new(){name = "Movement Settings", buttons = MovementSettingsButtons},
		new(){name = "What's New?", buttons = WhatsNewButtons},
		new(){name = "Room Mods", buttons = RoomModsButtons},
		new(){name = "Movement Mods", buttons = MovementModsButtons},
		new(){name = "Safety Mods", buttons = SafetyModsButtons},
		new(){name = "Fun Mods", buttons = FunModsButtons},
		new(){name = "Advantage Mods", buttons = AdvantageModsButtons},
		new(){name = "Visual Mods", buttons = VisualModsButtons},
		new(){name = "Overpowered Mods", buttons = OPModsButtons},
		new(){name = "Master Mods", buttons = MasterModsButtons},
		new(){name = "Debug", buttons = DebugButtons},
		new(){name = "Admin", buttons = AdminButtons},
		new(){name = "Credits", buttons = CreditButtons},
		new(){name = "NMLinks", buttons = NMLinksBTNS},
		new(){name = "SKYELinks", buttons = SKYELinksBTNS},
	};

	private static readonly Dictionary<string, ButtonCategory> CategoryLookup =
		BuildCategoryLookup();

	private static readonly Dictionary<string, ButtonInfo> ButtonLookup =
		BuildButtonLookup();

	private static readonly Dictionary<string, ButtonInfo> SavedToggleButtonLookup =
		BuildSavedToggleButtonLookup();

	public static IReadOnlyDictionary<string, ButtonInfo> SavedToggleButtons =>
		SavedToggleButtonLookup;

	public static ButtonInfo[] AllButtons { get; private set; } =
		BuildButtonArray();

	public static ButtonCategory GetCategory(string categoryName)
	{
		if (string.IsNullOrWhiteSpace(categoryName))
			return null;

		CategoryLookup.TryGetValue(
			categoryName,
			out ButtonCategory category
		);

		return category;
	}

	public static ButtonInfo GetIndex(string buttonText)
	{
		if (string.IsNullOrWhiteSpace(buttonText))
			return null;

		ButtonLookup.TryGetValue(
			buttonText,
			out ButtonInfo button
		);

		return button;
	}

	private static Dictionary<string, ButtonCategory> BuildCategoryLookup()
	{
		Dictionary<string, ButtonCategory> lookup =
			new(StringComparer.OrdinalIgnoreCase);

		foreach (ButtonCategory category in Categories)
		{
			if (string.IsNullOrWhiteSpace(category.name))
				throw new InvalidOperationException(
					"A button category does not have a name."
				);

			if (!lookup.TryAdd(category.name, category))
				throw new InvalidOperationException(
					$"Duplicate button category named {category.name}."
				);
		}

		return lookup;
	}

	private static Dictionary<string, ButtonInfo> BuildButtonLookup()
	{
		Dictionary<string, ButtonInfo> lookup =
			new(StringComparer.OrdinalIgnoreCase);

		foreach (ButtonCategory category in Categories)
		{
			foreach (ButtonInfo button in category.buttons)
			{
				if (string.IsNullOrWhiteSpace(button.buttonText))
					continue;

				lookup.TryAdd(
					button.buttonText,
					button
				);
			}
		}

		return lookup;
	}

	private static ButtonInfo[] BuildButtonArray()
	{
		int buttonCount =
			Categories.Sum(
				category => category.buttons.Length
			);

		ButtonInfo[] result =
			new ButtonInfo[buttonCount];

		int index = 0;

		foreach (ButtonCategory category in Categories)
		{
			foreach (ButtonInfo button in category.buttons)
			{
				result[index] = button;
				index++;
			}
		}

		return result;
	}

	private static Dictionary<string, ButtonInfo> BuildSavedToggleButtonLookup()
	{
		Dictionary<string, ButtonInfo> result =
			new(StringComparer.Ordinal);

		foreach (ButtonCategory category in Categories)
		{
			foreach (ButtonInfo button in category.buttons)
			{
				if (button.mode != ButtonMode.Toggle)
					continue;

				string key =
					category.name +
					"/" +
					button.buttonText;

				if (!result.TryAdd(key, button))
				{
					throw new InvalidOperationException(
						$"Duplicate saved toggle button key {key}."
					);
				}
			}
		}

		return result;
	}
}