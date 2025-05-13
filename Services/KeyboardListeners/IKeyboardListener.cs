using System;
using Haraka.Models;

namespace Haraka.Services.KeyboardListeners
{
    public interface IKeyboardListener
    {
        event EventHandler<string> WordTyped;
        event EventHandler<string> WordAccepted;
        event EventHandler? ShortcutTriggered;

        void StartListeningForShortcut();
        void StopListeningForShortcut();

        void StartListeningForTyping();
        void StopListeningForTyping();
        void SetToggleShortcut(ToggleShortcut shortcut);

        void ResetCurrentWord();
    }
}
