/*
 * NotMonk Client - Advantage.cs
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

using GorillaGameModes;
using UnityEngine;

namespace NotMonk_Client.Mods
{
	public abstract class Advantage
	{
		public static void TagPlayer(VRRig p)
		{
			if (p != GorillaTagger.Instance.offlineVRRig)
			{
				if (!p.mainSkin.material.name.Contains("fected"))
				{
					GameMode.ReportTag(p.OwningNetPlayer);

					GorillaTagger.Instance.offlineVRRig.enabled = false;
					GorillaTagger.Instance.offlineVRRig.transform.position = p.headConstraint.position;
				}
				else
				{
					GorillaTagger.Instance.offlineVRRig.enabled = true;
				}
			}
		}
		public static void TagAll()
		{
			foreach (VRRig p in VRRigCache.ActiveRigs)
			{
				if (p != GorillaTagger.Instance.offlineVRRig)
				{
					TagPlayer(p);
				}
			}
		}
		public static void TagSelf()
		{
			if (ControllerInputPoller.instance.rightControllerTriggerButton)
			{
				if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected"))
				{
					foreach (VRRig rig in VRRigCache.ActiveRigs)
					{
						if (rig.mainSkin.material.name.Contains("fected"))
						{
							GorillaTagger.Instance.offlineVRRig.enabled = false;
							GorillaTagger.Instance.offlineVRRig.transform.position = rig.rightHandTransform.position;
							GameMode.ReportTag(GorillaTagger.Instance.offlineVRRig.OwningNetPlayer);
							break;
						}
					}
				}
			}
			else
			{
				GorillaTagger.Instance.offlineVRRig.enabled = true;
			}
		}
	}
}
