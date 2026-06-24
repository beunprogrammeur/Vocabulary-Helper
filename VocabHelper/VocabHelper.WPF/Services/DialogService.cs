using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Factories;

namespace VocabHelper.WPF.Services
{
    [RegisterService<IDialogService>(RegistrationType.Singleton)]
    internal class DialogService : IDialogService
    {
        private readonly IWindowFactory _windowFactory;
        public DialogService(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public AnkiMappingModel? GetMapping(AnkiMappingModel previousMapping)
        {
            var window = _windowFactory.GetCandidateToCardMappingWindow();
            CandidateToCardMappingViewModel viewModel = window.ViewModel;
            viewModel.CandidateFields =
                [
                    "word",
                    "word-translation",
                    "sentence",
                    "sentence-translation"
                ];

            if (previousMapping != null)
            {
                foreach (string tag in previousMapping.Tags)
                {
                    viewModel.Tags.Add(tag);
                }

                foreach (var mapping in previousMapping.Mappings)
                {
                    viewModel.Mappings.Add(new CardCandidateMapViewModel()
                    {
                        CandidateField = mapping.CandidateFieldName,
                        CardField = mapping.AnkiFieldName
                    });
                }

                viewModel.SelectedDeck = previousMapping.DeckName;
                viewModel.SelectedCardType = previousMapping.CardName;
            }

            if (window.ShowDialog() == true)
            {
                return new AnkiMappingModel()
                {
                    Mappings = [.. viewModel.Mappings.Select(m => new AnkiCandidateMappingModel()
                    {
                        AnkiFieldName = m.CardField,
                        CandidateFieldName = m.CandidateField,
                    })],
                    Tags = viewModel.Tags.ToList(),
                    CardName = viewModel.SelectedCardType,
                    DeckName = viewModel.SelectedDeck
                };
            }

            return null;
        }

        public (bool success, string? text, LanguageId language) GetRawText()
        {
            var window = _windowFactory.GetLoadRawTextWindow();
            if (window.ShowDialog() == true)
            {
                return (true, window.ViewModel.RawText, window.ViewModel.ChosenLanguage);
            }

            return (false, null, default);
        }

        public bool? ShowMessageBox(MessageBoxType type, string message, string title = "")
        {
            var window = _windowFactory.GetMessageBoxWindow(type, message, title);
            if (window.ShowDialog() == true)
            {
                return window.ViewModel.ChosenButton switch
                {
                    MessageBoxButton.Ok => true,
                    MessageBoxButton.Yes => true,
                    MessageBoxButton.No => false,
                    _ => null
                };
            }
            return null;
        }
    }
}
