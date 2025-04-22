using System;
using System.Diagnostics;
using System.IO;
using Haraka.Utils;

namespace Haraka.Services
{
    internal class HarakaWrapper
    {
        private static string GetHarakaBinaryPath()
        {
            string binaryName = OperatingSystem.IsWindows() ? 
                HarakaConstants.HARAKA_BINARY_NAME_WINDOWS: HarakaConstants.HARAKA_BINARY_NAME_UNIX;
            string basePath = Path.Combine(AppContext.BaseDirectory, HarakaConstants.BIN_FOLDER);
            return Path.Combine(basePath, binaryName);
        }

        public string RunTransliteration(string word)
        {
            var psi = new ProcessStartInfo
            {
                FileName = GetHarakaBinaryPath(),
                Arguments = $"{ConfigManager.Config.Strategy} {word}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}
