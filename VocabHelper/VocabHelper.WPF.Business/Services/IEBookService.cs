using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface IEBookService
    {
        IEnumerable<EBookWordModel> GetAllWords(string path, bool removeDuplicates);
        IEnumerable<EBookWordModel> GetAllWordsFromText(string text, bool removeDuplicates);
    }
}
