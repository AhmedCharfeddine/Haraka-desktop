using Haraka.Services.CaretPositionProvider;
using Haraka.Services.KeyboardListeners;
using Haraka.Services.KeystrokeManagers;
using Haraka.Services.SuggestionInjector;
using Haraka.Utils;

namespace Haraka.Services
{
    public class AppServices
    {

        public IKeyboardListener KeyboardListener { get; }
        public WindowsCaretPositionProvider CaretPositionProvider { get; }
        public WindowsSuggestionInjector SuggestionInjector { get; }
        public TypingDaemon TypingDaemon { get; }
        public ShortcutRecorder ShortcutRecorder { get; }
        public SettingsManager SettingsManager { get; }

        public AppServices()
        {
            KeyboardListener = new WindowsKeyboardListener();
            CaretPositionProvider = new WindowsCaretPositionProvider();
            SuggestionInjector = new WindowsSuggestionInjector();

            TypingDaemon = new TypingDaemon(KeyboardListener, CaretPositionProvider, SuggestionInjector);
            SettingsManager = new SettingsManager(TypingDaemon);
            ShortcutRecorder = new ShortcutRecorder(KeyboardListener, SettingsManager);
        }
    }
}
