namespace Ypdf.CommandLine.Informing;

internal interface IUserInteractor
{
    string? Ask(string question);
    bool AskYesNo(string question);
}
