using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    public interface IDialogViewModel
    {
        public string Title { get; set; }
        event EventHandler<CloseDialogEventArgs> CloseDialog;
    }
}
