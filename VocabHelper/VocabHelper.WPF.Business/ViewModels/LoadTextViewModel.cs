using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Business.ViewModels.Persistence;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class LoadTextViewModel : BaseViewModel, IDialogViewModel
    {
        private readonly ITextProcessingService _textProcessingService;
        private readonly StatusViewModel _statusViewModel;
        private readonly EBookViewModel _ebookViewModel;

        [ObservableProperty] private string rawText;
        [ObservableProperty] private Language chosenLanguage;

        public PersistentSettings AppSettings { get; }

        public Language[] Languages => Enum.GetValues<Language>();

        public string Title { get; set; }

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public LoadTextViewModel(ITextProcessingService textProcessingService, StatusViewModel statusViewModel, PersistentSettings appSettings, EBookViewModel ebookViewModel)
        {
            _textProcessingService = textProcessingService;
            _statusViewModel = statusViewModel; // TODO: should I do this here? or do they both share the same service?
            AppSettings = appSettings;
            _ebookViewModel = ebookViewModel;
        }


        [RelayCommand]
        public void Accept()
        {
            foreach (string sentence in _textProcessingService.GetSentences(RawText))
            {
                _ebookViewModel.QueuedSentences.Add(new QueuedSentenceModel { Text = sentence });
            }
        }

        [RelayCommand]
        public void FileDropped(string[] files)
        {
            string? epub = files.FirstOrDefault(x => x.EndsWith(".epub", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(epub) && File.Exists(epub))
            {
                RawText = _textProcessingService.ReadEpub(epub);
            }
        }
    }
}
