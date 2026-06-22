using CommunityToolkit.Mvvm.ComponentModel;
using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService(RegistrationType.Singleton)]
    public partial class StatusViewModel : BaseViewModel
    {
        [ObservableProperty] private int completedTranslations;
        [ObservableProperty] private int totalTranslations;
        
        public StatusViewModel()
        {

        }
    }
}
