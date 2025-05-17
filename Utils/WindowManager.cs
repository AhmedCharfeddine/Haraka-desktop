using System;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Haraka.Services;
using Haraka.Views;

namespace Haraka.Utils
{
    public static class WindowManager
    {
        private static Window? _settingsWindow;
        private static ToastNotificationWindow _toastNotificationWindow;
        private static bool? _toastState;
        private static readonly Timer _autoCloseToastTimer = new(ConfigManager.Config.AutoCloseToastDelayMs);
        private static readonly TimeSpan fadeTimeSpan = TimeSpan.FromMilliseconds(ConfigManager.Config.FadeInOutAnimationDurationMs);

        static WindowManager()
        {
            _autoCloseToastTimer.Elapsed += OnAutoCloseToastTimerElapsed;
        }

        private static void OnAutoCloseToastTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _autoCloseToastTimer.Stop();
            Dispatcher.UIThread.Post(async () =>
            {
                _toastState = null;
                await _toastNotificationWindow.FadeOutAsync(fadeTimeSpan);
                _toastNotificationWindow.Hide();
            });
        }

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

        public static void OpenToastNotificationWindow(bool isEnabled)
        {
            if ((_toastNotificationWindow == null && _toastState == null) ||
                (_toastState != isEnabled))
            {
                Dispatcher.UIThread.Post(async () =>
                {
                    if (_toastNotificationWindow?.IsVisible == true)
                    {
                        _toastNotificationWindow?.Hide();
                    }
                    _toastState = isEnabled;
                    _toastNotificationWindow = new ToastNotificationWindow(isEnabled);
                    _toastNotificationWindow.Opened += AdjustToastPosition;
                    await _toastNotificationWindow.FadeInAsync(fadeTimeSpan);
                });
                _autoCloseToastTimer.Stop();
                _autoCloseToastTimer.Start();
            }
        }

        private static void AdjustToastPosition(object? sender, EventArgs e)
        {
            ToastNotificationWindow toast = (ToastNotificationWindow)sender;
            var screens = toast?.Screens;
            var primary = screens?.Primary;

            var paddingX = ConfigManager.Config.ToastXPadding;
            var paddingY = ConfigManager.Config.ToastYPadding;
            var x = primary?.WorkingArea.Right - toast?.Width - paddingX;
            var y = primary?.WorkingArea.Bottom - toast?.Height - paddingY;

            toast.Position = new PixelPoint((int)x, (int)y);
        }
    }
}
