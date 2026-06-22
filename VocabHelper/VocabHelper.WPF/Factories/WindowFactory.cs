using Microsoft.Extensions.DependencyInjection;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.Factories;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF.Factories
{
    [RegisterService<IWindowFactory>]
    internal class WindowFactory : IWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IViewModelFactory _viewModelFactory;
        public WindowFactory(IServiceProvider serviceProvider, IViewModelFactory viewModelFactory)
        {
            _serviceProvider = serviceProvider;
            _viewModelFactory = viewModelFactory;
        }

        private IWindow<T> GetDialogWindow<T>(T dialogViewModel) where T: class, IDialogViewModel
        {
            DialogWindow window = _serviceProvider.GetRequiredService<DialogWindow>();
            window.ViewModel = dialogViewModel;
            return new DialogWindowAdapter<T>(window, dialogViewModel);
        }

        public IWindow<LoadEBookViewModel> GetLoadEBookWindow() => GetDialogWindow(_viewModelFactory.GetLoadEbookViewModel());
        public IWindow<LoadRawTextViewModel> GetLoadRawTextWindow() => GetDialogWindow(_viewModelFactory.GetLoadRawTextViewModel());
        public IWindow<MessageBoxViewModel> GetMessageBoxWindow(MessageBoxType type, string message, string title) => GetDialogWindow(new MessageBoxViewModel(type, message, title));
        public IWindow<CandidateToCardMappingViewModel> GetCandidateToCardMappingWindow() => GetDialogWindow(_viewModelFactory.GetCandidateToCardMappingViewModel());
        public IWindow<MainViewModel> GetMainWindow() => GetDialogWindow(_viewModelFactory.GetMainViewModel());
    }
}
