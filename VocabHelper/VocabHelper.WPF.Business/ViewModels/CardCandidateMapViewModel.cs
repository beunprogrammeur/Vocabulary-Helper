using CommunityToolkit.Mvvm.ComponentModel;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class CardCandidateMapViewModel : BaseViewModel
    {
        [ObservableProperty] private string cardField;
        [ObservableProperty] private string candidateField;
    }
}
