using HtmlAgilityPack;
using System.Text.RegularExpressions;
using VersOne.Epub;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    internal class EBookService : IEBookService
    {
        public IEnumerable<EBookWord> GetAllWords(string path, bool removeDuplicates)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file at path '{path}' was not found.");
            }

            EpubBook book = EpubReader.ReadBook(path);
            Regex regexWord = new("[A-Za-z-]+");
            Regex regexSentence = new("([^.!?]+)[.!?]", RegexOptions.Singleline);

            List<EBookWord> words = [];

            int index = 0;
            // open the "files" in the book, where the text is
            foreach (EpubLocalTextContentFile file in book.ReadingOrder)
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(file.Content);

                // collapse newlines etc.
                string text = Regex.Replace(document.DocumentNode.InnerText, @"\s+", " ");

                // get all sentences
                foreach (Match matchSentence in regexSentence.Matches(text))
                {
                    string sentence = matchSentence.Value;

                    foreach (Match matchWord in regexWord.Matches(sentence))
                    {
                        string word = matchWord.Value;
                        words.Add(new EBookWord()
                        {
                            Word = word,
                            Sentence = sentence,
                            Index = index++
                        });
                    }
                }
            }

            if (!removeDuplicates)
            {
                return words;
            }

            return words.DistinctBy(x => x.Word);
        }
    }
}
