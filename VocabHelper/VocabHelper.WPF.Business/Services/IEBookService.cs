using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface IEBookService
    {
        IEnumerable<EBookWord> GetAllWords(string path, bool removeDuplicates);
    }
}
