using Haraka.Utils;

namespace Haraka.Models
{
    public class AppConfig
    {
        public string Strategy { get; set; } = HarakaConstants.DEFAULT_STRATEGY;
        public int MinWordLength { get; set; } = HarakaConstants.DEFAULT_MINIMUM_WORD_LENGTH;
        public int AutoClosePopupDelayMs { get; set; } = HarakaConstants.DEFAULT_AUTO_CLOSE_POPUP_DELAY_MS;
        public int PopupDebounceDelayMs { get; set; } = HarakaConstants.DEFAULT_POPUP_DEBOUNCE_DELAY_MS;
        public float NotificationVolume { get; set; } = HarakaConstants.DEFAULT_NOTIFICATION_VOLUME;
        public int AutoCloseToastDelayMs { get; set; } = HarakaConstants.DEFAULT_AUTO_CLOSE_TOAST_DELAY_MS;
        public int ToastXPadding { get; set; } = HarakaConstants.DEFAULT_X_TOAST_PADDING;
        public int ToastYPadding { get; set; } = HarakaConstants.DEFAULT_Y_TOAST_PADDING;
        public int FadeInOutAnimationDurationMs { get; set; } = HarakaConstants.FADE_IN_OUT_ANIMATION_DURATION_MS;
    }
}
