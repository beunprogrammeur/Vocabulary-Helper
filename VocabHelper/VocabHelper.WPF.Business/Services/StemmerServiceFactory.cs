using VocabHelper.Core;
using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.Services
{
    [RegisterService<IStemmerServiceFactory>]
    internal class StemmerServiceFactory : IStemmerServiceFactory
    {
        public IStemmerService GetStemmer(LanguageId languageId)
        {
            return languageId switch
            {
                LanguageId.Korean => new KoreanStemmerService(),
                LanguageId.Indonesian => new IndonesianStemmerService(),
                _ => throw new NotSupportedException($"The language '{languageId}' is not supported.")
            };
        }
    }
}
