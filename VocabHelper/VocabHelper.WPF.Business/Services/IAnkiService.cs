
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface IAnkiService
    {
        Task<IReadOnlyCollection<AnkiCard>> GetCards(string deck);
        Task<IReadOnlyCollection<string>> GetDecks();
    }
}
