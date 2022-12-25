using FileSystemLib.Paths;

namespace ExecutionLib.Informing.Users
{
    public interface IApplyCorrectionsUserAnswerDispatcher
    {
        UserAnswer Ask(IPathCorrection[] corrections);
    }
}