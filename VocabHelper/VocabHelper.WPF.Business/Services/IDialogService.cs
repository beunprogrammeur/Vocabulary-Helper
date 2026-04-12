using VocabHelper.Core;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Business.Services
{
    public interface IDialogService
    {
        AnkiMappingModel GetMapping(AnkiMappingModel previousMapping);
        (bool success, string? text, LanguageId language) GetRawText();
        public (bool Success, LanguageId? Language, string? File) OpenFile();
        bool? ShowMessageBox(MessageBoxType type, string message, string title = "");
    }
}
