using VocabHelper.Core;
using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF.Services
{
    internal class FileSelectionService : IFileSelectionService
    {
        public (bool Success, LanguageId? Language, string? File) OpenFile()
        {
            LoadEBookWindow window = new();
            if (window.ShowDialog() == true)
            {
                return (true, window.Language.Value, window.FilePath);
            }

            return (false, null, null);
        }
    }
}
