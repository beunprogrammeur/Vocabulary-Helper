using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class LoadTextViewModel : BaseViewModel, IDialogViewModel
    {
        private readonly ITextProcessingService _textProcessingService;
        private readonly StatusViewModel _statusViewModel;

        [ObservableProperty] private string rawText;
        [ObservableProperty] private LanguageId chosenLanguage;
        public LanguageId[] Languages => Enum.GetValues<LanguageId>();

        public string Title { get; set; }

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public LoadTextViewModel(ITextProcessingService textProcessingService, StatusViewModel statusViewModel)
        {
            _textProcessingService = textProcessingService;
            _statusViewModel = statusViewModel; // TODO: should I do this here? or do they both share the same service?
        }


        [RelayCommand]
        public void Accept()
        {
            var repository = _textProcessingService.ProcessText(RawText, ChosenLanguage, _statusViewModel.WordRepository);
            _statusViewModel.WordRepository = repository;
            _statusViewModel.UpdateWordRepository();

            CloseDialog?.Invoke(this, new CloseDialogEventArgs(true));
        }

        [RelayCommand]
        public void FileDropped(string[] files)
        {
            string? epub = files.FirstOrDefault(x => x.EndsWith(".epub", StringComparison.OrdinalIgnoreCase));
            if(!string.IsNullOrEmpty(epub) && File.Exists(epub))
            {
                RawText = _textProcessingService.ReadEpub(epub);
            }
        }
    }
}
