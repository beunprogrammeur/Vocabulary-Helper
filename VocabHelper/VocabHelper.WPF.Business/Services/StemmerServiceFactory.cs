namespace VocabHelper.WPF.Business.Services
{
    // todo: potentially move to a Core project 
    internal enum LanguageId
    {
        Korean,
        Indonesian
    };

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
