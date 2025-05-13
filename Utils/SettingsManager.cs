using System;
using System.IO;
using System.Text.Json;
using Haraka.Models;
using Haraka.Services.KeystrokeManagers;

namespace Haraka.Utils
{
    public class SettingsManager
    {
        public UserPreferences UserPreferences { get; set; }
        public ToggleShortcut? TemporaryShortcut { get; set; }

        private readonly string _settingsPath;
        private readonly TypingDaemon _typingDaemon;

        public SettingsManager(TypingDaemon typingDaemon)
        {
            _settingsPath = GetSettingsPath();
            LoadUserPreferences();
            _typingDaemon = typingDaemon;
        }

        private void LoadUserPreferences()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    UserPreferences = JsonSerializer.Deserialize<UserPreferences>(json) ?? new UserPreferences();
                }
            }
            catch
            {
                UserPreferences = new UserPreferences(); // fallback
            }
        }
        private static string GetSettingsPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, HarakaConstants.APP_DATA_FOLDER_NAME);
            Directory.CreateDirectory(dir); // Ensure it exists
            return Path.Combine(dir, HarakaConstants.USER_PREFERENCES_FILE_NAME);
        }

        public void Save()
        {
            // Toggle Haraka daemon
            if (UserPreferences.IsHarakaEnabled)
            {
                _typingDaemon.Start();
            }
            else
            {
                _typingDaemon.Stop();
            }

            if (TemporaryShortcut != null)
            {
                UserPreferences.Shortcut = TemporaryShortcut;
            }

            // Store preferences
            try
            {
                var dir = Path.GetDirectoryName(_settingsPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(UserPreferences, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }
        public void EnableHaraka()
        {
            UserPreferences.IsHarakaEnabled = true;
            Save(); 
        }

        public void DisableHaraka()
        {
            UserPreferences.IsHarakaEnabled = false;
            Save();
        }
    }
}
