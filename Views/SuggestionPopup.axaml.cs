using Avalonia.Controls;
using Avalonia.Platform;
using Haraka.Services.CaretPositionProvider;
using System;
using System.Runtime.InteropServices;

namespace Haraka.Views;

public partial class SuggestionPopup : Window
{
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