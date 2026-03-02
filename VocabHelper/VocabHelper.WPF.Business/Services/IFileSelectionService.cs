namespace VocabHelper.WPF.Business.Services
{
    public interface IFileSelectionService
    {
        bool OpenFile(out string path);
    }
}
