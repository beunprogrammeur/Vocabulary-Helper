using Microsoft.Extensions.DependencyInjection;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Business.Factories
{
    [RegisterService<IViewModelFactory>(RegistrationType.Singleton)]
    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CandidateToCardMappingViewModel GetCandidateToCardMappingViewModel() => _serviceProvider.GetRequiredService<CandidateToCardMappingViewModel>();
        public LoadEBookViewModel GetLoadEbookViewModel() => _serviceProvider.GetRequiredService<LoadEBookViewModel>();
        public LoadRawTextViewModel GetLoadRawTextViewModel() => _serviceProvider.GetRequiredService<LoadRawTextViewModel>();
        public EBookViewModel GetEBookViewModel() => _serviceProvider.GetRequiredService<EBookViewModel>();
        public MainViewModel GetMainViewModel() => _serviceProvider.GetRequiredService<MainViewModel>();
    }
}
