using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Business.Factories
{
    public interface IViewModelFactory
    {
        CandidateToCardMappingViewModel GetCandidateToCardMappingViewModel();
        LoadEBookViewModel GetLoadEbookViewModel();
        LoadTextViewModel GetLoadRawTextViewModel();
        EBookViewModel GetEBookViewModel();
        MainViewModel GetMainViewModel();
    }
}