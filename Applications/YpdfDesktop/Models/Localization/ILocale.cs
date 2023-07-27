namespace YpdfDesktop.Models.Localization
{
    public interface ILocale
    {
        string Id { get; }
        string Name { get; set; }

        string? Favorites { get; set; }
        string? Tools { get; set; }
        string? Settings { get; set; }
        string? Tasks { get; set; }

        string? Language { get; set; }
        string? Theme { get; set; }
        string? PythonAlias { get; set; }
        string? Save { get; set; }
        string? ResetAfterExecution { get; set; }

        string? Running { get; set; }
        string? Completed { get; set; }
        string? Faulted { get; set; }

        string? From { get; set; }
        string? To { get; set; }
        string? Execute { get; set; }
        string? Reset { get; set; }
        string? File { get; set; }
        string? Output { get; set; }
        string? QualityFactor { get; set; }
        string? SizeFactor { get; set; }
        string? Extension { get; set; }
        string? CheckCompressionValidity { get; set; }
        string? UseTika { get; set; }
        string? Password { get; set; }
        string? OwnerPassword { get; set; }
        string? UserPassword { get; set; }
        string? OwnerPasswordHint { get; set; }
        string? UserPasswordHint { get; set; }
        string? EncryptionAlgorithm { get; set; }
        string? ShowPassword { get; set; }
        string? TextAlignment { get; set; }
        string? FontPath { get; set; }
        string? FontSize { get; set; }
        string? FontFamily { get; set; }
        string? FontColor { get; set; }
        string? FontOpacity { get; set; }
        string? FontEncoding { get; set; }
        string? PageSize { get; set; }
        string? PageWidth { get; set; }
        string? PageHeight { get; set; }
        string? Margin { get; set; }
        string? Alignment { get; set; }
        string? AutoincreaseSize { get; set; }
        string? HorizontalAlignment { get; set; }
        string? VerticalAlignment { get; set; }
        string? TextPresenter { get; set; }
        string? ConsiderLeftPageMargin { get; set; }
        string? ConsiderTopPageMargin { get; set; }
        string? ConsiderRightPageMargin { get; set; }
        string? ConsiderBottomPageMargin { get; set; }
        string? ContentShift { get; set; }
        string? FillColor { get; set; }
        string? NumberLocationMode { get; set; }
        string? PageIncrease { get; set; }
        string? IncreasePage { get; set; }
        string? Orientation { get; set; }
        string? ApplyTo { get; set; }
        string? Pages { get; set; }

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

        string? Ok { get; set; }
        string? Yes { get; set; }
        string? No { get; set; }
        string? Custom { get; set; }
        string? Auto { get; set; }

        string? FileEmptyMessage { get; set; }
        string? FileExistsMessage { get; set; }
        string? FileNotPdfMessage { get; set; }
        string? FileNotTxtMessage { get; set; }
        string? FailedToLoadFileMessage { get; set; }
        string? ReplaceItMessage { get; set; }
        string? UnfinishedTasksMessage { get; set; }
        string? ExitWithoutWaitingForCompletionMessage { get; set; }
        string? SpecifyOutputFilePathMessage { get; set; }
        string? SpecifyCorrectQualityFactorMessage { get; set; }
        string? SpecifyCorrectSizeFactorMessage { get; set; }
        string? SpecifyAtLeastOnePasswordMessage { get; set; }
        string? SpecifyPasswordMessage { get; set; }
        string? SpecifyAtLeastOneDivisionMessage { get; set; }
        string? SpecifyAtLeastOneCroppingMessage { get; set; }
        string? IncorrectShiftContentPagesMessage { get; set; }
        string? IncorrectPagesMessage { get; set; }
        string? IncorrectPageRangeMessage { get; set; }
    }
}