using System;
using System.IO;
using System.Text.Json;

public class UserPreferences
{
    private const string USER_PREFERENCES_FOLDER_NAME = "Haraka";
    private const string USER_PREFERENCES_FILE_NAME = "settings.json";

    public bool IsHarakaEnabled { get; set; } = false;
    public string Shortcut { get; set; } = "Ctrl+Alt+H";
    public bool LaunchOnStartup { get; set; } = true;
    public bool IsNotificationSoundEnabled { get; set; } = true;

    public static string GetSettingsPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, USER_PREFERENCES_FOLDER_NAME);
        Directory.CreateDirectory(dir); // Ensure it exists
        return Path.Combine(dir, USER_PREFERENCES_FILE_NAME);
    }

    public static UserPreferences Load()
    {
        var path = GetSettingsPath();
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<UserPreferences>(json) ?? new UserPreferences();
        }
        return new UserPreferences();
    }

    public void Save()
    {
        var path = GetSettingsPath();
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
