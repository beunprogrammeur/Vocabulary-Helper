using System.Windows;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Windows
{
    internal class DialogWindowAdapter<T> : IWindow<T> where T : class, IDialogViewModel
    {
        public T ViewModel { get => Window.DataContext as T; set => Window.DataContext = value; }
        public Window Window { get; }

        public DialogWindowAdapter(DialogWindow window, T viewModel)
        {
            Window = window;
            ViewModel = viewModel;
        }

        public bool? ShowDialog() => Window.ShowDialog();
    }
}
