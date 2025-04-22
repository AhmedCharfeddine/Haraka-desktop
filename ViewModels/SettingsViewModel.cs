using System.ComponentModel;
using System.Runtime.CompilerServices;
using Haraka.Utils;

namespace Haraka.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel() { }

        public string Shortcut
        {
            get => SettingsManager.UserPreferences.Shortcut;
            set
            {
                SettingsManager.UserPreferences.Shortcut = value;
                OnPropertyChanged(nameof(Shortcut));
            }
        }

        public bool IsNotificationSoundEnabled
        {
            get => SettingsManager.UserPreferences.IsNotificationSoundEnabled;
            set
            {
                SettingsManager.UserPreferences.IsNotificationSoundEnabled = value;
                OnPropertyChanged(nameof(IsNotificationSoundEnabled));
            }
        }

        public bool IsHarakaEnabled
        {
            get => SettingsManager.UserPreferences.IsHarakaEnabled;
            set
            {
                SettingsManager.UserPreferences.IsHarakaEnabled = value;
                OnPropertyChanged(nameof(IsHarakaEnabled));
            }
        }

        public bool LaunchOnStartup
        {
            get => SettingsManager.UserPreferences.LaunchOnStartup;
            set
            {
                SettingsManager.UserPreferences.LaunchOnStartup = value;
                OnPropertyChanged(nameof(LaunchOnStartup));
            }
        }

        public void Save() => SettingsManager.UserPreferences.Save();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}