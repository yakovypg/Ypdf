using System;

namespace YpdfDesktop.Models.Localization
{
    public class Locale : ILocale
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string? Favorites { get; set; }
        public string? Tools { get; set; }
        public string? Settings { get; set; }
        public string? Tasks { get; set; }

        public string? Language { get; set; }
        public string? Theme { get; set; }
        public string? PythonAlias { get; set; }
        public string? Save { get; set; }
        public string? ResetAfterExecution { get; set; }

        public string? Running { get; set; }
        public string? Completed { get; set; }
        public string? Faulted { get; set; }

        public string? From { get; set; }
        public string? To { get; set; }
        public string? Execute { get; set; }
        public string? Reset { get; set; }
        public string? File { get; set; }
        public string? Output { get; set; }

        public string? Split { get; set; }
        public string? Merge { get; set; }
        public string? Compress { get; set; }
        public string? HandlePages { get; set; }
        public string? CropPages { get; set; }
        public string? DividePages { get; set; }
        public string? AddPageNumbers { get; set; }
        public string? AddWatermark { get; set; }
        public string? RemoveWatermark { get; set; }
        public string? ImageToPdf { get; set; }
        public string? TextToPdf { get; set; }
        public string? ExtractImages { get; set; }
        public string? ExtractText { get; set; }
        public string? SetPassword { get; set; }
        public string? RemovePassword { get; set; }

        public string? Yes { get; set; }
        public string? No { get; set; }

        public string? FileEmptyMessage { get; set; }
        public string? FileExistsMessage { get; set; }
        public string? ReplaceItMessage { get; set; }
        public string? UnfinishedTasksMessage { get; set; }
        public string? ExitWithoutWaitingForCompletionMessage { get; set; }
        public string? SpecifyOutputFilePathMessage { get; set; }

        public Locale() : this(Guid.NewGuid().ToString())
        {
        }

        public Locale(string id) : this(id, string.Empty)
        {
        }

        public Locale(string id, string name)
        {
            Id = id;
            Name = name;

            SetLocale();
        }

        protected virtual void SetLocale()
        {
        }
    }
}
