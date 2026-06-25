using CommunityToolkit.Mvvm.ComponentModel;
using VocabHelper.Core;

namespace VocabHelper.WPF.Business.ViewModels.Persistence
{
    public partial class LanguageSettings : BaseViewModel
    {
        [ObservableProperty]
        private string sourceLanguage;
        [ObservableProperty]
        private string targetLanguage;
    }
}
