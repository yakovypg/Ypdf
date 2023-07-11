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
        public string? QualityFactor { get; set; }
        public string? SizeFactor { get; set; }
        public string? Extension { get; set; }
        public string? CheckCompressionValidity { get; set; }
        public string? UseTika { get; set; }
        public string? Password { get; set; }
        public string? OwnerPassword { get; set; }
        public string? UserPassword { get; set; }
        public string? OwnerPasswordHint { get; set; }
        public string? UserPasswordHint { get; set; }
        public string? EncryptionAlgorithm { get; set; }
        public string? ShowPassword { get; set; }
        public string? TextAlignment { get; set; }
        public string? FontPath { get; set; }
        public string? FontSize { get; set; }
        public string? FontFamily { get; set; }
        public string? FontColor { get; set; }
        public string? FontOpacity { get; set; }
        public string? FontEncoding { get; set; }
        public string? PageSize { get; set; }
        public string? PageWidth { get; set; }
        public string? PageHeight { get; set; }
        public string? Margin { get; set; }
        public string? Alignment { get; set; }
        public string? AutoincreaseSize { get; set; }
        public string? HorizontalAlignment { get; set; }
        public string? VerticalAlignment { get; set; }
        public string? TextPresenter { get; set; }
        public string? ConsiderLeftPageMargin { get; set; }
        public string? ConsiderTopPageMargin { get; set; }
        public string? ConsiderRightPageMargin { get; set; }
        public string? ConsiderBottomPageMargin { get; set; }
        public string? ContentShift { get; set; }
        public string? FillColor { get; set; }
        public string? NumberLocationMode { get; set; }
        public string? PageIncrease { get; set; }
        public string? IncreasePage { get; set; }
        public string? Orientation { get; set; }
        public string? ApplyTo { get; set; }
        public string? Pages { get; set; }

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

        public string? Ok { get; set; }
        public string? Yes { get; set; }
        public string? No { get; set; }
        public string? Custom { get; set; }
        public string? Auto { get; set; }

        public string? FileEmptyMessage { get; set; }
        public string? FileExistsMessage { get; set; }
        public string? FileNotPdfMessage { get; set; }
        public string? FileNotTxtMessage { get; set; }
        public string? FailedToLoadFileMessage { get; set; }
        public string? ReplaceItMessage { get; set; }
        public string? UnfinishedTasksMessage { get; set; }
        public string? ExitWithoutWaitingForCompletionMessage { get; set; }
        public string? SpecifyOutputFilePathMessage { get; set; }
        public string? SpecifyCorrectQualityFactorMessage { get; set; }
        public string? SpecifyCorrectSizeFactorMessage { get; set; }
        public string? SpecifyAtLeastOnePasswordMessage { get; set; }
        public string? SpecifyPasswordMessage { get; set; }
        public string? SpecifyAtLeastOneDivisionMessage { get; set; }
        public string? IncorrectShiftContentPagesMessage { get; set; }
        public string? IncorrectPagesMessage { get; set; }
        public string? IncorrectPageRangeMessage { get; set; }

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
