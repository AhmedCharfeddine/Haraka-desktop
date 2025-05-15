using Avalonia;
using Avalonia.Controls;
using Haraka.Utils;

namespace Haraka.Views;

public partial class ToastNotificationWindow : Window
{
    public ToastNotificationWindow(bool state)
    {
        InitializeComponent();
        MessageTextBlock.Text = state ? "Haraka is enabled" : "Haraka is disabled";
    }
}