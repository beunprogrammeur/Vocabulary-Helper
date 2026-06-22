using VocabHelper.Core;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface ITextProcessingService
    {
        VocabularyRepositoryModel ProcessText(string text, LanguageId language, VocabularyRepositoryModel? repository = null);
    }
}