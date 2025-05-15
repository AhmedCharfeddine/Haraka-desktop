using System;
using Avalonia.Input;
using Haraka.Models;
using Haraka.Services.KeyboardListeners;
using Haraka.Utils;

namespace Haraka.Services.KeystrokeManagers
{
    public class ShortcutRecorder
    {
        public event EventHandler<ToggleShortcut>? ShortcutChanged;
        public event EventHandler<ToggleShortcut>? RecordingEnded;

        private SettingsManager _settingsManager { get; set; }
        private IKeyboardListener _keyboardListener { get; set; }
        private SoundPlayer _soundPlayer { get; set; }

        private bool _isRecordingShortcut = false;
        private bool _wasHarakaEnabledBeforeRecording = false;
        private ToggleShortcut? _currentShortcut;

        public ShortcutRecorder(IKeyboardListener keyboardListener, SettingsManager settingsManager, SoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
            _settingsManager = settingsManager;
            _keyboardListener = keyboardListener;
            _keyboardListener.ShortcutTriggered += OnShortcutTriggered;
            _keyboardListener.SetToggleShortcut(_settingsManager.UserPreferences.Shortcut);
            _keyboardListener.StartListeningForShortcut();
        }

        private void OnShortcutTriggered(object? sender, EventArgs e)
        {
            if (_settingsManager.UserPreferences.IsHarakaEnabled)
            {
                _settingsManager.DisableHaraka();
                WindowManager.OpenToastNotificationWindow(false);
                if (_settingsManager.UserPreferences.IsNotificationSoundEnabled) { _soundPlayer.PlayDisableSound(); }
            }
            else
            {
                _settingsManager.EnableHaraka();
                WindowManager.OpenToastNotificationWindow(true);
                if (_settingsManager.UserPreferences.IsNotificationSoundEnabled) { _soundPlayer.PlayEnableSound(); }
            }
        }

        private void OnShortcutRecorded(object? sender, ToggleShortcut e)
        {
            _settingsManager.UserPreferences.Shortcut = e;
        }

        public void OnShortcutRecordingKeyDown(Key key)
        {
            if (_isRecordingShortcut)
            {
                bool isRecordingOver = false;
                if (key == Key.LeftCtrl || key == Key.RightCtrl)
                {
                    _currentShortcut.Ctrl = true;
                }
                else if (key == Key.RightCtrl || key == Key.LeftAlt)
                {
                    _currentShortcut.Alt = true;
                }
                else if (key == Key.LeftShift || key == Key.RightShift)
                {
                    _currentShortcut.Shift = true;
                }
                else if (isKeyTerminal(key))
                {
                    _currentShortcut.Key = key;
                    isRecordingOver = true;
                }

                ShortcutChanged?.Invoke(this, _currentShortcut);
                
                if (isRecordingOver || key == Key.Escape)
                {
                    _isRecordingShortcut = false;
                    // TODO: send signal to end recording
                    RecordingEnded?.Invoke(this, _currentShortcut);
                    _keyboardListener.StartListeningForShortcut();
                    if (_wasHarakaEnabledBeforeRecording)
                    {
                        _settingsManager.EnableHaraka();
                        _wasHarakaEnabledBeforeRecording = false;
                    }
                }
            }
        }

        private bool isKeyTerminal(Key key)
        {
            return ((key >= Key.A && key <= Key.Z) ||
                // top-row number keys
                (key >= Key.D0 && key <= Key.D9) ||
                // numpad
                (key >= Key.NumPad0 && key <= Key.NumPad9) ||
                // function keys
                (key >= Key.F1 && key <= Key.F24));
        }

        public void OnShortcutRecordingKeyUp(Key key)
        {
            if (_isRecordingShortcut)
            {
                if (key == Key.LeftCtrl || key == Key.RightCtrl)
                {
                    _currentShortcut.Ctrl = false;
                }
                else if (key == Key.RightCtrl || key == Key.LeftAlt)
                {
                    _currentShortcut.Alt = false;
                }
                else if (key == Key.LeftShift || key == Key.RightShift)
                {
                    _currentShortcut.Shift = false;
                }
                ShortcutChanged?.Invoke(this, _currentShortcut);
            }
        }

        internal void StartRecording()
        {
            _wasHarakaEnabledBeforeRecording = _settingsManager.UserPreferences.IsHarakaEnabled;
            _settingsManager.DisableHaraka();
            _keyboardListener.StopListeningForShortcut();
            _isRecordingShortcut = true;
            _currentShortcut = new()
            {
                Ctrl = false,
                Alt = false,
                Shift = false,
                Key = null
            };
        }

        internal static bool IsShortcutValid(ToggleShortcut newShortcut)
        {
            bool isValid = true;

            if (newShortcut.Key == null)
            {
                isValid = false;
            }
            else if (IsModifier(newShortcut.Key))
            {
                isValid = false;
            }
            else if (!newShortcut.Ctrl && !newShortcut.Alt && !newShortcut.Alt)
            {
                isValid = false;
            }
            return isValid;
        }

        private static bool IsModifier(Key? key)
        {
            return key == Key.LeftCtrl || key == Key.RightCtrl ||
                 key == Key.LeftAlt || key == Key.RightAlt ||
                 key == Key.LeftShift || key == Key.RightShift;
        }

        internal void CommitToShortcut(ToggleShortcut newShortcut)
        {
            _currentShortcut = newShortcut;
            _keyboardListener.SetToggleShortcut(_currentShortcut);
        }
    }
}
