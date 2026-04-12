namespace VocabHelper.WPF.Business.EventArgs
{
    public class CloseDialogEventArgs : System.EventArgs
    {
        public bool? DialogResult { get; set; }
        public CloseDialogEventArgs(bool? dialogResult) => DialogResult = dialogResult;
    }
}
