using System.Windows;
using System.Windows.Controls;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoadEBookView.xaml
    /// </summary>
    public partial class LoadEBookView : UserControl
    {
        public LoadEBookView()
        {
            InitializeComponent();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }
        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var epubPath = files.FirstOrDefault(f => f.EndsWith(".epub", StringComparison.OrdinalIgnoreCase));

            var viewModel = DataContext as LoadEBookViewModel;


            if (epubPath == null)
            {
                //viewModel.DialogResult = false;
                return;
            }

            viewModel.FilePath = epubPath;
        }
    }
}
