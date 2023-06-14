using ExecutionLib.Configuration;
using ExecutionLib.Informing.Logging;
using ExecutionLib.Informing.Users;
using FileSystemLib.Naming;
using FileSystemLib.Paths;
using iText.Layout.Properties;
using YpdfLib.Compressors;
using YpdfLib.Converters;
using YpdfLib.Designers;
using YpdfLib.Extractors;
using YpdfLib.Imaging;
using YpdfLib.Informing;
using YpdfLib.Models.Design;
using YpdfLib.Models.Paging;
using YpdfLib.Security;

namespace ExecutionLib.Execution
{
    public class PdfToolExecutor
    {
        private readonly YpdfConfig _config;
        private readonly Dictionary<string, Action<YpdfConfig>> _commands;

        public delegate void ExecutionSuccessfullyCompletedHandler();
        public event ExecutionSuccessfullyCompletedHandler? ExecutionSuccessfullyCompleted;

        public delegate void ExecutionFaultedHandler();
        public event ExecutionFaultedHandler? ExecutionFaulted;

        public IExecutionLogger? Logger { get; set; }
        public IFileExistsUserAnswerDispatcher? FileExistsQuestion { get; set; }
        public IApplyCorrectionsUserAnswerDispatcher? ApplyCorrectionsQuestion { get; set; }

        public string[] PreferredExtensions { get; set; } = new string[]
        {
            "pdf",
            "txt",
            "jpg",
            "png"
        };

        public PdfToolExecutor(YpdfConfig config)
        {
            _config = config;
            _commands = GetCommands();
        }

        #region Executing

        public ExecutionInfo PrepareExecute(bool checkOutputPath = true, bool correctPaths = true)
        {
            if (string.IsNullOrEmpty(_config.PdfTool))
                return new ExecutionInfo(new NoFileProcessingToolException());

            try
            {
                _config.PathsConfig.PreparePaths();

                if (checkOutputPath)
                    CheckOutputPath();

                if (correctPaths)
                {
                    CorrectInputFile();
                    CorrectFilePaths();
                }
            }
            catch (Exception ex)
            {
                return new ExecutionInfo(ex);
            }

            return !_commands.TryGetValue(_config.PdfTool, out Action<YpdfConfig>? executor)
                ? new ExecutionInfo(new UnknownFileProcessingToolException(_config.PdfTool))
                : new ExecutionInfo(_config.PdfTool, executor);
        }

        public void Execute(IExecutionInfo executionInfo)
        {
            if (!executionInfo.CanExecute)
            {
                Logger?.LogError(executionInfo.Exception?.Message);
                ExecutionFaulted?.Invoke();
                return;
            }

            try
            {
                executionInfo.Executor?.Invoke(_config);
                ExecutionSuccessfullyCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.Message);

                string? outputPath = _config.PathsConfig.OutputPath;
                string[] inputFiles = _config.PathsConfig.AllInputFiles;

                if (File.Exists(outputPath))
                {
                    try
                    {
                        if (!inputFiles.Contains(outputPath))
                            File.Delete(outputPath);
                    }
                    catch { }
                }

                ExecutionFaulted?.Invoke();
            }
        }

        #endregion

        #region Initializing

        private Dictionary<string, Action<YpdfConfig>> GetCommands()
        {
            return new()
            {
                { "config", Config },
                { "compress", Compress },
                { "can-compress", CanCompress },
                { "copy", Copy },
                { "remove-pages", RemovePages },
                { "merge", Merge },
                { "move-page", MovePage },
                { "reorder-pages", ReorderPages },
                { "rotate", Rotate },
                { "crop", Crop },
                { "divide", Divide },
                { "split", Split },
                { "text-to-pdf", TextToPdf },
                { "add-page-numbers", AddPageNumbers },
                { "add-watermark", AddIndelibleWatermark },
                { "add-watermark-annotation", AddWatermarkAnnotation },
                { "remove-watermark-annotation", RemoveWatermarkAnnotation },
                { "extract-images", ExtractImages },
                { "extract-text", ExtractText },
                { "extract-text-tika", ExtractTextTika },
                { "compress-images", CompressImages },
                { "image-to-pdf", ConvertImagesToPdf },
                { "set-password", SetPassword },
                { "remove-password", RemovePassword }
            };
        }

