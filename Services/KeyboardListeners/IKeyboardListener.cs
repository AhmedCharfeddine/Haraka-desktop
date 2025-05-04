using System;

namespace Haraka.Services.KeyboardListeners
{
    public interface IKeyboardListener
    {
        event EventHandler<string> WordTyped;
        event EventHandler<string> WordAccepted;
        void StartListening()
        {
            Console.WriteLine("Keyboard listening is not supported on this platform.");
        }

        void StopListening()
        {
            Console.WriteLine("Keyboard listening is not supported on this platform.");
        }

        void ResetCurrentWord();
    }
}
