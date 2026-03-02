namespace VocabHelper.WPF.Business.Services
{
    internal class TranslationService : ITranslationService
    {
        public string Translate(string text, string sourceLanguage, string targetLanguage)
        {
            System.Diagnostics.Debug.WriteLine("Translate triggered!");
            return string.Empty;
        }
    }
}
