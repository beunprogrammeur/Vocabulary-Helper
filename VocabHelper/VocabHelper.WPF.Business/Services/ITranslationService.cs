
using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, Language sourceLanguage, Language targetLanguage);
    }
}
