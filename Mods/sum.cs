/*
 * NotMonk Client - sum.cs
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

using NotMonk_Client.Menu;

namespace NotMonk_Client.Mods.Settings
{
	public class sumfin
	{
		public static bool isWaterMarkOn;

		public static void iiwatermark()
		{
			Main.WatermarkName = "iiwatermark.png";
			Main.CreateWatermark();
		}
		public static void NMClientWaterMark()
		{
			Main.WatermarkName = "watermark.png";
			Main.CreateWatermark();
		}
		public static void EHHHHH()
		{
			if (Main.WatermarkName == "iiwatermark.png")
				isWaterMarkOn = true;
			else
				isWaterMarkOn = false;
		}
	}
}
