using System;
using Avalonia.Animation;
using Avalonia;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Haraka.Views;

public partial class ToastNotificationWindow : Window
{
    public ToastNotificationWindow(bool state)
    {
        InitializeComponent();
        MessageTextBlock.Text = state ? "Haraka is ON" : "Haraka is OFF";
    }

    public async Task FadeInAsync(TimeSpan duration)
    {
        var tcs = new TaskCompletionSource();
        Opened += (_, _) => tcs.SetResult();
        Show();
        await tcs.Task;

        if (this.FindControl<Border>("RootBorder") is { } border)
        {
            var animation = new Animation
            {
                Duration = duration,
                Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(Visual.OpacityProperty, 0d) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(Visual.OpacityProperty, 1d) }
                }
            }
            };

            await animation.RunAsync(border, CancellationToken.None);
        }
    }

    public async Task FadeOutAsync(TimeSpan duration)
    {
        if (this.FindControl<Border>("RootBorder") is { } border)
        {
            var animation = new Animation
            {
                Duration = duration,
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters = { new Setter(Visual.OpacityProperty, Opacity) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters = { new Setter(Visual.OpacityProperty, 0d) }
                    }
                }
            };

            await animation.RunAsync(border, CancellationToken.None);
        }
    }
}