using VocabHelper.Core;

namespace VocabHelper.WPF.Business.Models
{
    public class VocabularyRepositoryModel
    {
        public LanguageId Language { get; set; }
        public Dictionary<string, WordEntryModel> Words { get; } = [];
    }
}
