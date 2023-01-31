namespace YpdfDesktop.Models.Localization
{
    public interface ILocale
    {
        string Id { get; }
        string Name { get; set; }

        string? Favorites { get; set; }
        string? Tools { get; set; }
        string? Settings { get; set; }

        string? Language { get; set; }
        string? Theme { get; set; }
        string? PythonAlias { get; set; }
        string? Save { get; set; }

        string? From { get; set; }
        string? To { get; set; }
        string? Execute { get; set; }
        string? Reset { get; set; }
        string? File { get; set; }
        string? Output { get; set; }

        string? Split { get; set; }
        string? Merge { get; set; }
        string? Compress { get; set; }
        string? HandlePages { get; set; }
        string? CropPages { get; set; }
        string? DividePages { get; set; }
        string? AddPageNumbers { get; set; }
        string? AddWatermark { get; set; }
        string? RemoveWatermark { get; set; }
        string? ImageToPdf { get; set; }
        string? TextToPdf { get; set; }
        string? ExtractImages { get; set; }
        string? ExtractText { get; set; }
        string? SetPassword { get; set; }
        string? RemovePassword { get; set; }

        string? FileEmptyMessage { get; set; }
        string? FileExistsMessage { get; set; }
        string? ReplaceItMessage { get; set; }
    }
}