using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Haraka.Services;

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
                desktop.MainWindow = new MainWindow(AppServices);
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}