        #endregion

        #region Correctors

        private IPathCorrection[] GetFilePathsCorrections(string[] paths)
        {
            return GetFilePathsCorrections(paths, paths);
        }

        private IPathCorrection[] GetFilePathsCorrections(string[] paths, string[] pathsForQuestion)
        {
            if (paths.Length == 0)
                return Array.Empty<IPathCorrection>();

            var corrections = PathCorrector.CorrectFilePaths(PreferredExtensions, paths);

            var correctionsForQuestion = corrections
                .Where(t => pathsForQuestion.Contains(t.OldPath))
                .ToArray();

            var failedCorrection = corrections.FirstOrDefault(t => !t.IsPathCorrected);

            if (failedCorrection is not null)
                throw new FileNotFoundException(null, failedCorrection.OldPath);

            UserAnswer? userAnswer = ApplyCorrectionsQuestion?.Ask(correctionsForQuestion);

            Logger?.Log(string.Empty);

            return userAnswer == UserAnswer.Yes
                ? corrections
                : throw new FileNotFoundException(null, paths[0]);
        }

        private void CorrectInputFile()
        {
            if (string.IsNullOrEmpty(_config.PathsConfig.InputPath))
                return;

            if (PathVerifier.VerifyFileExists(out List<string> undetectedFiles, _config.PathsConfig.InputPath))
                return;

            Logger?.LogWarning($"File {_config.PathsConfig.InputPath} not found.");
            Logger?.Log(string.Empty);

            IPathCorrection[] corrections = GetFilePathsCorrections(undetectedFiles.ToArray());
            string? newPath = corrections[0].NewPath;

            _config.PathsConfig.InputPath = newPath;
        }

        private void CorrectFilePaths()
        {
            if (_config.PathsConfig.FilePaths.Count == 0)
                return;

            string[] filePaths = _config.PathsConfig.FilePaths.ToArray();

            if (PathVerifier.VerifyFileExists(out List<string> undetectedFiles, filePaths))
                return;

            foreach (string filePath in undetectedFiles)
                Logger?.LogWarning($"File {filePath} not found.");

            Logger?.Log(string.Empty);

            string[] pathsForQuestion = undetectedFiles.ToArray();
            IPathCorrection[] corrections = GetFilePathsCorrections(filePaths, pathsForQuestion);

            for (int i = 0; i < corrections.Length; ++i)
                _config.PathsConfig.FilePaths[i] = corrections[i].NewPath ?? string.Empty;
        }

        private void CheckOutputPath()
        {
            if (string.IsNullOrEmpty(_config.PathsConfig.OutputPath) ||
                !File.Exists(_config.PathsConfig.OutputPath))
            {
                return;
            }

            UserAnswer? userAnswer = FileExistsQuestion?.Ask(_config.PathsConfig.OutputPath);

            if (userAnswer != UserAnswer.Yes)
                throw new FileExistsException(_config.PathsConfig.OutputPath);
        }

        #endregion

        #region Commands

        private void Config(YpdfConfig config)
        {
            if (config.ResetGlobalConfig)
                config.GlobalConfig.Reset(true);

            if (config.SaveGlobalConfig && !config.ResetGlobalConfig)
                config.GlobalConfig.Save();

            if (config.ShowGlobalConfig)
            {
                string[] configItems = config.GlobalConfig.GetItems();
                string itemsStr = string.Join('\n', configItems);

                Logger?.LogInfo(itemsStr);
            }
        }

        private void Compress(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Compressor.Compress(inputPath, outputPath, config.GlobalConfig.PythonAlias,
                config.ImageCompression, config.ImageCompression.CheckCompressionValidity,
                Logger?.Out);
        }

        private void CanCompress(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            bool result = Compressor.IsCompressionValid(inputPath, config.GlobalConfig.PythonAlias, config.ImageCompression);
            Logger?.LogInfo(result.ToString());
        }

        private void Copy(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Copier.Copy(inputPath, outputPath);
        }

