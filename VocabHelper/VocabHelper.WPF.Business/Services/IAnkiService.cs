
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface IAnkiService
    {
        Task<IReadOnlyCollection<string>> GetCardFields(string cardName);
        Task<IReadOnlyCollection<AnkiCard>> GetCards(string deck);
        Task<IReadOnlyCollection<string>> GetCardTypeNames();
        Task<IReadOnlyCollection<string>> GetDecks();
        Task<List<long>> ImportNotesAsync(AnkiBulkImportModel bulkImport);
    }
}
