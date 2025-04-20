using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Haraka.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private UserPreferences _prefs;

        public SettingsViewModel()
        {
            _prefs = UserPreferences.Load();
        }

        public string Shortcut
        {
            get => _prefs.Shortcut;
            set
            {
                _prefs.Shortcut = value;
                OnPropertyChanged();
            }
        }

        public bool LaunchOnStartup
        {
            get => _prefs.LaunchOnStartup;
            set
            {
                _prefs.LaunchOnStartup = value;
                OnPropertyChanged();
            }
        }

        public void Save() => _prefs.Save();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}