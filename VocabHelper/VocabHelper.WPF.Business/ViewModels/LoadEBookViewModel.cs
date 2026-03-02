using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.Core;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class LoadEBookViewModel : BaseViewModel
    {
        private readonly IStemmerServiceFactory _stemmerServiceFactory;
        private readonly IEBookService _ebookService;

        [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
        [ObservableProperty] 
        private string filePath;
        
        [ObservableProperty] 
        private LanguageId[] languages;
        
        [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
        [ObservableProperty] 
        private LanguageId? chosenLanguage;

        [ObservableProperty]
        private ObservableCollection<CardCandidateViewModel> cardCandidates;

        public event EventHandler? WordSelectionComplete;

        public LoadEBookViewModel(IStemmerServiceFactory stemmerServiceFactory, IEBookService ebookService)
        {
            FilePath = string.Empty;
            Languages = Enum.GetValues<LanguageId>();
            
            _stemmerServiceFactory = stemmerServiceFactory;
            _ebookService = ebookService;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteContinue))]
        private void Continue()
        {
            WordSelectionComplete?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteContinue()
            => File.Exists(FilePath) && ChosenLanguage != null;
    }
}
