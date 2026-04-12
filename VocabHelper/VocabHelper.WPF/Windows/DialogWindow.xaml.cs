using System.Windows;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public static bool? ShowDialog(IDialogViewModel viewModel)
        {
            DialogWindow window = new()
            {
                DataContext = viewModel
            };
            viewModel.CloseDialog += window.ViewModel_CloseDialog;
            bool? result = window.ShowDialog();
            viewModel.CloseDialog -= window.ViewModel_CloseDialog;
            return result;
        }

        private void ViewModel_CloseDialog(object? sender, CloseDialogEventArgs e)
        {
            DialogResult = e.DialogResult;
            Close();
        }
    }
}
