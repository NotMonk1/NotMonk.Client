using System;
using System.Net;
using UnityEngine;

namespace NotMonk_Client.Classes
{
	internal class GetLatest
	{
		private const string PastebinUrl =
			"https://pastebin.com/raw/YKTyZ5gp";

		public static string LiveVer = "";
		public static string LiveType = "";
		public static string Var3 = "";

		public static void Load()
		{
			try
			{
				using (WebClient client = new WebClient())
				{
					string content =
						client.DownloadString(PastebinUrl);

					string[] lines = content.Split(
						new[] { "\r\n", "\n", "\r" },
						StringSplitOptions.None
					);

					if (lines.Length >= 1)
						LiveVer = lines[0].Trim();

					if (lines.Length >= 2)
						LiveType = lines[1].Trim();

					if (lines.Length >= 3)
						Var3 = lines[2].Trim();

					Debug.Log(
						$"[GetLatest] LiveVer: {LiveVer}"
					);

					Debug.Log(
						$"[GetLatest] LiveType: {LiveType}"
					);

					Debug.Log(
						$"[GetLatest] Var3: {Var3}"
					);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(
					"[GetLatest] Failed to load: " + ex
				);
			}
		}
	}
}