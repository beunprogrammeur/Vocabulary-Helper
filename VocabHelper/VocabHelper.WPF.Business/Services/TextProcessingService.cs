using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using VersOne.Epub;
using VocabHelper.Core;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    [RegisterService<ITextProcessingService>(RegistrationType.Singleton)]
    internal class TextProcessingService : ITextProcessingService
    {
        private readonly IStemmerServiceFactory _stemmerServiceFactory;
        public TextProcessingService(IStemmerServiceFactory stemmerServiceFactory)
        {
            _stemmerServiceFactory = stemmerServiceFactory;
        }

        public string ReadEpub(string epub)
        {
            StringBuilder builder = new ();
            EpubBook book = EpubReader.ReadBook(epub);

            foreach(EpubLocalTextContentFile file in book.ReadingOrder)
            {
                HtmlDocument document = new();
                document.LoadHtml(file.Content);
                builder.Append(document.DocumentNode.InnerText);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public VocabularyRepositoryModel ProcessText(string text, LanguageId language, VocabularyRepositoryModel? repository = null)
        {
            if (repository == null)
            {
                repository = new VocabularyRepositoryModel();
            }

            var words = GetWordsFromText(text);
            var stemmedWords = GetStemmedWords(words, language);

            foreach (var word in stemmedWords)
            {
                if (repository.Words.TryGetValue(word.Word, out WordEntryModel existingEntry))
                {
                    existingEntry.Count += word.Count;
                    existingEntry.Variations.UnionWith(word.Variations);
                }
                else
                {
                    repository.Words.Add(word.Word, word);
                }
            }

            repository.Language = language;
            return repository;
        }

        private IReadOnlyList<WordEntryModel> GetWordsFromText(string text)
        {
            List<WordEntryModel> words = [];

            Regex regexWord = new("[A-Za-z-]+", RegexOptions.Compiled);
            Regex regexSentence = new("([^.!?]+)[.!?]", RegexOptions.Singleline | RegexOptions.Compiled);

            int index = 0;

            string normalized = Regex.Replace(text, @"\s+", " ");

            foreach (Match matchSentence in regexSentence.Matches(normalized))
            {
                string sentence = matchSentence.Value;

                foreach (Match matchWord in regexWord.Matches(sentence))
                {
                    string word = matchWord.Value;

                    words.Add(new WordEntryModel(word, sentence, index++));
                }
            }

            return words;
        }

        private IReadOnlyList<WordEntryModel> GetStemmedWords(IReadOnlyList<WordEntryModel> words, LanguageId language)
        {
            Dictionary<string, WordEntryModel> stemmedWords = [];

            var stemmer = _stemmerServiceFactory.GetStemmer(language);

            foreach (var word in words)
            {
                string stemmedWord = stemmer.Stem(word.Word, word.Sentence).ToLower();
                string lowercaseWord = word.Word.ToLower();
                if (stemmedWords.TryGetValue(stemmedWord, out WordEntryModel entry))
                {
                    entry.Count++;
                    if (!entry.Variations.Contains(lowercaseWord))
                    {
                        entry.Variations.Add(lowercaseWord);
                    }
                }
                else
                {
                    stemmedWords.Add(stemmedWord, new WordEntryModel(stemmedWord, word.Sentence, word.Index)
                    {
                        Count = 1,
                        Variations = [lowercaseWord]
                    });
                }
            }

            return stemmedWords.Values.ToList();
        }
    }
}
