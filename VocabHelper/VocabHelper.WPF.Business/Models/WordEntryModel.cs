namespace VocabHelper.WPF.Business.Models
{
    public class WordEntryModel(string word, string sentence, int index)
    {
        public string Word { get; set; } = word;
        public string Sentence { get; set; } = sentence;
        public int Index { get; set; } = index;

        public int Count { get; set; } = 0;
        public HashSet<string> Variations { get; set; } = null;
    }
}
