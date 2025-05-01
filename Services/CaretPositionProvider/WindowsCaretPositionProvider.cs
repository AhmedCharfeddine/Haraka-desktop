#if WINDOWS
using System;
using System.Runtime.InteropServices;
using Avalonia;
using Haraka.Services.CaretPositionProvider;

public class WindowsCaretPositionProvider : ICaretPositionProvider
{
    [DllImport("user32.dll")]
    static extern bool GetCaretPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern IntPtr GetFocus();

    [DllImport("user32.dll")]
    static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public int X;
        public int Y;
    }

    public PixelPoint? GetCaretScreenPosition()
    {
        if (!GetCaretPos(out POINT caretPoint))
            return null;

        var hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
            return null;

        if (!ClientToScreen(hwnd, ref caretPoint))
            return null;

        return new PixelPoint(caretPoint.X, caretPoint.Y);
    }
}
#endif