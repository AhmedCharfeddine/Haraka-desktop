using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Input;
using Haraka.Models;
using Haraka.Services;

namespace Haraka.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged, IDisposable
    {
        private AppServices _appServices { get; set; }
        private string _shortcutTmp { get; set; }
        private bool _isStartRecordingBtnEnabled = true;

        public SettingsViewModel() : this(null!)
        {
        }

        public SettingsViewModel(AppServices services)
        {
            _appServices = services;
            _appServices.ShortcutRecorder.ShortcutChanged += OnShortcutChanged;
            _appServices.ShortcutRecorder.RecordingEnded += OnRecordingEnd;
            _shortcutTmp = _appServices.SettingsManager.UserPreferences.Shortcut.ToString();
        }

        public void Dispose()
        {
            // unregister event handlers when closing settings window
            _appServices.ShortcutRecorder.ShortcutChanged -= OnShortcutChanged;
            _appServices.ShortcutRecorder.RecordingEnded -= OnRecordingEnd;
        }

        internal void StartRecordingShortcut() 
        {
            IsStartRecordingBtnEnabled = false;
            _appServices.ShortcutRecorder.StartRecording();
        }
        internal void HandleKeyDown(Key key) => _appServices.ShortcutRecorder.OnShortcutRecordingKeyDown(key);
        internal void HandleKeyUp(Key key) => _appServices.ShortcutRecorder.OnShortcutRecordingKeyUp(key);
        
        public string Shortcut
        {
            get => _shortcutTmp;
            set
            {
                _shortcutTmp = value;
                OnPropertyChanged(nameof(Shortcut));
            }
        }

        public bool IsStartRecordingBtnEnabled
        {
            get => _isStartRecordingBtnEnabled;
            set
            {
                _isStartRecordingBtnEnabled = value;
                OnPropertyChanged(nameof(IsStartRecordingBtnEnabled));
            }
        }

        private void OnRecordingEnd(object? sender, ToggleShortcut newShortcut)
        {
            if (Services.KeystrokeManagers.ShortcutRecorder.IsShortcutValid(newShortcut))
            {
                _appServices.SettingsManager.TemporaryShortcut = newShortcut;
            }
            else
            {
                Shortcut = _appServices.SettingsManager.UserPreferences.Shortcut.ToString();
            }
            IsStartRecordingBtnEnabled = true;
        }

        private void OnShortcutChanged(object? sender, ToggleShortcut newShortcut)
        {
            Shortcut = newShortcut.ToString();
        }

        public bool IsNotificationSoundEnabled
        {
            get => _appServices.SettingsManager.UserPreferences.IsNotificationSoundEnabled;
            set
            {
                _appServices.SettingsManager.UserPreferences.IsNotificationSoundEnabled = value;
                OnPropertyChanged(nameof(IsNotificationSoundEnabled));
            }
        }

        public bool IsHarakaEnabled
        {
            get => _appServices.SettingsManager.UserPreferences.IsHarakaEnabled;
            set
            {
                _appServices.SettingsManager.UserPreferences.IsHarakaEnabled = value;
                OnPropertyChanged(nameof(IsHarakaEnabled));
            }
        }

        public bool LaunchOnStartup
        {
            get => _appServices.SettingsManager.UserPreferences.LaunchOnStartup;
            set
            {
                _appServices.SettingsManager.UserPreferences.LaunchOnStartup = value;
                OnPropertyChanged(nameof(LaunchOnStartup));
            }
        }

        public void Save() 
        {
            if (_appServices.SettingsManager.TemporaryShortcut != null)
            { 
                _appServices.ShortcutRecorder.CommitToShortcut(_appServices.SettingsManager.TemporaryShortcut); 
            }
            _appServices.SettingsManager.Save(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}