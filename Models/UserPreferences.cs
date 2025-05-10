namespace Haraka.Models;
public class UserPreferences
{
    public bool IsHarakaEnabled { get; set; } = false;
    public bool LaunchOnStartup { get; set; } = true;
    public bool IsNotificationSoundEnabled { get; set; } = true;
    public ToggleShortcut? Shortcut { get; set; }
}
