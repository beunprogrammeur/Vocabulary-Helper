using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Models;
using VocabHelper.WPF.Business.ViewModels.Persistence;

namespace VocabHelper.WPF.Business.Services
{
    [RegisterService<ILemmatisationService>]
    public class LemmatisationService : ILemmatisationService
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly string _endpointURL;
        private readonly string _systemPrompt;
        private readonly string _sourceLanguage;
        private readonly string _targetLanguage;

        public event EventHandler<LemmatisationEventArgs> ItemLemmatised;

        public LemmatisationService(PersistentSettings settings)
        {
            _apiSettings = settings.ApiSettings;

            _httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
            _endpointURL = _apiSettings.ApiEndpoint;
            _systemPrompt = _apiSettings.SystemPrompt;
            _sourceLanguage = settings.LanguageSettings.SourceLanguage;
            _targetLanguage = settings.LanguageSettings.TargetLanguage;
        }

        public void Lemmatise(IReadOnlyList<QueuedSentenceModel> queue, CancellationToken cancellationToken)
        {
            // Start het werk onmiddellijk op de achtergrond zonder de UI te blokkeren
            Task.Run(() => DoWorkAsync(queue, cancellationToken), cancellationToken);
        }

        private async void DoWorkAsync(IReadOnlyList<QueuedSentenceModel> queue, CancellationToken cancellationToken)
        {
            foreach (var currentItem in queue)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(currentItem.Text))
                {
                    continue;
                }

                try
                {
                    // Vraag Aya Expanse om de linguïstische analyse
                    var resultModel = await CallLlamaApiAsync(currentItem.Text, cancellationToken);

                    if (resultModel != null)
                    {
                        // Vul de originele bron-zin in voor de administratie
                        resultModel.SourceSentence = currentItem.Text;

                        // Vuur het event af! De luisteraar krijgt het resultaat direct mee
                        OnItemLemmatised(currentItem, resultModel);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Lemmatisation Error]: {ex.Message}");
                    // TODO: ILOGGER
                }
            }

        }
        private async Task<LemmatisationResultModel?> CallLlamaApiAsync(string sentence, CancellationToken cancellationToken)
        {
            var userPayload = new
            {
                source_language = _sourceLanguage,
                target_language = _targetLanguage,
                sentence = sentence
            };

            var requestBody = new
            {
                response_format = new { type = "json_object" },
                messages = new object[]
                {
                new { role = "system", content = _systemPrompt },
                new { role = "user", content = JsonSerializer.Serialize(userPayload) }
                },
                extra_body = new { cache_prompt = true }
            };

            var response = await _httpClient.PostAsJsonAsync(_endpointURL, requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
            string? rawJsonContent = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();


            if (string.IsNullOrWhiteSpace(rawJsonContent)) return null;
            string sanitisedJson = SanitiseMarkdownFromJson(rawJsonContent);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("---");
            builder.AppendLine();

            builder.AppendLine(sanitisedJson);

            builder.AppendLine();
            builder.AppendLine("---");
            builder.AppendLine();

            using FileStream fs = new FileStream("log.log", FileMode.Append);
            using (var writer = new StreamWriter(fs))
            {
                writer.WriteLine(sanitisedJson);
            }

            using var innerDoc = JsonDocument.Parse(sanitisedJson);
            var result = new LemmatisationResultModel
            {
                TargetSentence = innerDoc.RootElement.GetProperty("translated_sentence").GetString() ?? string.Empty
            };

            var wordsArray = innerDoc.RootElement.GetProperty("words");
            foreach (var wordElement in wordsArray.EnumerateArray())
            {
                string variant = wordElement.GetProperty("text_in_sentence").GetString() ?? string.Empty;
                string root = wordElement.GetProperty("root_word").GetString() ?? string.Empty;
                string trans = wordElement.GetProperty("translation").GetString() ?? string.Empty;

                result.LemmatisedWords.Add(new LemmatisedWord(root, trans, variant));
            }

            return result;
        }

        private string SanitiseMarkdownFromJson(string json)
        {
            var stringBuilder = new System.Text.StringBuilder();
            using (var reader = new System.IO.StringReader(json))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimmedLine = line.Trim();

                    if (trimmedLine.StartsWith("```"))
                    {
                        continue;
                    }

                    stringBuilder.AppendLine(line.Replace("<|END_OF_TURN_TOKEN|>", ""));
                }
            }

            return stringBuilder.ToString().Trim();
        }

        protected virtual void OnItemLemmatised(QueuedSentenceModel queuedSentence, LemmatisationResultModel lemmatisationResult)
        {
            ItemLemmatised?.Invoke(this, new LemmatisationEventArgs(queuedSentence, lemmatisationResult));
        }
    }
}
