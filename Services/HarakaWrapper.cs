using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Haraka.Utils;

namespace Haraka.Services
{
    public class HarakaWrapper
    {
        private string? _harakaBinaryPath { get; set; } = GetHarakaBinaryPath();
        private const string _harakaCliOptions = "-p";

        private static string GetHarakaBinaryPath()
        {
            string binaryName = OperatingSystem.IsWindows() ? 
                HarakaConstants.HARAKA_BINARY_NAME_WINDOWS: HarakaConstants.HARAKA_BINARY_NAME_UNIX;
            string basePath = Path.Combine(AppContext.BaseDirectory, HarakaConstants.BIN_FOLDER);
            return Path.Combine(basePath, binaryName);
        }

        public async Task<string> RunTransliterationAsync(string word)
        {
            var psi = new ProcessStartInfo
            {
                FileName = _harakaBinaryPath,
                Arguments = $"{ConfigManager.Config.Strategy} {_harakaCliOptions} \"{word}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (_, args) =>
            {
                if (args.Data != null)
                    outputBuilder.AppendLine(args.Data);
            };

            process.ErrorDataReceived += (_, args) =>
            {
                if (args.Data != null)
                    errorBuilder.AppendLine(args.Data);
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    var err = errorBuilder.ToString().Trim();
                    throw new Exception($"Haraka failed with exit code {process.ExitCode}: {err}");
                }

                return outputBuilder.ToString().Trim();
            }
            catch (Exception ex)
            {
                // Log error if needed
                return $"[Error] {ex.Message}";
            }

        }
    }
}
