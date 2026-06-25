using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF.Factories
{
    public interface IWindowFactory
    {
        IWindow<CandidateToCardMappingViewModel> GetCandidateToCardMappingWindow();
        IWindow<LoadTextViewModel> GetLoadRawTextWindow();
        IWindow<MainViewModel> GetMainWindow();
        IWindow<MessageBoxViewModel> GetMessageBoxWindow(MessageBoxType type, string message, string title);
    }
}