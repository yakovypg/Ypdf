namespace ExecutionLib.Configuration.Models
{
    public class NotConfiguredException : ApplicationException
    {
        public NotConfiguredException(string? name, string? message = null, Exception? innerException = null)
            : base(message ?? GetMessage(name), innerException)
        {
        }

        private static string GetMessage(string? name)
        {
            return string.IsNullOrEmpty(name)
                ? "not configured."
                : $"{name} not configured.";
        }
    }
}
