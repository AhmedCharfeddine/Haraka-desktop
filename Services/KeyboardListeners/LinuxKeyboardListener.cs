using System;

namespace Haraka.Services.KeyboardListeners
{
    public class LinuxKeyboardListener : IKeyboardListener
    {
        public event EventHandler<string>? WordTyped;

        public void ResetCurrentWord()
        {
            throw new NotImplementedException();
        }

        public void StartListening()
        {
            // TODO: Implement when adding Linux support
        }

        public void StopListening()
        {
            // TODO: Implement when adding Linux support
        }
    }
}
