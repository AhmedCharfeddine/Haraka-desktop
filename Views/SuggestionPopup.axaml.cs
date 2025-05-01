using Avalonia.Controls;
using Avalonia.Threading;
using Haraka.Services.CaretPositionProvider;
using System;
using System.Timers;

namespace Haraka.Views;

public partial class SuggestionPopup : Window
{
    private readonly Timer _autoCloseTimer;

    public SuggestionPopup()
    {
        InitializeComponent();
    }

    public void ShowNearCaret(ICaretPositionProvider caretProvider, string suggestion)
    {
        var caretPos = caretProvider.GetCaretScreenPosition();
        if (caretPos.HasValue)
        {
            Position = caretPos.Value;
        }

        SuggestionTextBlock.Text = suggestion;
        Show();
    }
}