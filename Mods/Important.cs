/*
 * NotMonk Client - Important.cs
 *
 * Copyright (C) 2026 NotMonk
 *
 * This program is free software, you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see:
 * https://www.gnu.org/licenses/
 *
 * GPL v3.0:
 * https://www.gnu.org/licenses/gpl-3.0.en.html
 *
 * SOCIALS:
 * Discord: @notmonk.idi0t
 * YouTube: @notmonk1
 * GitHub: @notmonk
 * TikTok: @notmonk_the_idiot
 */

using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NotMonk_Client.Mods
{
	abstract class Important
	{
		public static void ChangeName(string playerName)
		{
			if (string.IsNullOrWhiteSpace(playerName))
				return;

			var computer = GorillaComputer.instance;
			var localPlayer = PhotonNetwork.LocalPlayer;

			if (computer == null || localPlayer == null)
				return;

			computer.currentName = playerName;
			computer.SetLocalNameTagText(playerName);
			computer.savedName = playerName;

			PlayerPrefs.SetString("playerName", playerName);
			PlayerPrefs.Save();

			localPlayer.NickName = playerName;
		}

		public static void SetColor(Color color)
		{
			var rig = VRRig.LocalRig;

			if (rig == null)
				return;

			foreach (var text in rig.GetComponentsInChildren<Text>(true))
			{
				if (text != null)
					text.color = color;
			}

			foreach (var text in rig.GetComponentsInChildren<TMP_Text>(true))
			{
				if (text != null)
					text.color = color;
			}
		}

		public static void ResetColor()
		{
			SetColor(Color.white);
		}
	}
}
