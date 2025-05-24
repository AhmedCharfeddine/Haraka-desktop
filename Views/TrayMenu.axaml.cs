using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Haraka.Services;
using Haraka.Utils;

namespace Haraka;

public partial class TrayMenu : Window
{
    private readonly AppServices _appServices;
    private TrayIcon _trayIcon;

    public TrayMenu(AppServices appServices)
    {
        _appServices = appServices;
        InitializeComponent();
        InitializeTrayIcon();
    }

    private void InitializeTrayIcon()
    {
        _trayIcon = new TrayIcon
        {
            Icon = new WindowIcon(HarakaConstants.HARAKA_ICON_CIRCLE_PATH),
            ToolTipText = "Haraka",
            Menu = CreateTrayMenu(),
        };

        _trayIcon.Clicked += (_, __) => WindowManager.OpenSettingsWindow(_appServices);
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
        settingsItem.Click += (_, __) => WindowManager.OpenSettingsWindow(_appServices);

        var aboutItem = new NativeMenuItem("About");
        aboutItem.Click += (_, __) => WindowManager.OpenMainWindow(_appServices);

        var quitItem = new NativeMenuItem("Quit");
        quitItem.Click += (_, __) => QuitApp();

        return new NativeMenu
            {
                toggleMenu,
                settingsItem,
                aboutItem,
                new NativeMenuItemSeparator(),
                quitItem
            };
    }

    private static void QuitApp()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void ToggleOn()
    {
        _appServices.SettingsManager.EnableHaraka();
    }

    private void ToggleOff()
    {
        _appServices.SettingsManager.DisableHaraka();
    }
}