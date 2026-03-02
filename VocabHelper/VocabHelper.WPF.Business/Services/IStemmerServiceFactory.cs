using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Services
{
    public interface IStemmerServiceFactory
    {
        IStemmerService GetStemmer(LanguageId languageId);
    }
}
