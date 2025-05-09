using System;
using System.IO;
using System.Text.Json;
using Haraka.Models;


namespace Haraka.Utils
{
    public static class ConfigManager
    {
        private static readonly string ConfigPath = GetConfigPath();

        public static AppConfig Config { get; set; }

        private static string GetConfigPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, HarakaConstants.APP_DATA_FOLDER_NAME);
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, HarakaConstants.CONFIG_FILE_NAME);
        }

        static ConfigManager()
        {
            Load();
        }

        private static void Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    Config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                else
                {
                    var dir = Path.GetDirectoryName(ConfigPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var newConfig = new AppConfig();
                    var json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(ConfigPath, json);
                    Config = newConfig;
                }
            }
            catch 
            {
                Config = new AppConfig();
            }
        }
    }
}
