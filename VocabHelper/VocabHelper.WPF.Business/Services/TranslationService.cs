using System.Text.Json;
using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Services
{
    internal class TranslationService : ITranslationService
    {
        private static readonly HttpClient _httpClient = new();

        // Internal cache: original text → translated text
        private readonly Dictionary<string, string> _cache = [];

        // Limit concurrency to avoid Google throttling
        private readonly SemaphoreSlim _limiter = new(3);

        public async Task<string> TranslateAsync(string text, LanguageId sourceLanguage, LanguageId targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Check cache first
            if (_cache.TryGetValue(text, out var cached))
                return cached;

            var langIn = ConvertLanguageIdToGoogleTranslateCode(sourceLanguage);
            var langOut = ConvertLanguageIdToGoogleTranslateCode(targetLanguage);

            var url =
                "https://translate.googleapis.com/translate_a/single" +
                $"?client=gtx&sl={langIn}&tl={langOut}&dt=t&q={Uri.EscapeDataString(text)}";

            await _limiter.WaitAsync();
            try
            {
                var json = await _httpClient.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                var translated = doc.RootElement[0][0][0].GetString() ?? string.Empty;

                _cache[text] = translated;
                return translated;
            }
            finally
            {
                _limiter.Release();
            }
        }

        private string ConvertLanguageIdToGoogleTranslateCode(LanguageId languageId)
        {
            return languageId switch
            {
                LanguageId.English => "en",
                LanguageId.Korean => "ko",   // corrected: Google uses "ko", not "kr"
                LanguageId.Indonesian => "id",
                _ => throw new NotSupportedException($"Language {languageId} is not supported.")
            };
        }
    }
}
