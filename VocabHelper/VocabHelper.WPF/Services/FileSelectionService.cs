using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF.Services
{
    internal class FileSelectionService : IFileSelectionService
    {
        public bool OpenFile(out string path)
        {
            DragAndDropWindow window = new();
            if (window.ShowDialog() == true)
            {
                path = window.FilePath;
                return true;
            }

            path = string.Empty;
            return false;
        }
    }
}
