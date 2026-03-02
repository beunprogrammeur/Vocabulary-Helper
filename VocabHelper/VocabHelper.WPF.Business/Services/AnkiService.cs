using System.Text;
using System.Text.Json;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.Services;

internal class AnkiService : IAnkiService
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:8765")
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };


    private async Task<T> SendRequestAsync<T>(string action, object? parameters = null)
    {
        var payload = new Dictionary<string, object?>
        {
            ["action"] = action,
            ["version"] = 6
        };

        if (parameters != null)
        {
            payload["params"] = parameters;
        }

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/", content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"AnkiConnect request failed: {response.StatusCode}");

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        var error = root.GetProperty("error").GetString();
        if (error != null)
            throw new Exception($"AnkiConnect error: {error}");

        var resultJson = root.GetProperty("result").GetRawText();
        return JsonSerializer.Deserialize<T>(resultJson, JsonOptions)!;
    }

    public Task<IReadOnlyCollection<string>> GetDecks()
        => SendRequestAsync<IReadOnlyCollection<string>>("deckNames");

    public Task<IReadOnlyCollection<AnkiCard>> GetCards(string deck) => GetCardIds(deck)
            .ContinueWith(idsTask =>
            {
                var ids = idsTask.Result;
                if (ids.Count == 0) return Task.FromResult<IReadOnlyCollection<AnkiCard>>(Array.Empty<AnkiCard>());
                return GetCardDetails(ids);
            })
            .Unwrap();
    private Task<IReadOnlyCollection<long>> GetCardIds(string deck)
        => SendRequestAsync<IReadOnlyCollection<long>>("findCards", new { query = $"deck:\"{deck}\"" });

    private Task<IReadOnlyCollection<AnkiCard>> GetCardDetails(IEnumerable<long> ids)
        => SendRequestAsync<IReadOnlyCollection<AnkiCard>>("cardsInfo", new { cards = ids.ToArray() });
}
