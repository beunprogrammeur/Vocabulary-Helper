using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class EBookViewModel : BaseViewModel, IDialogViewModel
    {
        private readonly ITranslationService _translationService;
        private readonly IAnkiService _ankiService;
        private readonly IDialogService _dialogService;
        private readonly StatusViewModel _statusViewModel;

        private AnkiMappingModel _mappings;

        [ObservableProperty] private Language language;
        [ObservableProperty] private int totalTranslations;
        [ObservableProperty] private int completedTranslations;
        [ObservableProperty] private ObservableCollection<CardCandidateViewModel> cardCandidates = [];
        [ObservableProperty] private ObservableCollection<AnkiDeckViewModel> ankiDecks = [];

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public string Title { get; set; } = "Anki text importer";

        public EBookViewModel(ITranslationService translationService,
            IAnkiService ankiSerivce, IDialogService dialogService,
            StatusViewModel statusViewModel)
        {
            _translationService = translationService;
            _ankiService = ankiSerivce;
            _dialogService = dialogService;
            _statusViewModel = statusViewModel;

            _statusViewModel.WordRepositoryUpdated += OnWordRepositoryUpdated;
            Language = Core.Language.English; // TODO: move this to a settings thing (statusviewmodel for now).
        }

        private void OnWordRepositoryUpdated(object? sender, System.EventArgs e)
        {
            CardCandidates.Clear();
            foreach (var word in _statusViewModel.WordRepository.Words.Values.OrderBy(x => x.Index))
            {
                CardCandidates.Add(new CardCandidateViewModel()
                {
                    Word = word.Word,
                    Sentence = word.Sentence,
                    Index = word.Index,
                    Frequency = word.Count,
                    Variations = new ObservableCollection<string>(word.Variations)
                });
            }
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
        private async Task TranslateCards()
        {
            var activeCandidates = cardCandidates.Where(x => !x.IsIgnored);
            TotalTranslations = activeCandidates.Count();
            CompletedTranslations = 0;

            foreach (var candidate in activeCandidates)
            {
                if (string.IsNullOrEmpty(candidate.WordTranslation))
                {
                    candidate.WordTranslation =
                        await _translationService.TranslateAsync(
                            candidate.Word,
                            _statusViewModel.WordRepository.Language,
                            Core.Language.English);
                    CompletedTranslations++;
                }

                if (string.IsNullOrEmpty(candidate.SentenceTranslation))
                {

                    candidate.SentenceTranslation =
                        await _translationService.TranslateAsync(
                            candidate.Sentence,
                            _statusViewModel.WordRepository.Language,
                            Core.Language.English);
                }
            }
        }


        [RelayCommand]
        private async void MarkCandidatesIgnoredFromDeck()
        {
            string[] decksToSearch = AnkiDecks.Where(x => x.Match).Select(x => x.DeckName).ToArray();

            Dictionary<string, AnkiCard> ankiCards = [];

            foreach (string deck in decksToSearch)
            {
                var cards = await _ankiService.GetCards(deck);
                foreach (var card in cards)
                {
                    if (!ankiCards.ContainsKey(card.SearchFieldValue.ToLower()))
                    {
                        ankiCards[card.SearchFieldValue.ToLower()] = card;
                    }
                }
            }

            foreach (var candidate in cardCandidates)
            {
                if (ankiCards.ContainsKey(candidate.Word.ToLower()))
                {
                    candidate.IsIgnored = true;
                }
            }
        }

        [RelayCommand]
        private async void Mapping()
        {
            _mappings = _dialogService.GetMapping(_mappings);
        }

        [RelayCommand]
        private async void Import()
        {
            var mapping = _mappings;
            
            if(mapping == null)
            {
                _dialogService.ShowMessageBox(MessageBoxType.Ok, "Please do a mapping first", "Error");
                return;
            }


            AnkiBulkImportModel ankiBulkImportModel = new();

            foreach(var candidate in CardCandidates.Where(x => !x.IsIgnored && !string.IsNullOrEmpty(x.WordTranslation)))
            {
                ankiBulkImportModel.Notes.Add(new AnkiNoteImportModel()
                {
                    tags = mapping.Tags,
                    fields = CreateFieldsForCardCandiate(mapping.Mappings, candidate),
                    deckName = mapping.DeckName,
                    modelName = mapping.CardName,
                });
            }

            if (!ankiBulkImportModel.Notes.Any())
            {
                _dialogService.ShowMessageBox(MessageBoxType.Ok, "No cards were selected. (did you forget to translate the cards?)", "Error");
                return;
            }

            int cardCount = ankiBulkImportModel.Notes.Count;
            string? cardName = ankiBulkImportModel.Notes.FirstOrDefault()?.modelName;
            string? deckName = ankiBulkImportModel.Notes.FirstOrDefault()?.deckName;
            if (_dialogService.ShowMessageBox(MessageBoxType.YesNo, $"Are you sure you want to import {cardCount} `{cardName}` cards into deck `{deckName}`?", "Please confirm") == true)
            {
                _ankiService.ImportNotesAsync(ankiBulkImportModel);
            }
        }

        private Dictionary<string, string> CreateFieldsForCardCandiate(List<AnkiCandidateMappingModel> mappings, CardCandidateViewModel cardCandidate)
        {
            Dictionary<string, string> fields = [];
            foreach(var mapping in mappings)
            {
                switch (mapping.CandidateFieldName)
                {
                    case "word":
                        fields[mapping.AnkiFieldName] = cardCandidate.Word;
                        break;
                    case "word-translation":
                        fields[mapping.AnkiFieldName] = cardCandidate.WordTranslation;
                        break;
                    case "sentence":
                        fields[mapping.AnkiFieldName] = cardCandidate.Sentence;
                        break;
                    case "sentence-translation":
                        fields[mapping.AnkiFieldName] = cardCandidate.SentenceTranslation;
                        break;
                }
            }

            return fields;
        }
    }
}
