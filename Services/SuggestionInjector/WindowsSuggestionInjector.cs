using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Haraka.Services.SuggestionInjector
{
    internal class WindowsSuggestionInjector : ISuggestionInjector
    {
        private readonly InputSimulator _inputSimulator = new();

        public void Apply(string typedWord, string suggestion)
        {
            _inputSimulator.Keyboard.ModifiedKeyStroke(
                VirtualKeyCode.CONTROL,
                VirtualKeyCode.BACK);
            
            // Small delay to allow the system to process the backspace
            Thread.Sleep(50);

            _inputSimulator.Keyboard.TextEntry(suggestion + " ");
        }
    }
}