        private void RemovePages(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Eraser.RemovePages(inputPath, outputPath, config.Pages.ToArray());
        }

        private void Merge(YpdfConfig config)
        {
            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Merger.Merge(outputPath, config.PathsConfig.AllInputFiles);
        }

        private void MovePage(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            int pageToMove = config.PageMovement.PageToMove
                ?? throw new UndefinedParameterException(nameof(config.PageMovement.PageToMove));

            int newPosition = config.PageMovement.NewPosition
                ?? throw new UndefinedParameterException(nameof(config.PageMovement.NewPosition));

            PageMover.MovePage(inputPath, outputPath, pageToMove, newPosition);
        }

        private void ReorderPages(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            var pageOrder = config.PageOrder
                ?? throw new UndefinedParameterException(nameof(config.PageOrder));

            PageMover.ReorderPages(inputPath, outputPath, pageOrder);
        }

        private void Rotate(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            if (config.RotationAngle is not null)
                Rotator.Rotate(inputPath, outputPath, config.RotationAngle.Value);
            else
                Rotator.Rotate(inputPath, outputPath, config.PageRotations.ToArray());
        }

        private void Crop(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Cutter.CropPages(inputPath, outputPath, config.PageCroppings.ToArray());
        }

        private void Divide(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            Cutter.DividePages(inputPath, outputPath, config.PageDivisions.ToArray());
        }

        private void Split(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            var splitter = new Splitter(inputPath, config.PathsConfig.OutputDirectory);

            if (config.Pages.Count == 0)
            {
                splitter.Split(config.SplittingPartSize ?? Splitter.DEFAULT_SPLITTING_PART_SIZE);
                return;
            }

            string[] ranges = config.Pages
                .Select(t => t.ToString() ?? string.Empty)
                .ToArray();

            splitter.Split(ranges);
        }

        private void TextToPdf(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            var parameters = config.GetConfiguredTextPageParameters();

            TextConverter.ConvertToPdf(inputPath, outputPath, parameters);
        }

        private void AddPageNumbers(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            IPageNumberStyle style = config.GetConfiguredPageNumberStyle();
            IPageContentShift[] numberShifts = config.ContentShifts.ToArray();

            if (config.PageChange.Increase is null)
            {
                PageDesigner.AddPageNumbers(inputPath, outputPath, style, numberShifts);
                return;
            }

            string extension = new FileInfo(inputPath).Extension;
            string tempFilePath = new UniqueFile(extension, SharedConfig.Directories.Temp).GetNext();

            int[] pageNumbers = PdfInfo.GetAllPageNumbers(inputPath);
            List<int[]> associatedPages = PdfInfo.GetAssociatedPages(inputPath);

            if (associatedPages.Count > 0)
            {
                string message = "The PDF document contains associated pages, enlargement of " +
                    "one of them will cause the increase of all the others. But the associated " +
                    "pages are not fully supported yet. Therefore, rendering of some objects " +
                    "may not work correctly. Below are the sets of associated pages:\n";

                Logger?.LogWarning(message);
            }

            for (int i = 0; i < associatedPages.Count; ++i)
            {
                int[] pageSet = associatedPages[i];
                string pageSetStr = string.Join(',', pageSet);

                Logger?.Log($"Set {i + 1}: {pageSetStr}");

                pageNumbers = pageNumbers
                    .Except(pageSet)
                    .Append(pageSet[0])
                    .ToArray();
            }

            switch (config.PageNumberStyle.LocationMode)
            {
                case LocationMode.INCREASE_LEFT:
                    style.ConsiderLeftPageMargin = false;
                    style.HorizontalAlignment = TabAlignment.LEFT;
                    PageResizer.IncreaseLeft(inputPath, tempFilePath, config.PageChange.Increase.Left, config.PageChange.FillColor, pageNumbers);
                    break;

                case LocationMode.INCREASE_TOP:
                    style.ConsiderTopPageMargin = false;
                    style.VerticalAlignment = VerticalAlignment.TOP;
                    PageResizer.IncreaseTop(inputPath, tempFilePath, config.PageChange.Increase.Top, config.PageChange.FillColor, pageNumbers);
                    break;

                case LocationMode.INCREASE_RIGHT:
                    style.ConsiderRightPageMargin = false;
                    style.HorizontalAlignment = TabAlignment.RIGHT;
                    PageResizer.IncreaseRight(inputPath, tempFilePath, config.PageChange.Increase.Right, config.PageChange.FillColor, pageNumbers);
                    break;

                case LocationMode.INCREASE_BOTTOM:
                    style.ConsiderBottomPageMargin = false;
                    style.VerticalAlignment = VerticalAlignment.BOTTOM;
                    PageResizer.IncreaseBottom(inputPath, tempFilePath, config.PageChange.Increase.Bottom, config.PageChange.FillColor, pageNumbers);
                    break;
            }

            PageDesigner.AddPageNumbers(tempFilePath, outputPath, style, numberShifts);
            File.Delete(tempFilePath);
        }

