using System.Windows;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    [RegisterService]
    public partial class DialogWindow : Window, IWindow<IDialogViewModel>
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public IDialogViewModel ViewModel
        {
            get => DataContext as IDialogViewModel;
            set 
            { 
                DataContext = value; 
                value.CloseDialog -= ViewModel_CloseDialog;
                value.CloseDialog += ViewModel_CloseDialog;
            }
        }

        private void ViewModel_CloseDialog(object? sender, CloseDialogEventArgs e)
        {
            DialogResult = e.DialogResult;
            Close();
        }
    }
}
