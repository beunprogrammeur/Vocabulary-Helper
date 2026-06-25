using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.ViewModels.Persistence
{
    public class PersistentSettings : BaseViewModel
    {
        public LanguageSettings LanguageSettings { get; set; } = new();
        public ApiSettings ApiSettings { get; set; } = new ();
    }
}
