using System;
using System.IO;
using System.Text.Json;

namespace Haraka.Utils
{
    public static class SettingsManager
    {
        public static UserPreferences UserPreferences { get; set; }

        private static readonly string SettingsPath = GetSettingsPath();

        public static string GetSettingsPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, HarakaConstants.APP_DATA_FOLDER_NAME);
            Directory.CreateDirectory(dir); // Ensure it exists
            return Path.Combine(dir, HarakaConstants.USER_PREFERENCES_FILE_NAME);
        }

        static SettingsManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    UserPreferences = JsonSerializer.Deserialize<UserPreferences>(json) ?? new UserPreferences();
                }
            }
            catch
            {
                UserPreferences = new UserPreferences(); // fallback
            }
        }

        public static void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(UserPreferences, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }
    }
}
