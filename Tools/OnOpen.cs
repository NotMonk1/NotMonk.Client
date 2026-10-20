using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static NotMonk_Client.Menu.Settings;

namespace NotMonk_Client.Tools
{
	// Gorilla Tag only has the new Input System enabled, so UnityEngine.Input.GetKey throws.
	// Use this anywhere you currently call Input.GetKey(KeyCode.X).
	public static class KeyInput
	{
		public static bool Held(KeyCode code)
		{
			Keyboard kb = Keyboard.current;
			if (kb == null) return false;
			if (!Enum.TryParse<Key>(code.ToString(), out Key key) || key == Key.None) return false;
			return kb[key].isPressed;
		}
	}

	public static class OnOpen
	{
		private static bool isMenuOpen;

		// Call once per frame, e.g. as the first line of Main.Prefix().
		public static void PlaySoundOnOpen()
		{
			if (ControllerInputPoller.instance == null) return;

			bool left = ControllerInputPoller.instance.leftControllerPrimaryButton;
			bool right = ControllerInputPoller.instance.rightControllerPrimaryButton;
			bool openInput = (left && right) || KeyInput.Held(keyboardButton);

			if (openInput == isMenuOpen) return;
			isMenuOpen = openInput;

			Debug.Log("[MenuSound] " + (isMenuOpen ? "open" : "close") + " input detected");
			if (disableMenuSound) return;

			// OpenMenuSound is the global-namespace downloader class (NotMonk_MenuSoundPlayer).
			OpenMenuSound.PlaySound(isMenuOpen ? "menu_open.wav" : "menu_close.wav");
		}
	}
}