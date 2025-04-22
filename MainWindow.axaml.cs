using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Haraka.Utils;

namespace Haraka
{
    public partial class MainWindow : Window
    {
        private TrayIcon _trayIcon;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTrayIcon();
            InitializeAppIcon();
        }

        private void InitializeAppIcon()
        {
            this.Icon = new WindowIcon(HarakaConstants.HARAKA_ICON_CIRCLE_PATH);
        }

        private void InitializeTrayIcon()
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon(HarakaConstants.HARAKA_ICON_CIRCLE_PATH),
                ToolTipText = "Haraka",
                Menu = CreateTrayMenu()
            };

            _trayIcon.Clicked += (_, __) => WindowManager.OpenSettingsWindow();

            _trayIcon.IsVisible = true;
        }

        private NativeMenu CreateTrayMenu()
        {
            var toggleOn = new NativeMenuItem("On");
            var toggleOff = new NativeMenuItem("Off");

            toggleOn.Click += (_, __) => ToggleOn(toggleOff);
            toggleOff.Click += (_, __) => ToggleOff(toggleOn);

            var toggleSubmenu = new NativeMenu()
            {
                toggleOn,
                toggleOff
            };

            var toggleMenu = new NativeMenuItem("Toggle")
            {
                Menu = toggleSubmenu
            };

            var settingsItem = new NativeMenuItem("Settings");
            settingsItem.Click += (_, __) => WindowManager.OpenSettingsWindow();

            var quitItem = new NativeMenuItem("Quit");
            quitItem.Click += (_, __) => QuitApp();

            return new NativeMenu
            {
                toggleMenu,
                settingsItem,
                new NativeMenuItemSeparator(),
                quitItem
            };
        }

        private void QuitApp()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                lifetime.Shutdown();
        }

        private void ToggleOn(NativeMenuItem toggleOff)
        {
            SettingsManager.UserPreferences.IsHarakaEnabled = true;
            Console.WriteLine("Toggled ON");
        }

        private void ToggleOff(NativeMenuItem toggleOn)
        {
            SettingsManager.UserPreferences.IsHarakaEnabled = false;
            Console.WriteLine("Toggled OFF");
        }
    }
}