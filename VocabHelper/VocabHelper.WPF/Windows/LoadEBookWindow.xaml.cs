using System.Windows;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DragAndDropWindow.xaml
    /// </summary>
    [RegisterService<IWindow<LoadEBookViewModel>>]
    public partial class LoadEBookWindow : Window, IWindow<LoadEBookViewModel>
    {
        public string FilePath => ViewModel.FilePath;
        public LanguageId? Language => ViewModel.ChosenLanguage;
        public bool Success => DialogResult == true;

        LoadEBookViewModel ViewModel { get => DataContext as LoadEBookViewModel; set => DataContext = value; }
        LoadEBookViewModel IWindow<LoadEBookViewModel>.ViewModel { get => ViewModel; set => ViewModel = value; }

        public LoadEBookWindow(LoadEBookViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            ViewModel.WordSelectionComplete += OnWordSelectionComplete;
        }

        private void OnWordSelectionComplete(object? sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }
        private async void Window_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var epubPath = files.FirstOrDefault(f => f.EndsWith(".epub", StringComparison.OrdinalIgnoreCase));

            if (epubPath == null)
            {
                DialogResult = false;
                return;
            }

            ViewModel.FilePath = epubPath;
        }
    }
}
