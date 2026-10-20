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
