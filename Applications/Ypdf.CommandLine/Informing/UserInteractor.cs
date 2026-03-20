using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ypdf.CommandLine.Informing;

internal abstract class UserInteractor : IUserInteractor
{
    private readonly IReadOnlyCollection<string> _defaultYesAnswersInUppercase =
        ["YES", "Y", "True", "T", "OK", "1"];

    protected UserInteractor(
        TextWriter textWriter,
        TextReader textReader,
        IEnumerable<string>? yesAnswers = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(textWriter, nameof(textWriter));
        ExtendedArgumentNullException.ThrowIfNull(textReader, nameof(textReader));

        Writer = textWriter;
        Reader = textReader;

        YesAnswers = yesAnswers is not null
            ? [.. yesAnswers]
            : _defaultYesAnswersInUppercase;
    }

    protected TextWriter Writer { get; }
    protected TextReader Reader { get; }
    protected IReadOnlyCollection<string> YesAnswers { get; }

    public string? Ask(string? question)
    {
        Writer.Write(question);
        return Reader.ReadLine();
    }

    public bool AskYesNo(string? question)
    {
        string answer = Ask(question) ?? string.Empty;

        return YesAnswers.Any(
            t => t.Equals(answer, StringComparison.OrdinalIgnoreCase));
    }
}
