using System.Threading.Tasks;
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
            var input = InputTextBlock.Text;

            if (string.IsNullOrWhiteSpace(input) || input.Length <= 3)
            {
                TransliterationResult.Watermark = "Please type your input";
                return;
            }

            var wrapper = new HarakaWrapper();
            // Run the CLI and get output
            string result = await wrapper.RunTransliterationAsync(input);

            await AnimateTypingAsync(result);
        }

        private async Task AnimateTypingAsync(string text)
        {
            TransliterationResult.Text = string.Empty;

            foreach (char c in text)
            {
                TransliterationResult.Text += c;
                await Task.Delay(20);
            }
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            _appServices.SettingsManager.Save();
        }
    }
}