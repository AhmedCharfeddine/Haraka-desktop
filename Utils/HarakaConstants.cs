﻿namespace Haraka.Utils
{
    public static class HarakaConstants
    {
        public const string APP_DATA_FOLDER_NAME = "Haraka";
        public const string USER_PREFERENCES_FILE_NAME = "settings.json";
        public const string CONFIG_FILE_NAME = "config.json";

        public const string DEFAULT_STRATEGY = "mapping";

        public const string HARAKA_ICON_PATH = "Assets/haraka-logo.ico";
        public const string HARAKA_ICON_CIRCLE_PATH = "Assets/haraka-logo-circle.ico";
        public const string HARAKA_ICON_PURPLE_PATH = "Assets/haraka-logo-purple.svg";

        public const string HARAKA_BINARY_NAME_WINDOWS = "Haraka.exe";
        public const string HARAKA_BINARY_NAME_UNIX = "Haraka";
        public const string BIN_FOLDER = "bin";

        public static int DEFAULT_MINIMUM_WORD_LENGTH = 3;

        public static int DEFAULT_AUTO_CLOSE_POPUP_DELAY_MS = 4000;
        public static int DEFAULT_POPUP_DEBOUNCE_DELAY_MS = 300;

        public static string ENABLE_HARAKA_NOTIFICATION_SOUND_PATH = "Assets/Sounds/enable_sound.ogg";
        public static string DISABLE_HARAKA_NOTIFICATION_SOUND_PATH = "Assets/Sounds/disable_sound.ogg";

        public static string ENABLE_HARAKA_NOTIFICATION_SOUND = "toggleOn";
        public static string DISABLE_HARAKA_NOTIFICATION_SOUND = "toggleOff";

        public static float DEFAULT_NOTIFICATION_VOLUME = 0.1f;

        public static int DEFAULT_AUTO_CLOSE_TOAST_DELAY_MS = 2500;

        public static int DEFAULT_X_TOAST_PADDING = 75;
        public static int DEFAULT_Y_TOAST_PADDING = 25;

        public static int FADE_IN_OUT_ANIMATION_DURATION_MS = 250;
    }
}
