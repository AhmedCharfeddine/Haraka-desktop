namespace Haraka.Services.SuggestionInjector
{
    public interface ISuggestionInjector
    {
        void Apply(string typedWord, string suggestion);
    }
}
