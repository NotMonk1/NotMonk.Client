using System;
using System.Collections.Generic;
using System.Text;

namespace NotMonk_Client.Other
{
	public static class RigUtils
	{
		public static VRRig GetVRRigFromPlayer(NetPlayer p) =>
			GorillaGameManager.StaticFindRigForPlayer(p);
	}
}
