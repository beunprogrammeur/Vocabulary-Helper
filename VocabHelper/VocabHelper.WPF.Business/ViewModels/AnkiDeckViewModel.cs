using CommunityToolkit.Mvvm.ComponentModel;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class AnkiDeckViewModel : BaseViewModel
    {
        [ObservableProperty] private string deckName;
        [ObservableProperty] private bool match;
    }
}
