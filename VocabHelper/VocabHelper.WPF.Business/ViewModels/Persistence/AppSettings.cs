using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.ViewModels.Persistence
{
    public class AppSettings : BaseViewModel
    {
        public LanguageSettings LanguageSettings { get; set; } = new();
    }
}
