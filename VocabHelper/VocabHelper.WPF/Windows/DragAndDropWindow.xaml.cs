using System.Windows;

namespace VocabHelper.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DragAndDropWindow.xaml
    /// </summary>
    public partial class DragAndDropWindow : Window
    {
        public string FilePath { get; private set; }
        public DragAndDropWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Please drop an EPUB file.");
                return;
            }

            FilePath = epubPath;
            DialogResult = true;
            Close();
        }
    }
}
