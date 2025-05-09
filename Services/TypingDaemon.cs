using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Threading;
using Haraka.Services.CaretPositionProvider;
using Haraka.Services.KeyboardListeners;
using Haraka.Services.SuggestionInjector;
using Haraka.Utils;
using Haraka.Views;

namespace Haraka.Services
{
    public class TypingDaemon
    {
        private readonly IKeyboardListener _keyboardListener;
        private readonly ICaretPositionProvider _caretPositionProvider;
        private readonly ISuggestionInjector _suggestionInjector;
        private readonly HarakaWrapper _harakaWrapper;
        private readonly SuggestionPopup _suggestionPopup;
        private readonly Timer _debounceTimer;
        private readonly Timer _autoClosePopupTimer;
        private string _latestWord = string.Empty;

        private static readonly Lazy<TypingDaemon> _instance =
            new(() => new TypingDaemon());

        public static TypingDaemon Instance => _instance.Value;

        public TypingDaemon()
        {
            _harakaWrapper = new HarakaWrapper();
            _suggestionPopup = new SuggestionPopup();
            _debounceTimer = new Timer(ConfigManager.Config.PopupDebounceDelayMs);
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += async (_, _) => await OnDebounceElapsedAsync();

            _keyboardListener = new WindowsKeyboardListener();
            _caretPositionProvider = new WindowsCaretPositionProvider();
            _suggestionInjector = new WindowsSuggestionInjector();

            _keyboardListener.WordTyped += OnWordTyped;
            _keyboardListener.WordAccepted += OnWordAccepted;

            // Auto-close popup after delay
            _autoClosePopupTimer = new Timer(ConfigManager.Config.AutoClosePopupDelayMs);
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
            if (_latestWord.Length >= ConfigManager.Config.MinWordLength)
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
        }

        private void OnWordTyped(object? sender, string word)
        {
            _latestWord = word;
            _debounceTimer.Stop();
            _debounceTimer.Start();

            if (string.IsNullOrEmpty(word))
            {
                _suggestionPopup.Hide();
            }
        }

        private async void OnWordAccepted(object? sender, string word)
        {
            if (_suggestionPopup.IsVisible
                && _latestWord.Length >= ConfigManager.Config.MinWordLength
                && string.Equals(_latestWord, word))
            {
                 var suggestion = await _harakaWrapper.RunTransliterationAsync(word);
                _suggestionInjector.Apply(word, suggestion);
            }
            _latestWord = string.Empty;
            _suggestionPopup.Hide();
        }
    }
}
