using System;
using NotMonk_Client.Classes;
using UnityEngine;

namespace NotMonk_Client.Menu;

public enum GradientAnimationMode
{
    Scroll,
    Pulse,
}

public class Settings
{
    /*
     * These are the settings for the menu.
     *
     * To change the colors, you need to modify the ExtGradient variables.
     * Here are some examples on how to use ExtGradient:
     *
     * Solid Color:
     *  new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) }
     *
     * Simple Gradient:
     *  new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.white) }
     *
     * Rainbow Color:
     *   new ExtGradient { rainbow = true }
     *
     * Epileptic Color (random color every frame):
     *   new ExtGradient { epileptic = true }
     *
     * Self Color:
     *   new ExtGradient { copyRigColor = true }
     *
     * To change the font, you may use the following code:
     *   Font.CreateDynamicFontFromOSFont("Comic Sans MS", 24)
     */

    [SavedSetting] public static bool dropMenu    = true;
    [SavedSetting] public static bool animateMenu = true;

    [SavedSetting] public static float menuAnimationSpeed = 14f;

    [SavedSetting] public static GradientAnimationMode gradientAnimationMode =
            GradientAnimationMode.Scroll;

    public static ExtGradient backgroundColor;

    public static readonly ExtGradient[] buttonColors =
    {
            new(),
            new(),
    };

    public static ExtGradient outlineColor;

    public static readonly Color[] textColors =
    {
            Color.white,
            Color.white,
    };

