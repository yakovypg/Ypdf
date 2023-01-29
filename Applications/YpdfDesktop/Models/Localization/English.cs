namespace YpdfDesktop.Models.Localization
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

            Language = "Language";
            Theme = "Theme";
            PythonAlias = "Python path/alias";
            Save = "Save";

            From = "From";
            To = "To";
            Execute = "Execute";
            Reset = "Reset";
            File = "File";
            Output = "Output";

            Split = "split";
            Merge = "merge";
            Compress = "compress";
            HandlePages = "handle pages";
            CropPages = "crop pages";
            DividePages = "divide pages";
            AddPageNumbers = "add page nums";
            AddWatermark = "add watermark";
            RemoveWatermark = "rm watermark";
            ImageToPdf = "image2pdf";
            TextToPdf = "text2pdf";
            ExtractImages = "extract images";
            ExtractText = "extract text";
            SetPassword = "set password";
            RemovePassword = "rm password";
        }
    }
}
