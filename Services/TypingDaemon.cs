using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Threading;
using Haraka.Services.CaretPositionProvider;
using Haraka.Services.KeyboardListeners;
using Haraka.Views;

namespace Haraka.Services
{
    public class TypingDaemon
    {
        private readonly IKeyboardListener _keyboardListener;
        private readonly ICaretPositionProvider _caretPositionProvider;
        private readonly HarakaWrapper _harakaWrapper;
        private readonly SuggestionPopup _suggestionPopup;
        private Timer _debounceTimer;
        private Timer _autoClosePopupTimer;
        private string _latestWord;

        private static readonly Lazy<TypingDaemon> _instance =
            new Lazy<TypingDaemon>(() => new TypingDaemon());

        public static TypingDaemon Instance => _instance.Value;

        public TypingDaemon()
        {
            _harakaWrapper = new HarakaWrapper();
            _suggestionPopup = new SuggestionPopup();
            _debounceTimer = new Timer(300);
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += async (_, _) => await OnDebounceElapsedAsync();

            if (OperatingSystem.IsWindows())
            {
                _keyboardListener = new WindowsKeyboardListener();
                _caretPositionProvider = new WindowsCaretPositionProvider();
            }
            else
            {
                _keyboardListener = new LinuxKeyboardListener();
                _caretPositionProvider = new DefaultCaretPositionProvider();
            }

            _keyboardListener.WordTyped += OnWordTyped;

            // Auto-close after 4 seconds
            _autoClosePopupTimer = new Timer(4000);
            _autoClosePopupTimer.Elapsed += (s, e) => Dispatcher.UIThread.Post(() =>
            {

                _suggestionPopup.Hide();
                _keyboardListener.ResetCurrentWord();

            });
            _autoClosePopupTimer.AutoReset = false;
        }

        public void Start()
        {
            _keyboardListener.StartListening();
        }

        public void Stop()
        {
            _keyboardListener.StopListening();
            _debounceTimer.Stop();
        }

        private async Task OnDebounceElapsedAsync()
        {
            var suggestion = await _harakaWrapper.RunTransliterationAsync(_latestWord);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (!string.IsNullOrWhiteSpace(suggestion))
                {
                    _autoClosePopupTimer?.Stop();
                    _autoClosePopupTimer?.Start();
                    _suggestionPopup.ShowNearCaret(_caretPositionProvider, suggestion);
                }
                else
                {
                    _suggestionPopup.Hide();
                }
            });
        }

        private void OnWordTyped(object? sender, string word)
        {
            _latestWord = word;
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }
    }
}
