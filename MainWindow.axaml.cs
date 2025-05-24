using Avalonia.Controls;
using Avalonia.Interactivity;
using Haraka.Services;
using Haraka.Utils;
using Haraka.ViewModels;

namespace Haraka
{
    public partial class MainWindow : Window
    {
        private readonly AppServices _appServices;

        public MainWindow(AppServices services)
        {
            _appServices = services;
            DataContext = new MainWindowViewModel(services);
            InitializeComponent();
            InitializeAppIcon();
        }

        private void InitializeAppIcon()
        {
            this.Icon = new WindowIcon(HarakaConstants.HARAKA_ICON_CIRCLE_PATH);
        }

        private async void Transliterate(object? sender, RoutedEventArgs e)
        {
            var input = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(input) || input.Length <= 3)
            {
                TransliterationResult.Text = "Please enter a word.";
                return;
            }

            var wrapper = new HarakaWrapper();
            // Run the CLI and get output
            string result = await wrapper.RunTransliterationAsync(input);
            TransliterationResult.Text = result;
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            _appServices.SettingsManager.Save();
        }
    }
}