/*
 * NotMonk Client - Safety.cs
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

using System;
using System.Linq;
using Puppy.Notifications;
using UnityEngine;
using static NotMonk_Client.Classes.RigManager;

namespace NotMonk_Client.Mods
{
    public class Safety
    {
        public static VRRig reportRig;

        public static float antiReportDelay;
        public static void AntiReport(Action<VRRig, Vector3> onReport)
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (reportRig != null)
            {
                onReport?.Invoke(reportRig, reportRig.transform.position);
                reportRig = null;

                return;
            }

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                Transform report = line.reportButton.gameObject.transform;

                foreach (VRRig vrrig in from vrrig in VRRigCache.m_activeRigs where !vrrig.isLocal let D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position) let D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position) where D1 < 0.35f || D2 < 0.35f select vrrig)
                    onReport?.Invoke(vrrig, report.transform.position);
            }
        }
        public static void AntiReportDisconnect() =>
                AntiReport((vrrig, position) =>
                           {
                               NetworkSystem.Instance.ReturnToSinglePlayer();

                               if (!(Time.time > antiReportDelay)) return;
                               antiReportDelay = Time.time + 1f;
                               NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected.");
                           });
    }
}