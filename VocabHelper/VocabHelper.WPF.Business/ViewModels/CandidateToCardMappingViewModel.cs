using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class CandidateToCardMappingViewModel : BaseViewModel, IDialogViewModel
    {
        private readonly IAnkiService _ankiService;
        public ObservableCollection<string> Tags { get; } = [];
        public ObservableCollection<CardCandidateMapViewModel> Mappings { get; } =  [];
        public ObservableCollection<string> CardTypes { get; set; } = [];
        public ObservableCollection<string> Decks { get; set; } = [];
        
        public ObservableCollection<string> AnkiFields { get; set; } = [];
        public IReadOnlyList<string> CandidateFields { get; set; } = [];
        public string Title { get; set; }

        [ObservableProperty] 
        private string tagCandidate;
        [NotifyCanExecuteChangedFor(nameof(AcceptMappingCommand))]
        [ObservableProperty] 
        private string selectedDeck;
        [NotifyCanExecuteChangedFor(nameof(AcceptMappingCommand))]
        [ObservableProperty] 
        private string selectedCardType;
        [ObservableProperty] 
        private string selectedTag;
        [ObservableProperty] 
        private string selectedAnkiField;
        [ObservableProperty] 
        private string selectedCandidateField;
        [ObservableProperty] 
        private CardCandidateMapViewModel selectedMapping;

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public CandidateToCardMappingViewModel(IAnkiService ankiService)
        {
            _ankiService = ankiService;
        }

        [RelayCommand]
        public async Task LoadCardTypesAsync()
        {
            Mappings.Clear();
            CardTypes.Clear();
            Decks.Clear();

            var types = await _ankiService.GetCardTypeNames();
            var decks = await _ankiService.GetDecks();
            foreach (string cardType in types)
            {
                CardTypes.Add(cardType);
            }

            foreach(string deck in decks)
            {
                Decks.Add(deck);
            }
        }

        private bool CanAcceptMapping() => !string.IsNullOrEmpty(SelectedDeck) && !string.IsNullOrEmpty(SelectedCardType);

        [RelayCommand(CanExecute = nameof(CanAcceptMapping))]
        public void AcceptMapping()
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(true));
        }

        [RelayCommand]
        public void Map()
        {
            if(!Mappings.Any(x => x.CardField == SelectedAnkiField && x.CandidateField == SelectedCandidateField))
            {
               Mappings.Add(new CardCandidateMapViewModel() 
               { 
                   CardField = SelectedAnkiField, 
                   CandidateField = SelectedCandidateField 
               });
            }
        }

        [RelayCommand]
        public void UnMap()
        {
            Mappings.Remove(SelectedMapping);
        }

        [RelayCommand]
        public void AddTag()
        {
            if(!Tags.Contains(TagCandidate))
            {
                Tags.Add(TagCandidate);
            }
        }

        [RelayCommand]
        public void RemoveTag()
        {
            Tags.Remove(SelectedTag);
        }

        partial void OnSelectedCardTypeChanged(string value)
        {
            LoadFieldsAsync(value);
        }

        private async Task LoadFieldsAsync(string value)
        {
            var fields = await _ankiService.GetCardFields(value);

            AnkiFields.Clear();
            foreach (string field in fields)
            {
                AnkiFields.Add(field);
            }
        }
    }
}
