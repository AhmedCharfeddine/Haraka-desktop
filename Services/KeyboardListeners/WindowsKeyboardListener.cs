using Gma.System.MouseKeyHook;
using System;
using System.Text;
using System.Windows.Forms;

namespace Haraka.Services.KeyboardListeners
{
    public class WindowsKeyboardListener : IKeyboardListener
    {
        public event EventHandler<string>? WordTyped;
        public event EventHandler<string> WordAccepted;

        private IKeyboardMouseEvents? _globalHook;
        private readonly StringBuilder _currentWord = new();
        private bool _isListening = false;
        public event Action<Shortcut>? ShortcutRecorded;
        private bool _recordingHotkey = false;


        public void StartListening()
        {
            if (!_isListening)
            {
                _globalHook = Hook.GlobalEvents();
                _globalHook.KeyPress += OnKeyPress;
                _globalHook.KeyDown += OnKeyDown;
                _globalHook.MouseClick += OnMouseClick;
                _isListening = true;
            }
        }

        public void StopListening()
        {
            if (_isListening)
            {
                if (_globalHook != null)
                {
                    _globalHook.KeyPress -= OnKeyPress;
                    _globalHook.KeyDown -= OnKeyDown;
                    _globalHook.MouseClick -= OnMouseClick;
                    _globalHook.Dispose();
                    _globalHook = null;
                }

                _isListening = false;
                ResetCurrentWord();
            }
        }

        public void ResetCurrentWord()
        {
            _currentWord.Clear();
            WordTyped?.Invoke(this, String.Empty);
        }

        private void OnKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                ResetCurrentWord();
            }
            else if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '\'')
            {
                _currentWord.Append(e.KeyChar);
            }
            WordTyped?.Invoke(this, _currentWord.ToString());
        }
        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            // Clear Buffer on mouse clicks
            ResetCurrentWord();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (e.Control)
                {
                    // Wipe the current word (simulate ctrl+backspace)
                    ResetCurrentWord();
                }
                else
                {
                    if (_currentWord.Length > 0)
                        _currentWord.Length -= 1;
                }
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                // Assume caret moved -> invalidate the buffer
                // TODO: could probably improve this behavior
                ResetCurrentWord();
            }
            else if (e.KeyCode == Keys.Space)
            {
                WordAccepted?.Invoke(this, _currentWord.ToString());
                ResetCurrentWord();
            }
        }

        public void StartRecordingShortcut()
        {
            _recordingHotkey = true;
        }

        public void StopRecordingShortcut()
        {
            _recordingHotkey = false;
        }
        private void OnShortcutRecordingKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (_recordingHotkey)
            {
                var shortcut = BuildShortcutFromCurrentState(e);
                ShortcutRecorded?.Invoke(shortcut);
                _recordingHotkey = false;
                return;
            }

            // Normal key press handling for TypingDaemon
            // WordTyped?.Invoke(...);
        }

        private Shortcut BuildShortcutFromCurrentState(KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
