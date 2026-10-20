/*
 * NotMonk Client - Movement.cs
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

using BepInEx;
using GorillaLocomotion;
using NotMonk_Client.Classes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static NotMonk_Client.Menu.Main;

namespace NotMonk_Client.Mods
{
	public abstract class Movement
	{
		private static GameObject platl;
		private static GameObject platr;

		public static float armlength { get; private set; }
		public static bool previousTeleportTrigger;
		public static float startX = -1f;
		public static float startY = -1f;
		public static Vector3 lastPosition = Vector3.zero;

		private static readonly List<MeshCollider> meshColliders =
			new List<MeshCollider>();

		private static readonly Dictionary<MeshCollider, bool> originalColliderStates =
			new Dictionary<MeshCollider, bool>();

		private static bool cached;
		private static bool noclipWasOn;

		public static void CacheMeshColliders()
		{
			if (cached)
				return;

			meshColliders.Clear();

			foreach (MeshCollider collider in
					 Resources.FindObjectsOfTypeAll<MeshCollider>())
			{
				if (collider == null)
					continue;

				if (!collider.gameObject.scene.isLoaded)
					continue;

				meshColliders.Add(collider);
			}

			cached = true;
		}

		public static void Fly()
		{
			if (GTPlayer.Instance == null ||
				GorillaTagger.Instance == null ||
				ControllerInputPoller.instance == null)
				return;

			if (!ControllerInputPoller.instance.rightControllerPrimaryButton)
				return;

			GTPlayer.Instance.transform.position +=
				GorillaTagger.Instance.headCollider.transform.forward *
				Time.deltaTime * Settings.Movement.flySpeed;

			GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
		}

		public static void TriggerFly()
		{
			if (GTPlayer.Instance == null ||
				GorillaTagger.Instance == null ||
				ControllerInputPoller.instance == null)
				return;

			if (!ControllerInputPoller.instance.rightControllerTriggerButton)
				return;

			GTPlayer.Instance.transform.position +=
				GorillaTagger.Instance.headCollider.transform.forward *
				Time.deltaTime * Settings.Movement.flySpeed;

			GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
		}

		public static void Platforms()
		{
			if (ControllerInputPoller.instance == null)
				return;

			bool leftGrip = ControllerInputPoller.instance.leftGrab;
			bool rightGrip = ControllerInputPoller.instance.rightGrab;

			if (leftGrip)
			{
				if (platl == null)
				{
					platl = GameObject.CreatePrimitive(PrimitiveType.Cube);
					platl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
					platl.transform.position =
						TrueLeftHand().position + TrueLeftHand().right * 0.05f;
					platl.transform.rotation = TrueLeftHand().rotation;

					FixStickyColliders(platl);

					ColorChanger changer = platl.AddComponent<ColorChanger>();
					changer.colors = Menu.Settings.backgroundColor;
				}
			}
			else if (platl != null)
			{
				Object.Destroy(platl);
				platl = null;
			}

			if (rightGrip)
			{
				if (platr == null)
				{
					platr = GameObject.CreatePrimitive(PrimitiveType.Cube);
					platr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
					platr.transform.position =
						TrueRightHand().position - TrueRightHand().right * 0.05f;
					platr.transform.rotation = TrueRightHand().rotation;

					FixStickyColliders(platr);

					ColorChanger changer = platr.AddComponent<ColorChanger>();
					changer.colors = Menu.Settings.backgroundColor;
				}
			}
			else if (platr != null)
			{
				Object.Destroy(platr);
				platr = null;
			}
		}

		public static void TriggerPlatforms()
		{
			if (ControllerInputPoller.instance == null)
				return;

			bool leftGrip =
				ControllerInputPoller.instance.leftControllerTriggerButton;

			bool rightGrip =
				ControllerInputPoller.instance.rightControllerTriggerButton;

			if (leftGrip)
			{
				if (platl == null)
				{
					platl = GameObject.CreatePrimitive(PrimitiveType.Cube);
					platl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
					platl.transform.position =
						TrueLeftHand().position + TrueLeftHand().right * 0.05f;
					platl.transform.rotation = TrueLeftHand().rotation;

					FixStickyColliders(platl);

					ColorChanger changer = platl.AddComponent<ColorChanger>();
					changer.colors = Menu.Settings.backgroundColor;
				}
			}
			else if (platl != null)
			{
				Object.Destroy(platl);
				platl = null;
			}

			if (rightGrip)
			{
				if (platr == null)
				{
					platr = GameObject.CreatePrimitive(PrimitiveType.Cube);
					platr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
					platr.transform.position =
						TrueRightHand().position - TrueRightHand().right * 0.05f;
					platr.transform.rotation = TrueRightHand().rotation;

					FixStickyColliders(platr);

					ColorChanger changer = platr.AddComponent<ColorChanger>();
					changer.colors = Menu.Settings.backgroundColor;
				}
			}
			else if (platr != null)
			{
				Object.Destroy(platr);
				platr = null;
			}
		}

		public static void TeleportGun()
		{
			if (GTPlayer.Instance == null ||
				GorillaTagger.Instance == null ||
				ControllerInputPoller.instance == null)
				return;

			if (!ControllerInputPoller.instance.rightGrab)
			{
				previousTeleportTrigger = false;
				return;
			}

			var gunData = RenderGun();
			GameObject pointer = gunData.NewPointer;

			if (pointer == null)
				return;

			bool triggerPressed =
				ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;

			if (triggerPressed && !previousTeleportTrigger)
			{
				GTPlayer.Instance.TeleportTo(
					pointer.transform.position + Vector3.up,
					GTPlayer.Instance.transform.rotation);

				GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
			}

			previousTeleportTrigger = triggerPressed;
		}

		internal class SpeedBoostClahhh
		{
			public static float previousJumpMultiplier;
			public static float previousMaxJumpSpeed;

			private static float boostSpeed = Settings.Movement.speedBoost;
			private static bool isEnabled;

			public static void SpeedBoost()
			{
				if (GTPlayer.Instance == null || isEnabled)
					return;

				previousMaxJumpSpeed = GTPlayer.Instance.maxJumpSpeed;
				previousJumpMultiplier = GTPlayer.Instance.jumpMultiplier;

				boostSpeed = Settings.Movement.speedBoost;

				GTPlayer.Instance.maxJumpSpeed = boostSpeed;
				GTPlayer.Instance.jumpMultiplier = 9f;

				isEnabled = true;
			}

			public static void Disable()
			{
				if (GTPlayer.Instance == null || !isEnabled)
					return;

				GTPlayer.Instance.maxJumpSpeed = previousMaxJumpSpeed;
				GTPlayer.Instance.jumpMultiplier = previousJumpMultiplier;

				isEnabled = false;
			}
		}

		public static void DefGrav()
		{
			Physics.gravity = new Vector3(0f, -9.81f, 0f);
		}

		public static void LongArms()
		{
			if (GTPlayer.Instance == null)
				return;

			GTPlayer.Instance.transform.localScale =
				new Vector3(1.2f, 1.2f, 1.2f);
		}

		public static void StickArms()
		{
			if (GTPlayer.Instance == null || GorillaTagger.Instance == null)
				return;

			GTPlayer.Instance.GetControllerTransform(true).position =
				GorillaTagger.Instance.leftHandTransform.position +
				GorillaTagger.Instance.leftHandTransform.forward *
				((armlength - 0.917f) * GTPlayer.Instance.scale);

			GTPlayer.Instance.GetControllerTransform(false).position =
				GorillaTagger.Instance.rightHandTransform.position +
				GorillaTagger.Instance.rightHandTransform.forward *
				((armlength - 0.917f) * GTPlayer.Instance.scale);
		}

		public static void DisableLongArms()
		{
			if (GTPlayer.Instance == null)
				return;

			GTPlayer.Instance.transform.localScale = Vector3.one;
		}


		public static void WASDFly()
		{
			var player = GTPlayer.Instance;
			var tagger = GorillaTagger.Instance;

			if (player == null || tagger == null)
				return;

			var controller = player.GetControllerTransform(false);

			if (controller == null || controller.parent == null)
				return;

			Transform parentTransform = controller.parent;

			bool W = UnityInput.Current.GetKey(KeyCode.W);
			bool A = UnityInput.Current.GetKey(KeyCode.A);
			bool S = UnityInput.Current.GetKey(KeyCode.S);
			bool D = UnityInput.Current.GetKey(KeyCode.D);
			bool Space = UnityInput.Current.GetKey(KeyCode.Space);
			bool Ctrl = UnityInput.Current.GetKey(KeyCode.LeftControl);
			bool Shift = UnityInput.Current.GetKey(KeyCode.LeftShift);
			bool Alt = UnityInput.Current.GetKey(KeyCode.LeftAlt);

			var mouse = Mouse.current;

			if (mouse != null && mouse.rightButton.isPressed)
			{
				const float mouseLookSensitivity = 0.2f;

				Vector2 delta = mouse.delta.ReadValue();
				Vector3 euler = parentTransform.eulerAngles;

				float pitch = euler.x > 180f ? euler.x - 360f : euler.x;
				float yaw = euler.y;

				yaw += delta.x * mouseLookSensitivity;
				pitch -= delta.y * mouseLookSensitivity;
				pitch = Mathf.Clamp(pitch, -90f, 90f);

				parentTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
			}

			float speed = Settings.Movement.flySpeed;

			if (Shift) speed *= 2f;
			if (Alt) speed *= 0.5f;

			Vector3 movement = Vector3.zero;

			if (W) movement += parentTransform.forward;
			if (S) movement -= parentTransform.forward;
			if (A) movement -= parentTransform.right;
			if (D) movement += parentTransform.right;
			if (Space) movement += Vector3.up;
			if (Ctrl) movement -= Vector3.up;

			var body = tagger.rigidbody;

			if (body != null && movement.sqrMagnitude > 0f)
			{
				movement.Normalize();
				body.transform.position +=
					movement * (speed * Time.deltaTime);
			}

			var rig = VRRig.LocalRig;

			if (rig != null &&
				rig.head != null &&
				rig.head.rigTarget != null &&
				tagger.headCollider != null)
			{
				rig.head.rigTarget.transform.rotation =
					tagger.headCollider.transform.rotation;
			}
		}


		public static void Noclip()
		{
			CacheMeshColliders();

			bool vrNoclip =
				ControllerInputPoller.instance != null &&
				ControllerInputPoller.instance.rightControllerPrimaryButton;

			bool pcNoclip = Keyboard.current?.fKey.isPressed == true;
			bool noclipOn = vrNoclip || pcNoclip;

			if (noclipOn != noclipWasOn)
			{
				if (noclipOn)
				{
					originalColliderStates.Clear();

					foreach (MeshCollider collider in meshColliders)
					{
						if (collider != null)
							originalColliderStates[collider] = collider.enabled;
					}
				}
				else
				{
					foreach (KeyValuePair<MeshCollider, bool> entry
							 in originalColliderStates)
					{
						if (entry.Key != null)
							entry.Key.enabled = entry.Value;
					}

					originalColliderStates.Clear();
				}

				noclipWasOn = noclipOn;
			}

			if (!noclipOn)
				return;

			foreach (MeshCollider collider in meshColliders)
			{
				if (collider != null)
					collider.enabled = false;
			}
		}
	}
}