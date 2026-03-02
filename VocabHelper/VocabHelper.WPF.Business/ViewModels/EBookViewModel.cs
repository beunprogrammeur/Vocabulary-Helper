using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.WPF.Business.Models;
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
            foreach (string deck in await _ankiService.GetDecks())
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
            var result = _fileSelectionService.OpenFile();
            if (!result.Success)
            {
                return;
            }
            var allWords = _ebookService.GetAllWords(result.File, false);

            IStemmerService stemmerService = _stemmerServiceFactory.GetStemmer(result.Language.Value);
            Dictionary<string, CardCandidateViewModel> stemmedWords = [];
            foreach (var word in allWords)
            {
                var stemmedWord = stemmerService.Stem(word.Word.ToLower());

                if (stemmedWords.TryGetValue(stemmedWord, out CardCandidateViewModel candidate))
                {
                    candidate.Frequency++;
                    if (!candidate.Variations.Contains(word.Word.ToLower()))
                    {
                        candidate.Variations.Add(word.Word.ToLower());
                    }
                }
                else
                {
                    stemmedWords[stemmedWord] = new CardCandidateViewModel()
                    {
                        Word = stemmedWord,
                        Sentence = word.Sentence,
                        Index = word.Index,
                        Variations = [stemmedWord],
                        Frequency = 1
                    };
                }
            }

            foreach (var candidate in stemmedWords.Values.OrderBy(x => x.Index))
            {
                CardCandidates.Add(candidate);
            }
        }

        [RelayCommand]
        private async void TranslateCards()
        {
            _translationService.Translate("apa kabar?", "id", "en");
        }

        [RelayCommand]
        private async void MarkCandidatesIgnoredFromDeck()
        {
            string[] decksToSearch = AnkiDecks.Where(x => x.Match).Select(x => x.DeckName).ToArray();

            Dictionary<string, AnkiCard> ankiCards = [];

            foreach (string deck in decksToSearch)
            {
                var cards = await _ankiService.GetCards(deck);
                foreach(var card in cards)
                {
                    if (!ankiCards.ContainsKey(card.SearchFieldValue.ToLower()))
                    {
                        ankiCards[card.SearchFieldValue.ToLower()] = card;
                    }
                }
            }

            foreach(var candidate in cardCandidates)
            {
                if (ankiCards.ContainsKey(candidate.Word.ToLower()))
                {
                    candidate.IsIgnored = true;
                }
            }
        }
    }
}
