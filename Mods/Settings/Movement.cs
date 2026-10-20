using NotMonk_Client.Mods;
using NotMonk_Client.Classes;
using UnityEngine;

namespace NotMonk_Client.Mods.Settings;

public class Movement
{
    private static readonly string[] flySpeedNames =
    [
            "Very Slow",
            "Slow",
            "Normal",
            "Fast",
            "Very Fast",
            "Extreme",
    ];

    private static readonly float[] flySpeedValues =
    [
            5f,
            10f,
            15f,
            20f,
            30f,
            50f,
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedFlySpeed))]
    public static int flySpeedIndex = 2;

    public static float flySpeed = 15f;

    public static string FlySpeedName =>
            flySpeedNames[flySpeedIndex];

    public static void ChangeFlySpeed(
            bool increment)
    {
        flySpeedIndex +=
                increment
                        ? 1
                        : -1;

        if (flySpeedIndex >= flySpeedNames.Length)
            flySpeedIndex = 0;
        else if (flySpeedIndex < 0)
            flySpeedIndex = flySpeedNames.Length - 1;

        ApplySavedFlySpeed();
    }

    public static void SetFlySpeedIndex(
            int index)
    {
        flySpeedIndex =
                Mathf.Clamp(
                        index,
                        0,
                        flySpeedValues.Length - 1);

        ApplySavedFlySpeed();
    }

    private static void ApplySavedFlySpeed()
    {
        flySpeedIndex =
                Mathf.Clamp(
                        flySpeedIndex,
                        0,
                        flySpeedValues.Length - 1);

        flySpeed =
                flySpeedValues[
                        flySpeedIndex];
    }

	private static readonly string[] speedBoostNames =
    [
	    "Very Slow",
    	"Slow",
	    "Fast",
    	"Faster",
	    "Very Fast",
    	"Extreme",
    ];

	private static readonly float[] speedBoostValues =
	[
		5f,
    	10f,
    	20f,
	    30f,
	    50f,
	    70f,
    ];

	[SavedSetting(OnLoaded = nameof(ApplySavedSpeedBoost))]
	public static int speedBoostIndex = 2;

	public static float speedBoost = 15f;

	public static string SpeedBoostName =>
		speedBoostNames[speedBoostIndex];

	public static void ChangeSpeedBoost(
		bool increment)
	{
		speedBoostIndex +=
			increment
				? 1
				: -1;

		if (speedBoostIndex >= speedBoostNames.Length)
			speedBoostIndex = 0;
		else if (speedBoostIndex < 0)
			speedBoostIndex = speedBoostNames.Length - 1;

		ApplySavedSpeedBoost();
	}

	public static void SetSpeedBoostIndex(
		int index)
	{
		speedBoostIndex =
			Mathf.Clamp(
				index,
				0,
				speedBoostValues.Length - 1);

		ApplySavedSpeedBoost();
	}

	private static void ApplySavedSpeedBoost()
	{
		speedBoostIndex =
			Mathf.Clamp(
				speedBoostIndex,
				0,
				speedBoostValues.Length - 1);

		speedBoost =
			speedBoostValues[
				speedBoostIndex];
	}

	private static readonly string[] nameTagColorNames =
		[
			"White",
			"Red",
			"Orange",
			"Yellow",
			"Green",
			"Cyan",
			"Blue",
			"Purple",
			"Pink",
			"Rig Color",
		];

	private static readonly Color[] nameTagColorValues =
	[
		Color.white,
		new Color(1f, 0f, 0f),
		new Color(1f, 0.5f, 0f),
		new Color(1f, 1f, 0f),
		new Color(0f, 1f, 0f),
		new Color(0f, 1f, 1f),
		new Color(0f, 0.25f, 1f),
		new Color(0.6f, 0.2f, 1f),
		new Color(1f, 0.2f, 0.6f),
		RigManager.GetPlayerColor(VRRig.LocalRig),
	];

	[SavedSetting(OnLoaded = nameof(ApplySavedNameTagColor))]
	public static int nameTagColorIndex = 0;

	public static Color nameTagColor = Color.white;

	public static string NameTagColorName =>
		nameTagColorNames[nameTagColorIndex];

	public static void ChangeNameTagColor(bool increment)
	{
		nameTagColorIndex += increment ? 1 : -1;

		if (nameTagColorIndex >= nameTagColorNames.Length)
			nameTagColorIndex = 0;
		else if (nameTagColorIndex < 0)
			nameTagColorIndex = nameTagColorNames.Length - 1;

		ApplySavedNameTagColor();
	}

	public static void ApplySavedNameTagColor()
	{
		nameTagColorIndex = Mathf.Clamp(
			nameTagColorIndex,
			0,
			nameTagColorValues.Length - 1
		);

		nameTagColor = nameTagColorValues[nameTagColorIndex];

		

		Important.SetColor(nameTagColor);
	}
}