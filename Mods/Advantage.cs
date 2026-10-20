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
