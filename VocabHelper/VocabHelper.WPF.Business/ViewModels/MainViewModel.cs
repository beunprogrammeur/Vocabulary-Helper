using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public class MainViewModel : BaseViewModel, IDialogViewModel
    {
        public EBookViewModel DictionaryViewModel { get; }
        public LoadRawTextViewModel TextLoadViewModel { get; }
        public CandidateToCardMappingViewModel CandidateToCardMappingViewModel { get; }
        public MainViewModel(EBookViewModel dictionaryViewModel, 
            LoadRawTextViewModel textLoadViewModel, 
            CandidateToCardMappingViewModel candidateToCardMappingViewModel)
        {
            DictionaryViewModel = dictionaryViewModel;
            TextLoadViewModel = textLoadViewModel;
            CandidateToCardMappingViewModel = candidateToCardMappingViewModel;
        }

        public string Title { get; set; } = "Anki Vocabulary Helper";

        public event EventHandler<CloseDialogEventArgs> CloseDialog;
    }
}
