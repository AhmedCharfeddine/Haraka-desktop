using Avalonia.Controls;
using Haraka.Views;

namespace Haraka.Utils
{
    public static class WindowManager
    {
        private static Window? _settingsWindow;

        public static void OpenSettingsWindow()
        {
            if (_settingsWindow == null || !_settingsWindow.IsVisible)
            {
                _settingsWindow = new SettingsWindow();

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
