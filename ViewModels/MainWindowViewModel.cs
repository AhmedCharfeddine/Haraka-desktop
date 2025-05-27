using System.ComponentModel;
using System.Runtime.CompilerServices;
using Haraka.Services;

namespace Haraka.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {        
        private AppServices _appServices;

        public bool ShowMainWindowOnStartup
        {
            get => _appServices.SettingsManager.UserPreferences.ShowMainWindowOnStartup; 
            set
            {
                _appServices.SettingsManager.UserPreferences.ShowMainWindowOnStartup = value;
                OnPropertyChanged(nameof(ShowMainWindowOnStartup));
            }
        }

        public string ToggleShortcutString
        {
            get => _appServices.SettingsManager.UserPreferences.Shortcut.ToString();
        }

        public MainWindowViewModel(AppServices appServices)
        {
            _appServices = appServices;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
