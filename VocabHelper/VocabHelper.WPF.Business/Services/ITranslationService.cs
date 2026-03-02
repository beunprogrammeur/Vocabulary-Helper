
using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, LanguageId sourceLanguage, LanguageId targetLanguage);
    }
}
