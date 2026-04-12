using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VocabHelper.Core;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    public partial class LoadRawTextViewModel : BaseViewModel, IDialogViewModel
    {
        [ObservableProperty] private string rawText;
        [ObservableProperty] private LanguageId chosenLanguage;
        public LanguageId[] Languages => Enum.GetValues<LanguageId>();

        public string Title { get; set; }

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        [RelayCommand]
        public void Accept()
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(true));
        }
    }
}
