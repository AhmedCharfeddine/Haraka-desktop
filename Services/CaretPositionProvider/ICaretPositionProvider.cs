using Avalonia;

namespace Haraka.Services.CaretPositionProvider
{
    public interface ICaretPositionProvider
    {
        /** 
         returns the caret position in screen coordinates, or the cursor position as a fallback
         */
        PixelPoint? GetCaretScreenPosition();
    }
}
