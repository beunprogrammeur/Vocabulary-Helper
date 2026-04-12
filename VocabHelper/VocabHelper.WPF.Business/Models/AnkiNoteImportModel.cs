namespace VocabHelper.WPF.Business.Models
{
    public class AnkiNoteImportModel
    {
        public List<string> tags { get; set; }
        public string deckName { get; set; }
        public string modelName { get; set; }
        public Dictionary<string, string> fields { get; set; }
    }
}
