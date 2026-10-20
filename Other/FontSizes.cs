using StupidTemplate.Classes;
using UnityEngine;

namespace NotMonk_Client.Other
{
	public class FontSizes
	{
		public static int FontSize = 24;

		private static readonly string[] fontNames =
		[
			"XL Small",
			"smaller",
			"Small",
			"Medium",
			"Default",
			"Biggah",
			"Extreme",
		];

		private static readonly int[] fontPercentValues =
		[
			5,
			10,
			15,
			20,
			24,
			30,
			50,
		];

		public static void ChangeFontSize(
			bool increment)
		{
			FontSize +=
					increment
							? 1
							: -1;

			if (FontSize >= fontNames.Length)
				FontSize = 0;
			else if (FontSize < 0)
				FontSize = fontNames.Length - 1;
		}
	}
}
