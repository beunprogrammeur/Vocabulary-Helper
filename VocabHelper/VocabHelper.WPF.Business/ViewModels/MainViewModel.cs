using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public class MainViewModel : BaseViewModel, IDialogViewModel
    {
        public EBookViewModel DictionaryViewModel { get; }
        public LoadTextViewModel TextLoadViewModel { get; }
        public CandidateToCardMappingViewModel CandidateToCardMappingViewModel { get; }
        public StatusViewModel StatusViewModel { get; }
        public MainViewModel(EBookViewModel dictionaryViewModel, 
            LoadTextViewModel textLoadViewModel, 
            CandidateToCardMappingViewModel candidateToCardMappingViewModel,
            StatusViewModel statusViewModel)
        {
            DictionaryViewModel = dictionaryViewModel;
            TextLoadViewModel = textLoadViewModel;
            CandidateToCardMappingViewModel = candidateToCardMappingViewModel;
            StatusViewModel = statusViewModel;
        }

        public string Title { get; set; } = "Anki Vocabulary Helper";

        public event EventHandler<CloseDialogEventArgs> CloseDialog;
    }
}
