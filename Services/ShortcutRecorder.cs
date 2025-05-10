using Haraka.Services.KeyboardListeners;

namespace Haraka.Services
{
    public class ShortcutRecorder
    {
        private IKeyboardListener _keyboardListener { get; set; }

        public ShortcutRecorder(IKeyboardListener keyboardListener)
        {
            _keyboardListener = keyboardListener;
        }
    }
}
