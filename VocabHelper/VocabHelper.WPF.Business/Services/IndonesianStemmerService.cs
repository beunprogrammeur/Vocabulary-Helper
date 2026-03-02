using Sastrawi.Stemmer;

namespace VocabHelper.WPF.Business.Services
{
    internal class IndonesianStemmerService : IStemmerService
    {
        private IStemmer _stemmer;

        public IndonesianStemmerService()
        {
            _stemmer = new StemmerFactory().CreateStemmer();
        }

        public string Stem(string word)
        {
            if(string.IsNullOrEmpty(word))
            {
                return word;
            }

            return _stemmer.Stem(word);
        }
    }
}
