using Gma.System.MouseKeyHook;
using Haraka.Models;
using System;
using System.Text;
using System.Windows.Forms;

using Key = Avalonia.Input.Key;

namespace Haraka.Services.KeyboardListeners
{
    public class WindowsKeyboardListener : IKeyboardListener
    {
        public event EventHandler<string>? WordTyped;
        public event EventHandler<string>? WordAccepted;
        public event EventHandler? ShortcutTriggered;

        private IKeyboardMouseEvents _globalHook = Hook.GlobalEvents();
        private readonly StringBuilder _currentWord = new();
        private ToggleShortcut _shortcut;
        private Keys _terminalShortcutKey;

        private bool _isTypingActive = false;
        private bool _isShortcutRecordingActive = false;

        public void StartListeningForTyping()
        {
            if (!_isTypingActive)
            {
                _globalHook.KeyPress += OnKeyPress;
                _globalHook.KeyDown += OnKeyDown;
                _globalHook.MouseClick += OnMouseClick;
                _isTypingActive = true;
            }
        }

        public void StopListeningForTyping()
        {
            if (_isTypingActive)
            {
                if (_globalHook != null)
                {
                    _globalHook.KeyPress -= OnKeyPress;
                    _globalHook.KeyDown -= OnKeyDown;
                    _globalHook.MouseClick -= OnMouseClick;
                }

                _isTypingActive = false;
                ResetCurrentWord();
            }
        }

        public void ResetCurrentWord()
        {
            _currentWord.Clear();
            WordTyped?.Invoke(this, String.Empty);
        }

        public void StartListeningForShortcut()
        {
            if (!_isShortcutRecordingActive)
            {
                _globalHook.KeyDown += ShortcutRecordOnKeyDown;
                _isShortcutRecordingActive = true;
            }
        }

        public void StopListeningForShortcut()
        {
            if (_isShortcutRecordingActive)
            {
                _globalHook.KeyDown -= ShortcutRecordOnKeyDown;
                _isShortcutRecordingActive = false;
            }
        }

        public void SetToggleShortcut(ToggleShortcut shortcut)
        {
            _shortcut = shortcut;
            _terminalShortcutKey = getAvaloniaKeyMapping(shortcut.Key);
        }

        private void ShortcutRecordOnKeyDown(object? sender, KeyEventArgs e)
        {
            if (!(_shortcut.Ctrl ^ e.Control)
                && !(_shortcut.Alt ^ e.Alt)
                && !(_shortcut.Shift ^ e.Shift)
                && (_terminalShortcutKey == e.KeyCode))
            {
                ShortcutTriggered?.Invoke(this, e);
            }
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

        private Keys getAvaloniaKeyMapping(Key? key)
        {
            switch (key)
            {
                case Key.A: return Keys.A;
                case Key.B: return Keys.B;
                case Key.C: return Keys.C;
                case Key.D: return Keys.D;
                case Key.E: return Keys.E;
                case Key.F: return Keys.F;
                case Key.G: return Keys.G;
                case Key.H: return Keys.H;
                case Key.I: return Keys.I;
                case Key.J: return Keys.J;
                case Key.K: return Keys.K;
                case Key.L: return Keys.L;
                case Key.M: return Keys.M;
                case Key.N: return Keys.N;
                case Key.O: return Keys.O;
                case Key.P: return Keys.P;
                case Key.Q: return Keys.Q;
                case Key.R: return Keys.R;
                case Key.S: return Keys.S;
                case Key.T: return Keys.T;
                case Key.U: return Keys.U;
                case Key.V: return Keys.V;
                case Key.W: return Keys.W;
                case Key.X: return Keys.X;
                case Key.Y: return Keys.Y;
                case Key.Z: return Keys.Z;
                case Key.D0: return Keys.D0;
                case Key.D1: return Keys.D1;
                case Key.D2: return Keys.D2;
                case Key.D3: return Keys.D3;
                case Key.D4: return Keys.D4;
                case Key.D5: return Keys.D5;
                case Key.D6: return Keys.D6;
                case Key.D7: return Keys.D7;
                case Key.D8: return Keys.D8;
                case Key.D9: return Keys.D9;
                case Key.NumPad0: return Keys.NumPad0;
                case Key.NumPad1: return Keys.NumPad1;
                case Key.NumPad2: return Keys.NumPad2;
                case Key.NumPad3: return Keys.NumPad3;
                case Key.NumPad4: return Keys.NumPad4;
                case Key.NumPad5: return Keys.NumPad5;
                case Key.NumPad6: return Keys.NumPad6;
                case Key.NumPad7: return Keys.NumPad7;
                case Key.NumPad8: return Keys.NumPad8;
                case Key.NumPad9: return Keys.NumPad9;
                case Key.F1: return Keys.F1;
                case Key.F2: return Keys.F2;
                case Key.F3: return Keys.F3;
                case Key.F4: return Keys.F4;
                case Key.F5: return Keys.F5;
                case Key.F6: return Keys.F6;
                case Key.F7: return Keys.F7;
                case Key.F8: return Keys.F8;
                case Key.F9: return Keys.F9;
                case Key.F10: return Keys.F10;
                case Key.F11: return Keys.F11;
                case Key.F12: return Keys.F12;
                case Key.Space: return Keys.Space;
                default: return Keys.None;
            }
        }
    }
}
