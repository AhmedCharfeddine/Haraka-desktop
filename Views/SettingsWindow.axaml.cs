using System;
using Avalonia.Controls;
using Avalonia.Input;
using Haraka.Services;
using Haraka.ViewModels;

namespace Haraka.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow(AppServices services)
    {
        InitializeComponent();
        DataContext = new SettingsViewModel(services);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        if (DataContext is IDisposable disposable)
        { 
            disposable.Dispose(); 
        }
    }

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
        {
            vm.Save();
        }

        this.Close();
    }

    private void Start_Recording_Shortcut(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
        {
            ShortcutTextBox.Focus();
            ShortcutTextBox.Clear();
            vm.StartRecordingShortcut();
        }
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private SettingsViewModel ViewModel => (SettingsViewModel)DataContext!;

    private void OnShortcutKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
        {
            vm.HandleKeyDown(e.Key);
            e.Handled = true;
        }
    }

    private void OnShortcutKeyUp(object sender, KeyEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
        {
            vm.HandleKeyUp(e.Key);
            e.Handled = true;
        }
    }
}
