using VocabHelper.Core;
using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.Services
{
    [RegisterService<IStemmerServiceFactory>]
    internal class StemmerServiceFactory : IStemmerServiceFactory
    {
        public IStemmerService GetStemmer(Language languageId)
        {
            return languageId switch
            {
                Language.Korean => new KoreanStemmerService(),
                Language.Indonesian => new IndonesianStemmerService(),
                _ => throw new NotSupportedException($"The language '{languageId}' is not supported.")
            };
        }
    }
}
