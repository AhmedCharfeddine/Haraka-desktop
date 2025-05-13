using System.Text;
using Avalonia.Input;

namespace Haraka.Models
{
    public class ToggleShortcut
    {
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public Key? Key { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new();
            if (Ctrl)
            {
                builder.Append("Ctrl");
            }
            if (Shift)
            {
                if (Ctrl)
                {
                    builder.Append('+');
                }
                builder.Append("Shift");
            }
            if (Alt)
            {
                if (Ctrl || Shift)
                {
                    builder.Append('+');
                }
                builder.Append("Alt");
            }
            if (Key != null)
            {
                if (Ctrl || Shift || Alt)
                {
                    builder.Append('+');
                }
                builder.Append(Key.ToString());
            }

            return builder.ToString();
        }
    }
}
