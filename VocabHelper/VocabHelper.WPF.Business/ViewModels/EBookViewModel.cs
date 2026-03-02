using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class EBookViewModel : BaseViewModel
    {
        private readonly ITranslationService _translationService;
        private readonly IAnkiService _ankiService;
        private readonly IEBookService _ebookService;
        private readonly IFileSelectionService _fileSelectionService;
        private readonly IStemmerServiceFactory _stemmerServiceFactory;

        [ObservableProperty] private ObservableCollection<CardCandidateViewModel> cardCandidates = [];
        [ObservableProperty] private ObservableCollection<AnkiDeckViewModel> ankiDecks = [];

        public EBookViewModel(ITranslationService translationService, 
            IAnkiService ankiSerivce, IEBookService ebookService, IFileSelectionService fileSelectionService,
            IStemmerServiceFactory stemmerServiceFactory)
        {
            _translationService = translationService;
            _ankiService = ankiSerivce;
            _ebookService = ebookService;
            _fileSelectionService = fileSelectionService;
            _stemmerServiceFactory = stemmerServiceFactory;
        }

        [RelayCommand]
        private async void LoadAnkiDecksAsync()
        {
            AnkiDecks.Clear();
            foreach(string deck in await _ankiService.GetDecks())
            {
                AnkiDecks.Add(new AnkiDeckViewModel() { DeckName = deck });
            }
        }

        [RelayCommand]
        private void ClearData()
        {
            cardCandidates.Clear();
        }

        [RelayCommand]
        private void LoadEBook()
        {
            if (_fileSelectionService.OpenFile(out string path))
            {
                ClearData();
                var words = _ebookService.GetAllWords(path, true);
                // TODO: do something with a stemmer to reduce the amount of entries.
                foreach(var entry in words)
                {
                    cardCandidates.Add(new CardCandidateViewModel()
                    {
                        Word = entry.Word,
                        Sentence = entry.Sentence,
                    });
                }
            }
        }

        [RelayCommand]
        private async void TranslateCards()
        {
            _translationService.Translate("apa kabar?", "id", "en");
        }
    }
}