    public static readonly MenuTheme[] themes =
    {
            new()
            {
                    name = "Purple",

                    backgroundPrimary   = new Color32(23, 16, 46, 255),
                    //backgroundSecondary = new Color32(38, 25, 73, 255),

                    disabledButtonPrimary   = new Color32(17, 13, 32, 255),
                    //disabledButtonSecondary = new Color32(31, 23, 54, 255),

                    enabledButtonPrimary   = new Color32(95,  75,  216, 255),
                    //enabledButtonSecondary = new Color32(139, 109, 255, 255),

                    outline = new Color32(9, 7, 18, 255),
            },

            new()
            {
                    name = "Blue",

                    backgroundPrimary   = new Color32(10, 24, 44, 255),
                    //backgroundSecondary = new Color32(17, 47, 78, 255),

                    disabledButtonPrimary   = new Color32(10, 19, 31, 255),
                    //disabledButtonSecondary = new Color32(16, 34, 55, 255),

                    enabledButtonPrimary   = new Color32(32, 105, 196, 255),
                    //enabledButtonSecondary = new Color32(74, 158, 255, 255),

                    outline = new Color32(5, 12, 22, 255),
            },

            new()
            {
                    name = "Red",

                    backgroundPrimary   = new Color32(42, 12, 17, 255),
                    //backgroundSecondary = new Color32(73, 19, 28, 255),

                    disabledButtonPrimary   = new Color32(28, 12, 15, 255),
                    //disabledButtonSecondary = new Color32(49, 19, 24, 255),

                    enabledButtonPrimary   = new Color32(184, 46, 65,  255),
                    //enabledButtonSecondary = new Color32(245, 85, 105, 255),

                    outline = new Color32(20, 5, 8, 255),
            },

            new()
            {
                    name = "Mint",

                    backgroundPrimary   = new Color32(9,  36, 33, 255),
                    //backgroundSecondary = new Color32(14, 61, 54, 255),

                    disabledButtonPrimary   = new Color32(8,  27, 25, 255),
                    //disabledButtonSecondary = new Color32(13, 45, 41, 255),

                    enabledButtonPrimary   = new Color32(31, 171, 140, 255),
                    //enabledButtonSecondary = new Color32(83, 226, 192, 255),

                    outline = new Color32(4, 18, 16, 255),
            },

            new()
            {
                    name = "Monochrome",

                    backgroundPrimary   = new Color32(18, 18, 18, 255),
                    //backgroundSecondary = new Color32(35, 35, 35, 255),

                    disabledButtonPrimary   = new Color32(24, 24, 24, 255),
                    //disabledButtonSecondary = new Color32(45, 45, 45, 255),

                    enabledButtonPrimary   = new Color32(100, 100, 100, 255),
                    //enabledButtonSecondary = new Color32(160, 160, 160, 255),

                    outline = new Color32(5, 5, 5, 255),
            },

			new()
            {
		            name = "Orange",

		            backgroundPrimary   = new Color32(43, 22, 8, 255),
                    //backgroundSecondary = new Color32(74, 38, 14, 255),

                    disabledButtonPrimary   = new Color32(29, 16, 7, 255),
                    //disabledButtonSecondary = new Color32(50, 27, 11, 255),

                    enabledButtonPrimary   = new Color32(214, 106, 33, 255),
                    //enabledButtonSecondary = new Color32(255, 149, 74, 255),

                    outline = new Color32(20, 10, 4, 255),
            },

            new()
            {
		            name = "Gold",

		            backgroundPrimary   = new Color32(41, 33, 8, 255),
                    //backgroundSecondary = new Color32(70, 57, 13, 255),

                    disabledButtonPrimary   = new Color32(28, 23, 7, 255),
                    //disabledButtonSecondary = new Color32(48, 39, 11, 255),

                    enabledButtonPrimary   = new Color32(196, 156, 32, 255),
                    //enabledButtonSecondary = new Color32(255, 209, 74, 255),

                    outline = new Color32(19, 15, 4, 255),
            },

            new()
            {
		            name = "Teal",

		            backgroundPrimary   = new Color32(8, 33, 39, 255),
                    //backgroundSecondary = new Color32(14, 57, 66, 255),

                    disabledButtonPrimary   = new Color32(7, 23, 27, 255),
                    //disabledButtonSecondary = new Color32(11, 40, 46, 255),

                    enabledButtonPrimary   = new Color32(32, 158, 184, 255),
                    //enabledButtonSecondary = new Color32(74, 213, 245, 255),

                    outline = new Color32(4, 17, 20, 255),
            },

            new()
            {
		            name = "Magenta",

		            backgroundPrimary   = new Color32(41, 10, 38, 255),
                    //backgroundSecondary = new Color32(70, 17, 65, 255),

                    disabledButtonPrimary   = new Color32(28, 8, 26, 255),
                    //disabledButtonSecondary = new Color32(48, 13, 45, 255),

                    enabledButtonPrimary   = new Color32(196, 42, 178, 255),
                    //enabledButtonSecondary = new Color32(255, 82, 230, 255),

                    outline = new Color32(19, 4, 18, 255),
            },

            new()
            {
		            name = "Indigo",

		            backgroundPrimary   = new Color32(17, 14, 46, 255),
                    //backgroundSecondary = new Color32(29, 23, 78, 255),

                    disabledButtonPrimary   = new Color32(12, 10, 32, 255),
                    //disabledButtonSecondary = new Color32(21, 17, 54, 255),

                    enabledButtonPrimary   = new Color32(75, 65, 216, 255),
                    //enabledButtonSecondary = new Color32(115, 102, 255, 255),

                    outline = new Color32(7, 6, 18, 255),
            },

            new()
            {
		            name = "Lime",

		            backgroundPrimary   = new Color32(24, 39, 8, 255),
                    //backgroundSecondary = new Color32(41, 67, 13, 255),

                    disabledButtonPrimary   = new Color32(17, 27, 7, 255),
                    //disabledButtonSecondary = new Color32(29, 47, 11, 255),

                    enabledButtonPrimary   = new Color32(120, 184, 32, 255),
                    //enabledButtonSecondary = new Color32(168, 245, 74, 255),

                    outline = new Color32(11, 19, 4, 255),
            },

            new()
            {
		            name = "Rose",

		            backgroundPrimary   = new Color32(43, 12, 24, 255),
                    //backgroundSecondary = new Color32(74, 19, 40, 255),

                    disabledButtonPrimary   = new Color32(29, 12, 20, 255),
                    //disabledButtonSecondary = new Color32(50, 19, 34, 255),

                    enabledButtonPrimary   = new Color32(214, 46, 105, 255),
                    //enabledButtonSecondary = new Color32(255, 91, 146, 255),

                    outline = new Color32(20, 5, 12, 255),
            },

            new()
            {
		            name = "Slate",

		            backgroundPrimary   = new Color32(19, 24, 30, 255),
                    //backgroundSecondary = new Color32(33, 42, 52, 255),

                    disabledButtonPrimary   = new Color32(15, 19, 24, 255),
                    //disabledButtonSecondary = new Color32(26, 33, 41, 255),

                    enabledButtonPrimary   = new Color32(85, 110, 140, 255),
                    //enabledButtonSecondary = new Color32(130, 160, 196, 255),

                    outline = new Color32(8, 11, 14, 255),
            },

            new()
            {
		            name = "Brown",

		            backgroundPrimary   = new Color32(35, 24, 16, 255),
                    //backgroundSecondary = new Color32(60, 41, 27, 255),

                    disabledButtonPrimary   = new Color32(24, 16, 11, 255),
                    //disabledButtonSecondary = new Color32(41, 28, 19, 255),

                    enabledButtonPrimary   = new Color32(155, 105, 65, 255),
                    //enabledButtonSecondary = new Color32(206, 148, 100, 255),

                    outline = new Color32(16, 10, 6, 255),
            },

			new()
            {
		            name = "Cyan",

		            backgroundPrimary   = new Color32(8, 38, 43, 255),
                    //backgroundSecondary = new Color32(14, 64, 73, 255),

                    disabledButtonPrimary   = new Color32(7, 26, 29, 255),
                    //disabledButtonSecondary = new Color32(12, 44, 49, 255),

                    enabledButtonPrimary   = new Color32(32, 175, 196, 255),
                    //enabledButtonSecondary = new Color32(74, 230, 255, 255),

                    outline = new Color32(4, 19, 21, 255),
            },

            new()
            {
		            name = "Yellow",

		            backgroundPrimary   = new Color32(43, 40, 8, 255),
                    //backgroundSecondary = new Color32(74, 68, 14, 255),

                    disabledButtonPrimary   = new Color32(29, 27, 7, 255),
                    //disabledButtonSecondary = new Color32(50, 46, 12, 255),

                    enabledButtonPrimary   = new Color32(214, 196, 33, 255),
                    //enabledButtonSecondary = new Color32(255, 240, 74, 255),

                    outline = new Color32(20, 18, 4, 255),
            },

            new()
            {
		            name = "Violet",

		            backgroundPrimary   = new Color32(32, 12, 46, 255),
                    //backgroundSecondary = new Color32(55, 19, 78, 255),

                    disabledButtonPrimary   = new Color32(22, 9, 32, 255),
                    //disabledButtonSecondary = new Color32(38, 15, 54, 255),

                    enabledButtonPrimary   = new Color32(158, 46, 216, 255),
                    //enabledButtonSecondary = new Color32(197, 91, 255, 255),

                    outline = new Color32(14, 5, 20, 255),
            },

            new()
            {
		            name = "Maroon",

		            backgroundPrimary   = new Color32(35, 10, 15, 255),
                    //backgroundSecondary = new Color32(60, 17, 24, 255),

                    disabledButtonPrimary   = new Color32(24, 8, 11, 255),
                    //disabledButtonSecondary = new Color32(41, 14, 19, 255),

                    enabledButtonPrimary   = new Color32(140, 32, 44, 255),
                    //enabledButtonSecondary = new Color32(190, 60, 74, 255),

                    outline = new Color32(16, 4, 7, 255),
            },

            new()
            {
		            name = "Forest",

		            backgroundPrimary   = new Color32(13, 33, 12, 255),
                    //backgroundSecondary = new Color32(22, 57, 20, 255),

                    disabledButtonPrimary   = new Color32(10, 23, 9, 255),
                    //disabledButtonSecondary = new Color32(17, 39, 15, 255),

                    enabledButtonPrimary   = new Color32(56, 158, 48, 255),
                    //enabledButtonSecondary = new Color32(101, 213, 90, 255),

                    outline = new Color32(6, 17, 5, 255),
            },

            new()
            {
		            name = "Navy",

		            backgroundPrimary   = new Color32(8, 14, 35, 255),
                    //backgroundSecondary = new Color32(14, 24, 60, 255),

                    disabledButtonPrimary   = new Color32(7, 10, 24, 255),
                    //disabledButtonSecondary = new Color32(12, 18, 41, 255),

                    enabledButtonPrimary   = new Color32(38, 68, 184, 255),
                    //enabledButtonSecondary = new Color32(80, 112, 245, 255),

                    outline = new Color32(4, 7, 16, 255),
            },

            new()
            {
		            name = "Coral",

		            backgroundPrimary   = new Color32(43, 20, 14, 255),
                    //backgroundSecondary = new Color32(74, 34, 24, 255),

                    disabledButtonPrimary   = new Color32(29, 14, 10, 255),
                    //disabledButtonSecondary = new Color32(50, 24, 17, 255),

                    enabledButtonPrimary   = new Color32(214, 90, 66, 255),
                    //enabledButtonSecondary = new Color32(255, 130, 104, 255),

                    outline = new Color32(20, 9, 6, 255),
            },

            new()
            {
		            name = "Steel",

		            backgroundPrimary   = new Color32(15, 21, 27, 255),
                    //backgroundSecondary = new Color32(26, 37, 47, 255),

                    disabledButtonPrimary   = new Color32(12, 16, 21, 255),
                    //disabledButtonSecondary = new Color32(21, 29, 37, 255),

                    enabledButtonPrimary   = new Color32(64, 130, 158, 255),
                    //enabledButtonSecondary = new Color32(105, 178, 206, 255),

                    outline = new Color32(6, 9, 12, 255),
            },
	};

