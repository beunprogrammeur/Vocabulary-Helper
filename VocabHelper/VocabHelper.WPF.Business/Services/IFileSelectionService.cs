using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Services
{
    public interface IFileSelectionService
    {
        public (bool Success, LanguageId? Language, string? File) OpenFile();
    }
}
