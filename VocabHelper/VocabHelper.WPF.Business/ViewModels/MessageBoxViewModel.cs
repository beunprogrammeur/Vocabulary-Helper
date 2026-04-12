using CommunityToolkit.Mvvm.Input;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.ViewModels
{
    public enum MessageBoxType
    {
        Ok,
        YesNo
    }

    public enum MessageBoxButton
    {
        Unknown,
        Ok,
        Yes,
        No
    }

    public partial class MessageBoxViewModel : BaseViewModel, IDialogViewModel
    {
        public MessageBoxType MessageBoxType { get; set; }
        public MessageBoxButton ChosenButton { get; set; }
        public bool ButtonOkVisible { get; set; }
        public bool ButtonYesVisible { get; set; }
        public bool ButtonNoVisible { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public event EventHandler<CloseDialogEventArgs> CloseDialog;

        public MessageBoxViewModel(MessageBoxType type, string message, string title = "")
        {
            MessageBoxType = type;
            Message = message;
            Title = title;

            switch (type)
            {
                default:
                case MessageBoxType.Ok:
                    ButtonOkVisible = true;
                    break;
                case MessageBoxType.YesNo:
                    ButtonYesVisible = true;
                    ButtonNoVisible = true;
                    break;  
            }
        }

        [RelayCommand]
        public void ButtonYes()
        {
            ChosenButton = MessageBoxButton.Yes;
            Close(true);
        }

        [RelayCommand]
        public void ButtonNo()
        {
            ChosenButton = MessageBoxButton.No;
            Close(false);
        }

        [RelayCommand]
        public void ButtonOk()
        {
            ChosenButton = MessageBoxButton.Ok;
            Close(true);
        }

        private void Close(bool? state)
        {
            CloseDialog?.Invoke(this, new CloseDialogEventArgs(state));
        }
    }
}
