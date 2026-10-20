using UnityEngine;

namespace NotMonk_Client.Mods
{
	public abstract class Visual
	{
		public static void BoxESP()
		{
			for (int i = 0; i < VRRigCache.ActiveRigs.Count; i++)
			{
				VRRig Player = VRRigCache.ActiveRigs[i];
				if (Player != GorillaTagger.Instance.offlineVRRig)
				{
					if (!Player.mainSkin.material.name.Contains("fected"))
					{
						Color PlayerColorUntagged = Player.playerColor;
						GameObject UntaggedESP = GameObject.CreatePrimitive(PrimitiveType.Cube);
						UntaggedESP.transform.position = Player.transform.position;
						UnityEngine.Object.Destroy(UntaggedESP.GetComponent<Collider>());
						UntaggedESP.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
						UntaggedESP.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
						UntaggedESP.GetComponent<Renderer>().material.color = PlayerColorUntagged;
						UnityEngine.Object.Destroy(UntaggedESP, Time.deltaTime);
					}
					if (Player.mainSkin.material.name.Contains("fected"))
					{
						GameObject TaggedESP = GameObject.CreatePrimitive(PrimitiveType.Cube);
						TaggedESP.transform.position = Player.transform.position;
						UnityEngine.Object.Destroy(TaggedESP.GetComponent<Collider>());
						TaggedESP.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
						TaggedESP.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
						TaggedESP.GetComponent<Renderer>().material.color = Color.orange;
						UnityEngine.Object.Destroy(TaggedESP, Time.deltaTime);
					}
				}

			}
		}

		public class Fullbright
		{
			public static bool Enabled { get; private set; }

			private static Color originalAmbientLight;
			private static float originalAmbientIntensity;
			private static bool originalFog;
			private static bool savedSettings;

			public static void Enable()
			{
				if (!savedSettings)
				{
					originalAmbientLight = RenderSettings.ambientLight;
					originalAmbientIntensity = RenderSettings.ambientIntensity;
					originalFog = RenderSettings.fog;

					savedSettings = true;
				}

				Enabled = true;
				Apply();
			}

			public static void Disable()
			{
				Enabled = false;

				if (!savedSettings)
					return;

				RenderSettings.ambientLight = originalAmbientLight;
				RenderSettings.ambientIntensity = originalAmbientIntensity;
				RenderSettings.fog = originalFog;

				savedSettings = false;
			}

			public static void Toggle()
			{
				if (Enabled)
					Disable();
				else
					Enable();
			}

			public static void Apply()
			{
				if (!Enabled)
					return;

				RenderSettings.ambientMode =
					UnityEngine.Rendering.AmbientMode.Flat;

				RenderSettings.ambientLight = Color.white;
				RenderSettings.ambientIntensity = 2f;
				RenderSettings.fog = false;
			}

		}
	}
}
