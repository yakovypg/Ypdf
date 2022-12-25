namespace ExecutionLib.Informing.Users
{
    public interface IFileExistsUserAnswerDispatcher
    {
        UserAnswer Ask(string filePath);
    }
}