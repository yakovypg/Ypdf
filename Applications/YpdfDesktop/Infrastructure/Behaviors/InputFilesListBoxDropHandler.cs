using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Pages.Tools;

namespace YpdfDesktop.Infrastructure.Behaviors
{
    public class InputFilesListBoxDropHandler : DropHandlerBase
    {
        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return sender is ListBox listBox &&
                   Validate(listBox, e, sourceContext, targetContext, true);
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return sender is ListBox listBox &&
                   Validate(listBox, e, sourceContext, targetContext, false);
        }

        private bool Validate(ListBox listBox, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
        {
            return sourceContext is InputFile sourceItem
                ? ValidateInternalItem(listBox, sourceItem, e, targetContext, bExecute)
                : ValidateExternalItem(listBox, e, targetContext, bExecute);
        }

        private bool ValidateInternalItem(ListBox listBox, InputFile sourceItem, DragEventArgs e, object? targetContext, bool bExecute)
        {
            if (targetContext is not MergeViewModel viewModel ||
                listBox.GetVisualAt(e.GetPosition(listBox)) is not Control targetControl ||
                targetControl.DataContext is not InputFile targetItem)
            {
                return false;
            }

            int sourceItemIndex = viewModel.InputFiles.IndexOf(sourceItem);
            int targetItemIndex = viewModel.InputFiles.IndexOf(targetItem);

            if (sourceItemIndex < 0 || targetItemIndex < 0)
                return false;

            switch (e.DragEffects)
            {
                case DragDropEffects.Move:
                    if (bExecute) MoveItem(viewModel.InputFiles, sourceItemIndex, targetItemIndex);
                    return true;

                case DragDropEffects.Link:
                    if (bExecute) SwapItem(viewModel.InputFiles, sourceItemIndex, targetItemIndex);
                    return true;

                default:
                    return false;
            }
        }

        private bool ValidateExternalItem(ListBox listBox, DragEventArgs e, object? targetContext, bool bExecute)
        {
            if (targetContext is not MergeViewModel viewModel ||
                listBox.GetVisualAt(e.GetPosition(listBox)) is not Control targetControl ||
                !e.Data.Contains(DataFormats.FileNames))
            {
                return false;
            }

            IEnumerable<string>? paths = e.Data.GetFileNames()?.Reverse();

            if (paths is null)
                return false;

            int targetIndex = targetControl.DataContext is InputFile targetItem
                ? viewModel.InputFiles.IndexOf(targetItem)
                : 0;

            if (targetIndex < 0)
                return false;

            if (!bExecute)
                return true;

            if (!TryGetInputFiles(paths, out IEnumerable<InputFile> inputFiles))
                return false;

            if (inputFiles.Any(t => t.File.Extension.Replace(".", null).ToLower() != "pdf"))
                return false;

            foreach (InputFile inputFile in inputFiles)
                InsertItem(viewModel.InputFiles, inputFile, targetIndex);

            return true;
        }

        private static bool TryGetInputFiles(IEnumerable<string> paths, out IEnumerable<InputFile> inputFiles)
        {
            try
            {
                inputFiles = paths.Select(t => new InputFile(t));
                return true;
            }
            catch
            {
                inputFiles = Array.Empty<InputFile>();
                return false;
            }
        }
    }
}