    [SavedSetting(OnLoaded = nameof(ApplyTheme))]
    public static int themeIndex;

    [SavedSetting(OnLoaded = nameof(ApplyTheme))]
    public static bool rainbowColors = false;
    [SavedSetting] public static bool outlines;

    [SavedSetting] public static float menuCornerRadius = 0.06f;
    [SavedSetting] public static float outlineThickness = 0.018f;

	public static Font currentFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    //public static Font currentFont = Font.CreateDynamicFontFromOSFont("Arial", 24);

	[SavedSetting] public static bool fpsCounter       = true;
    [SavedSetting] public static bool disconnectButton = true;
    [SavedSetting] public static bool rightHanded;
    [SavedSetting] public static bool disableNotifications;
	[SavedSetting] public static bool disableMenuSound;

    [SavedSetting] public static bool RPChideRoomCode = false;

	[SavedSetting] public static KeyCode keyboardButton = KeyCode.X;

    [SavedSetting] public static Vector3 menuSize       = new(0.1f, 1f, 1f); // Depth, width, height
    [SavedSetting] public static int     buttonsPerPage = 8;

    [SavedSetting] public static bool incrementalButtons      = true;
    [SavedSetting] public static bool buttonGradients         = true;
    [SavedSetting] public static bool roundedButtons          = true;
    [SavedSetting] public static bool verticalButtonGradients = false;

