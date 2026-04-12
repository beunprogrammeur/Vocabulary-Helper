using Microsoft.Extensions.DependencyInjection;
using VocabHelper.Core;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF.Services
{
    internal class DialogService : IDialogService
    {

        public (bool Success, LanguageId? Language, string? File) OpenFile()
        {
            LoadEBookWindow window = new();
            if (window.ShowDialog() == true)
            {
                return (true, window.Language.Value, window.FilePath);
            }

            return (false, null, null);
        }

        public AnkiMappingModel? GetMapping(AnkiMappingModel previousMapping)
        {

            CandidateToCardMappingViewModel viewModel = App.Services.GetRequiredService<CandidateToCardMappingViewModel>();
            viewModel.CandidateFields = [
                "word",
                "word-translation",
                "sentence",
                "sentence-translation"
            ];

            if(previousMapping != null)
            {
                foreach(string tag in previousMapping.Tags)
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

            if (DialogWindow.ShowDialog(viewModel) == true)
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
            LoadRawTextViewModel loadRawTextViewModel = new LoadRawTextViewModel();
            if (DialogWindow.ShowDialog(loadRawTextViewModel) == true)
            {
                return (true, loadRawTextViewModel.RawText, loadRawTextViewModel.ChosenLanguage);
            }

            return (false, null, default);
        }

        public bool? ShowMessageBox(MessageBoxType type, string message, string title = "")
        {
            MessageBoxViewModel viewModel = new MessageBoxViewModel(type, message, title);
            if (DialogWindow.ShowDialog(viewModel) == true)
            {
                return viewModel.ChosenButton switch
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
