using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Haraka.Services;
using Haraka.Utils;

namespace Haraka
{
    public partial class App : Application
    {
        private static AppServices? AppServices { get; set; }
        public override void Initialize()
        {
            AppServices = new AppServices();
            AvaloniaXamlLoader.Load(this);
            if (AppServices.SettingsManager.UserPreferences.IsHarakaEnabled)
            {
                AppServices.SettingsManager.EnableHaraka();
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;

                WindowManager.InitializeTrayIcon(AppServices);
                if (AppServices.SettingsManager.UserPreferences.ShowMainWindowOnStartup)
                {
                    WindowManager.OpenMainWindow(AppServices);
                }
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}