using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.EventArgs
{
    public class LemmatisationEventArgs
    {
        public LemmatisationEventArgs(QueuedSentenceModel queuedSentence, LemmatisationResultModel lemmatisationResult)
        {
            QueuedSentence = queuedSentence;
            LemmatisationResult = lemmatisationResult;
        }

        public QueuedSentenceModel QueuedSentence { get; }
        public LemmatisationResultModel LemmatisationResult { get; }
    }
}
