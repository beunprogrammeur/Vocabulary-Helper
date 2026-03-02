namespace VocabHelper.WPF.Business.Services
{
    public interface ITranslationService
    {
        string Translate(string text, string sourceLanguage, string targetLanguage);
    }
}
