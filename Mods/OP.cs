/*
 * NotMonk Client - OP.cs
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
using GorillaTagScripts;
using Photon.Pun;
using Puppy.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NotMonk_Client.Mods
{
	public abstract class OPMods
	{
		// Credit to Poison - I used their lucy mods as inspo :p
		public static void PartyKick()
		{
			if (!PhotonNetwork.InRoom) return;
			if (!FriendshipGroupDetection.Instance.IsInParty) return;
			PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("NotMonk Client On Top", JoinType.ForceJoinWithParty);
		}

		private static readonly Dictionary<string, GameObject> ObjectCache = new Dictionary<string, GameObject>();

		public static GameObject GetObject(string path)
		{
			if (ObjectCache.TryGetValue(path, out GameObject cached) && cached != null)
				return cached;

			GameObject found = GameObject.Find(path);

			if (found == null)
			{
				string[] parts = path.Split('/');
				string childPath = string.Join("/", parts.Skip(1));

				for (int i = 0; i < SceneManager.sceneCount && found == null; i++)
				{
					Scene scene = SceneManager.GetSceneAt(i);
					if (!scene.isLoaded) continue;

					foreach (GameObject root in scene.GetRootGameObjects())
					{
						if (root.name != parts[0]) continue;

						Transform t = parts.Length == 1 ? root.transform : root.transform.Find(childPath);
						if (t != null)
						{
							found = t.gameObject;
							break;
						}
					}
				}
			}

			if (found != null)
				ObjectCache[path] = found;

			return found;
		}

		public static IEnumerable<T> GetAllType<T>() where T : UnityEngine.Object
			=> Resources.FindObjectsOfTypeAll<T>();

		private const float GhostSearchCooldown = 5f;
		private const string GhostsPath = "Environment Objects/05Maze_PersistentObjects/2025_Halloween1_PersistentObjects/Halloween Ghosts/";

		private static HalloweenGhostChaser _lucy;
		private static float _lucyNextSearch;

		private static LurkerGhost _lurker;
		private static float _lurkerNextSearch;

		public static HalloweenGhostChaser Lucy
		{
			get
			{
				bool stale = _lucy == null
							 || _lucy.GetView.ViewID == 0
							 || (NetworkSystem.Instance.IsMasterClient && !_lucy.IsMine);

				return FindCached(ref _lucy, ref _lucyNextSearch, stale,
					GhostsPath + "Lucy/Halloween Ghost/FloatingChaseSkeleton",
					() => GetAllType<HalloweenGhostChaser>()
						.Where(g => g != null && g.HasView && g.GetView.ViewID != 0)
						.OrderByDescending(g => g.IsMine)
						.ThenBy(g => g.gameObject.activeInHierarchy ? 0 : 1)
						.FirstOrDefault());
			}
			set => _lucy = value;
		}

		public static LurkerGhost Lurker
		{
			get
			{
				bool stale = _lurker == null || _lurker.GetView.ViewID == 0;

				return FindCached(ref _lurker, ref _lurkerNextSearch, stale,
					GhostsPath + "Lurker Ghost/GhostLurker_Prefab",
					() => GetAllType<LurkerGhost>()
						.FirstOrDefault(g => g != null && g.HasView && g.GetView.ViewID != 0));
			}
			set => _lurker = value;
		}
		private static T FindCached<T>(ref T cache, ref float nextSearch, bool stale, string path, Func<T> fallback)
			where T : Component
		{
			if (!stale || Time.time <= nextSearch)
				return cache;

			nextSearch = Time.time + GhostSearchCooldown;

			GameObject obj = GetObject(path);
			cache = obj != null ? obj.GetComponent<T>() : fallback();

			return cache;
		}
		private static void ModifyLucy(Action<HalloweenGhostChaser> action)
		{
			HalloweenGhostChaser lucy = Lucy;
			if (lucy == null) return;

			if (!lucy.IsMine)
			{
				NotifiLib.SendNotification("YOU ARE NOT MASTER CLIENT!!");
				return;
			}

			action(lucy);
		}

		public static void StartGong(HalloweenGhostChaser lucy, bool summoned)
		{
			lucy.timeGongStarted = Time.time;
			lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
			lucy.isSummoned = summoned;
		}

		public static void SpawnBlueLucy() => ModifyLucy(lucy => StartGong(lucy, false));

		public static void SpawnRedLucy() => ModifyLucy(lucy => StartGong(lucy, true));

		public static void SpawnLucy()
		{
			HalloweenGhostChaser lucy = Lucy;
			if (lucy == null) return;

			if (!lucy.IsMine)
			{
				NotifiLib.SendNotification("YOU ARE NOT MASTER CLIENT!!");
				return;
			}

			if (Time.time <= lucy.grabTime + lucy.grabDuration + 0.1f) return;

			NetPlayer local = NetworkSystem.Instance.LocalPlayer;

			if (lucy.targetPlayer != local)
			{
				lucy.currentState = HalloweenGhostChaser.ChaseState.Dormant;
			}

			lucy.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
			lucy.grabTime = Time.time;
			lucy.targetPlayer = local;
		}

		public static void DespawnLucy() => ModifyLucy(lucy =>
		{
			lucy.currentState = HalloweenGhostChaser.ChaseState.Dormant;
			lucy.isSummoned = false;
		});
	}
}