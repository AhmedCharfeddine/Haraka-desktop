using System.Windows.Forms;

namespace Haraka.Models
{
    public class ToggleShortcut
    {
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public Keys Key { get; set; }
    }
}
