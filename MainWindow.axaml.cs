using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Haraka.ViewModels;
using Haraka.Views;

namespace Haraka
{
    public partial class MainWindow : Window
    {
        private TrayIcon _trayIcon;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTrayIcon();
        }
        private void InitializeTrayIcon()
        {
            _trayIcon = new TrayIcon
            {
                Icon = new WindowIcon("Assets/haraka-logo.ico"),
                ToolTipText = "Haraka",
                Menu = CreateTrayMenu()
            };

            _trayIcon.IsVisible = true;
        }

        private NativeMenu CreateTrayMenu()
        {
            var toggleOn = new NativeMenuItem("On");
            var toggleOff = new NativeMenuItem("Off");

            toggleOn.Click += (_, __) => ToggleOn();
            toggleOff.Click += (_, __) => ToggleOff();

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
            settingsItem.Click += (_, __) => OpenSettings();

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

        private void OpenSettings()
        {
            var settingsWindow = new SettingsWindow
            {
                DataContext = new SettingsViewModel()
            };
            settingsWindow.Show();
        }

        private void QuitApp()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                lifetime.Shutdown();
        }

        private void ToggleOn()
        {
            // TODO: store in preferences
            Console.WriteLine("Toggled ON");
        }

        private void ToggleOff()
        {
            // TODO: store in preferences
            Console.WriteLine("Toggled OFF");
        }
    }
}