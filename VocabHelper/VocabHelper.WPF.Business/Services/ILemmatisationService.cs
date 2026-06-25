using VocabHelper.WPF.Business.EventArgs;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.Services
{
    public interface ILemmatisationService
    {
        event EventHandler<LemmatisationEventArgs> ItemLemmatised;

        void Lemmatise(IReadOnlyList<QueuedSentenceModel> queue, CancellationToken cancellationToken);
    }
}