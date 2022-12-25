namespace RuntimeLib.Processes
{
    public interface IProcessController
    {
        bool RedirectStandardError { get; set; }
        bool RedirectStandardOutput { get; set; }

        TextWriter? Output { get; set; }

        Func<string?, string?>? ErrorDataConverter { get; set; }
        Func<string?, string?>? OutputDataConverter { get; set; }

        Predicate<string?>? ErrorDataVerifier { get; set; }
        Predicate<string?>? OutputDataVerifier { get; set; }
    }
}