        private void AddIndelibleWatermark(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            IIndelibleWatermark watermark = config.GetConfiguredIndelibleWatermark();

            if (config.Pages.Count == 0)
                WatermarkDesigner.AddIndelibleWatermark(inputPath, outputPath, watermark);
            else
                WatermarkDesigner.AddIndelibleWatermark(inputPath, outputPath, watermark, config.PageNumbers);
        }

        private void AddWatermarkAnnotation(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            IWatermarkAnnotation watermark = config.GetConfiguredWatermarkAnnotation();

            if (config.Pages.Count == 0)
                WatermarkDesigner.AddWatermarkAnnotation(inputPath, outputPath, watermark);
            else
                WatermarkDesigner.AddWatermarkAnnotation(inputPath, outputPath, watermark, config.PageNumbers);
        }

        private void RemoveWatermarkAnnotation(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            if (config.Pages.Count == 0)
                WatermarkDesigner.RemoveWatermarkAnnotation(inputPath, outputPath);
            else
                WatermarkDesigner.RemoveWatermarkAnnotation(inputPath, outputPath, config.PageNumbers);
        }

        private void ExtractImages(YpdfConfig config)
        {
            ImageExtractor.Extract(config.PathsConfig.OutputDirectory,
                config.GlobalConfig.PythonAlias, config.PathsConfig.AllInputFiles, -1, Logger?.Out);
        }

        private void ExtractText(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            TextExtractor.Extract(inputPath, outputPath);
        }

        private void ExtractTextTika(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            TextExtractor.ExtractByPython(inputPath, outputPath, config.GlobalConfig.PythonAlias);
        }

        private void CompressImages(YpdfConfig config)
        {
            string[] inputFiles = config.PathsConfig.AllInputFiles;

            if (inputFiles.Length == 0)
                return;

            if (inputFiles.Length == 1)
            {
                string outputPath = config.PathsConfig.OutputPath
                    ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

                ImageCompressor.Compress(inputFiles[0], outputPath, config.ImageCompression,
                    config.GlobalConfig.PythonAlias, Logger?.Out);
            }
            else
            {
                ImageCompressor.Compress(inputFiles, config.PathsConfig.OutputDirectory,
                    config.ImageCompression, config.GlobalConfig.PythonAlias, Logger?.Out);
            }
        }

        private void ConvertImagesToPdf(YpdfConfig config)
        {
            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            IImagePageParameters parameters = config.GetConfiguredImagePageParameters();
            ImageInserter.ConvertToPdf(outputPath, parameters, config.PathsConfig.AllInputFiles);
        }

        private void SetPassword(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            if (config.PdfPassword.UserPassword is null && config.PdfPassword.OwnerPassword is null)
                throw new UndefinedParameterException("Password");

            Protector.SetPassword(inputPath, outputPath, config.PdfPassword);
        }

        private void RemovePassword(YpdfConfig config)
        {
            string inputPath = config.PathsConfig.InputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.InputPath));

            string outputPath = config.PathsConfig.OutputPath
                ?? throw new UndefinedParameterException(nameof(config.PathsConfig.OutputPath));

            if (config.PdfPassword.MasterPassword is null)
                throw new UndefinedParameterException("Password");

            Protector.RemovePassword(inputPath, outputPath, config.PdfPassword);
        }

        #endregion
    }
}
