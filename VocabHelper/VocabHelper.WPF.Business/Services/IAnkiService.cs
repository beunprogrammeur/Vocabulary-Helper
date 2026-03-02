
namespace VocabHelper.WPF.Business.Services
{
    public interface IAnkiService
    {
        Task<IReadOnlyCollection<object>> GetCards(string deck);
        Task<IReadOnlyCollection<string>> GetDecks();
    }
}
