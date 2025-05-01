using Gma.System.MouseKeyHook;
using System;
using System.Text;
#if WINDOWS
using System.Windows.Forms;
#endif

namespace Haraka.Services.KeyboardListeners
{
    public class WindowsKeyboardListener : IKeyboardListener
    {
        public event EventHandler<string>? WordTyped;

        private IKeyboardMouseEvents? _globalHook;
        private StringBuilder _currentWord = new StringBuilder();
        private bool _isListening = false;

        public void StartListening()
        {
#if WINDOWS
            if (_isListening)
                return;

            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyPress += OnKeyPress;
            _globalHook.KeyDown += OnKeyDown;
            _isListening = true;
#endif
        }

        public void StopListening()
        {
#if WINDOWS
            if (!_isListening)
                return;

            if (_globalHook != null)
            {
                _globalHook.KeyPress -= OnKeyPress;
                _globalHook.Dispose();
                _globalHook = null;
            }

            _isListening = false;
            ResetCurrentWord();
#endif
        }

        public void ResetCurrentWord()
        {
#if WINDOWS
            _currentWord.Clear();
#endif
        }

#if WINDOWS
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
            if (_currentWord.Length > 0)
            {
                WordTyped?.Invoke(this, _currentWord.ToString());
            }
        }
#endif
        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
#if WINDOWS
            if (e.KeyCode == Keys.Back)
            {
                if (_currentWord.Length > 0)
                    _currentWord.Length -= 1;
            }
            else if (e.Control && e.KeyCode == Keys.Back)
            {
                // Wipe the current word (simulate ctrl+backspace)
                ResetCurrentWord();
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                // Assume caret moved -> invalidate the buffer
                ResetCurrentWord();
            }
        }
#endif
    }
}
