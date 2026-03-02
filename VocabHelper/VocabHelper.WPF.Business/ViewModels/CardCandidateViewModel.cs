using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class CardCandidateViewModel : BaseViewModel
    {
        [ObservableProperty] private bool isIgnored;
        [ObservableProperty] private string word;
        [ObservableProperty] private string wordTranslation;
        [ObservableProperty] private string sentence;
        [ObservableProperty] private string sentenceTranslation;
        [ObservableProperty] private ObservableCollection<string> variations = [];
        [ObservableProperty] private int index;
    }
}
