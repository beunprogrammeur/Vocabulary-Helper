namespace VocabHelper.WPF.Business.Models
{
    public record LemmatisedWord(string RootWord, string Translation, string Variant);
    public class LemmatisationResultModel
    {
        public string SourceSentence { get; set; }
        public string TargetSentence { get; set; }
        public List<LemmatisedWord> LemmatisedWords { get; set; } = [];
    }
}
