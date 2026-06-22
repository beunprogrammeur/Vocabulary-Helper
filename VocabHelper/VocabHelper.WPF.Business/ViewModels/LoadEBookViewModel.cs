using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class LoadEBookViewModel : BaseViewModel, IDialogViewModel
    {
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

        public string Title { get; set; } = "Load EBook";

        public event EventHandler? WordSelectionComplete;
        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public LoadEBookViewModel()
        {
            FilePath = string.Empty;
            Languages = Enum.GetValues<LanguageId>();
        }

        [RelayCommand(CanExecute = nameof(CanExecuteContinue))]
        private void Continue()
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(true));
        }

        private bool CanExecuteContinue()
            => File.Exists(FilePath) && ChosenLanguage != null;
    }
}
