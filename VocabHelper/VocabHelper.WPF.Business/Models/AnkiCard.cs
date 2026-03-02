using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabHelper.WPF.Business.Models
{
    public class AnkiField
    {
        public string Value { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class AnkiCard
    {
        public long CardId { get; set; }
        public long NoteId { get; set; }
        public string DeckName { get; set; } = string.Empty;

        public Dictionary<string, AnkiField> Fields { get; set; } = new();
        public List<string> Tags { get; set; } = new();

        public string? SearchFieldName =>
            Fields.FirstOrDefault(f => f.Value.Order == 0).Key;

        public string? SearchFieldValue =>
            Fields.FirstOrDefault(f => f.Value.Order == 0).Value.Value;
    }

}
