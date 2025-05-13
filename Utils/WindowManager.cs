using Avalonia.Controls;
using Haraka.Services;
using Haraka.Views;

namespace Haraka.Utils
{
    public static class WindowManager
    {
        private static Window? _settingsWindow;

        public static void OpenSettingsWindow(AppServices services)
        {
            if (_settingsWindow == null || !_settingsWindow.IsVisible)
            {
                _settingsWindow = new SettingsWindow(services);

                _settingsWindow.Closed += (_, _) =>
                {
                    _settingsWindow = null;
                };

                _settingsWindow.Show();
            }
            else
            {
                _settingsWindow.Activate();
            }
        }
    }
}