    [SavedSetting] public static bool pageButtonsAtTop;

    [SavedSetting] public static bool disableCustomBoards;

    [SavedSetting] public static float searchKeyboardFollowSpeed = 5f;

    [SavedSetting] public static Vector3 buttonSize = new(0.09f, 0.9f, 0.08f);

    [SavedSetting] public static float buttonSpacing      = 0.1f;
    [SavedSetting] public static float buttonCornerRadius = 0.018f;

    [SavedSetting] public static int buttonCornerSegments = 5;

    [SavedSetting] public static float gradientSpeed = 0.5f; // Speed of colors

    [SavedSetting] public static GunSettings gunSettings = new();

    static Settings() =>
            ApplyTheme();

    public static string GradientAnimationName =>
            gradientAnimationMode.ToString();

    public static string ThemeName => themes[themeIndex].name;

    public static void ChangeGradientAnimation(bool increment)
    {
        int count = Enum.GetValues(typeof(GradientAnimationMode)).Length;

        int index =
                (int)gradientAnimationMode +
                (increment ? 1 : -1);

        if (index >= count)
            index = 0;
        else if (index < 0)
            index = count - 1;

        gradientAnimationMode =
                (GradientAnimationMode)index;

        ColorChanger.ClearGradientCache();
    }

	public static void ChangeTheme(bool increment)
    {
        themeIndex += increment ? 1 : -1;

        if (themeIndex >= themes.Length)
            themeIndex = 0;
        else if (themeIndex < 0)
            themeIndex = themes.Length - 1;

        ApplyTheme();
    }
	public static float oldgradientSpeed = gradientSpeed;

	public static void SetRainbowColors(bool enabled)
	{
		if (enabled)
		{
			oldgradientSpeed = gradientSpeed;
			gradientSpeed = 0.5f;
		}
		else
		{
			gradientSpeed = oldgradientSpeed;
		}

		rainbowColors = enabled;

        ApplyTheme();
    }

	public static void ApplyTheme()
    {
        if (themes.Length == 0)
            return;

        themeIndex = Mathf.Clamp(
                themeIndex,
                0,
                themes.Length - 1);

        MenuTheme theme = themes[themeIndex];

        backgroundColor = rainbowColors
                                  ? new ExtGradient
                                  {
                                          rainbow = true,
                                  }
                                  : CreateGradient(
                                          theme.backgroundPrimary,
                                          theme.backgroundSecondary);

        buttonColors[0] = CreateGradient(
                theme.disabledButtonPrimary,
                theme.disabledButtonSecondary);

        buttonColors[1] = rainbowColors
                                  ? new ExtGradient
                                  {
                                          rainbow = true,
                                  }
                                  : CreateGradient(
                                          theme.enabledButtonPrimary,
                                          theme.enabledButtonSecondary);

        outlineColor = new ExtGradient
        {
                colors = ExtGradient.GetSolidGradient(theme.outline),
        };

        textColors[0] = theme.disabledText;
        textColors[1] = theme.enabledText;

        ColorChanger.ClearGradientCache();
    }

    private static ExtGradient CreateGradient(Color first, Color second) =>
            new()
            {
                    colors = ExtGradient.GetSimpleGradient(first, second),
            };
}