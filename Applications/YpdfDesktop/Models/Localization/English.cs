﻿namespace YpdfDesktop.Models.Localization
{
    public class English : Locale
    {
        public English() : base("en-us", "English")
        {
        }

        protected override void SetLocale()
        {
            Favorites = "Favorites";
            Tools = "Tools";
            Settings = "Settings";
            Tasks = "Tasks";

            Language = "Language";
            Theme = "Theme";
            PythonAlias = "Python path/alias";
            Save = "Save";
            ResetAfterExecution = "Reset after execution";

            Running = "Running";
            Completed = "Completed";
            Faulted = "Faulted";

            From = "From";
            To = "To";
            Execute = "Execute";
            Reset = "Reset";
            File = "File";
            Output = "Output";
            QualityFactor = "Quality factor";
            SizeFactor = "Size factor";
            Extension = "Extension";
            CheckCompressionValidity = "Check compression validity";
            UseTika = "Use tika";
            Password = "Password";
            OwnerPassword = "Owner password";
            UserPassword = "User password";
            OwnerPasswordHint = "Password for editing the PDF document";
            UserPasswordHint = "Password for viewing the PDF document";
            EncryptionAlgorithm = "Encryption algorithm";
            ShowPassword = "Show password";
            TextAlignment = "Text alignment";
            FontPath = "Font path";
            FontSize = "Font size";
            FontFamily = "Font family";
            FontColor = "Font color";
            FontOpacity = "Font opacity";
            FontEncoding = "Font encoding";
            PageSize = "Page size";
            PageWidth = "Page width";
            PageHeight = "Page height";
            Margin = "Margin";
            Alignment = "Alignment";
            AutoincreaseSize = "Autoincrease size";
            HorizontalAlignment = "Horizontal alignment";
            VerticalAlignment = "Vertical alignment";
            TextPresenter = "Text presenter";
            ConsiderLeftPageMargin = "Consider left page margin";
            ConsiderTopPageMargin = "Consider top page margin";
            ConsiderRightPageMargin = "Consider right page margin";
            ConsiderBottomPageMargin = "Consider bottom page margin";
            ContentShift = "Content shift";
            FillColor = "Fill color";
            NumberLocationMode = "Number location mode";
            PageIncrease = "Page increase";
            IncreasePage = "Increase page";
            Orientation = "Orientation";
            ApplyTo = "Apply to";
            Pages = "Pages";
            ReorderPages = "Reorder pages";
            TurnPages = "Turn pages";
            RemovePages = "Remove pages";
            Watermark = "Watermark";
            Angle = "Angle";
            MakeAsAnnotation = "Make as annotation";
            ShowBoundedRectangle = "Show bounded rectangle";
            ShowSourceTextBounds = "Show source text bounds";
            ShowRotatedTextBounds = "Show rotated text bounds";
            Remarks = "Remarks";

            Split = "split";
            Merge = "merge";
            Compress = "compress";
            HandlePages = "handle pages";
            CropPages = "crop pages";
            DividePages = "divide pages";
            AddPageNumbers = "add page nums";
            AddWatermark = "add watermark";
            RemoveWatermark = "rm watermark";
            ImageToPdf = "image to pdf";
            TextToPdf = "text to pdf";
            ExtractImages = "extract images";
            ExtractText = "extract text";
            SetPassword = "set password";
            RemovePassword = "rm password";

            Ok = "Ok";
            Yes = "Yes";
            No = "No";
            Custom = "Custom";
            Auto = "Auto";

            FileEmptyMessage = "File is empty";
            FileExistsMessage = "File already exists";
            FileNotPdfMessage = "File is not PDF document";
            FileNotTxtMessage = "File is not TXT document";
            FailedToLoadFileMessage = "Failed to load file";
            ReplaceItMessage = "Replace it";
            UnfinishedTasksMessage = "You have unfinished tasks";
            ExitWithoutWaitingForCompletionMessage = "Exit without waiting for completion";
            SpecifyOutputFilePathMessage = "Specify the output file path";
            SpecifyCorrectQualityFactorMessage = "Specify the quality factor";
            SpecifyCorrectSizeFactorMessage = "Specify the size factor";
            SpecifyAtLeastOnePasswordMessage = "Specify at least one password";
            SpecifyPasswordMessage = "Specify the password";
            SpecifyAtLeastOneDivisionMessage = "Specify at least one division";
            SpecifyAtLeastOneCroppingMessage = "Specify at least one cropping";
            IncorrectShiftContentPagesMessage = "Pages on which the content will be shifted are incorrect";
            IncorrectPagesMessage = "Pages to which the current configuration will be applied are incorrect";
            IncorrectPageRangeMessage = "There is an incorrect page range";
            AllPagedRemovedMessage = "All pages have been removed";
            OperationCouldNotBePerformedMessage = "Operation could not be performed";
            DesignerIgnoreFontFamilyMessage = "Designer does not take into account the font family";
            DesignerHasSmallInaccuraciesInTextAllocationByWidthMessage = "Designer has small inaccuracies in the allocation of text by width";
            WatermarkAnnotationNotFullySupportedMessage = "Watermark annotation is not fully supported";
        }
    }
}
