using VocabHelper.Core;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface ITextProcessingService
    {
        string ReadEpub(string epub);
        VocabularyRepositoryModel ProcessText(string text, Language language, VocabularyRepositoryModel? repository = null);
    }
}