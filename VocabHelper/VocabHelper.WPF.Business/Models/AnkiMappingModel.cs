namespace VocabHelper.WPF.Business.Models
{
    public class AnkiMappingModel
    {
        public List<AnkiCandidateMappingModel> Mappings { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public string DeckName { get; set; }
        public string CardName { get; set; }
    }
}
