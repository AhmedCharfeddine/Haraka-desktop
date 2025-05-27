using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;

namespace HarakaDesktop.Controls;

public partial class CustomTitleBar : UserControl
{
    public CustomTitleBar()
    {
        InitializeComponent();
        this.FindControl<Border>("TitleBarBorder").PointerPressed += TitleBar_PointerPressed;
        var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "haraka-logo-circle.ico");
        if (File.Exists(iconPath))
        {
            AppIcon.Source = new Bitmap(iconPath);
        }
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var window = this.FindAncestorOfType<Window>();
            window?.BeginMoveDrag(e);
        }
    }

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
    {
        if (this.FindAncestorOfType<Window>() is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        this.FindAncestorOfType<Window>()?.Close();
    }
}
