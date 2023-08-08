using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using YpdfDesktop.Models.IO;
using YpdfDesktop.Models.Paging;
using YpdfDesktop.ViewModels.Pages.Tools;

namespace YpdfDesktop.Infrastructure.Behaviors.Drop
{
    public class PagesItemsControlDropHandler : DropHandlerBase
    {
        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return sender is ItemsControl itemsControl
                && Validate(itemsControl, e, sourceContext, targetContext, true);
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return sender is ItemsControl itemsControl
                && Validate(itemsControl, e, sourceContext, targetContext, false);
        }

        private bool Validate(ItemsControl itemsControl, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
        {
            return sourceContext is PageHandlingInfo sourceItem
                && ValidateInternalItem(itemsControl, sourceItem, e, targetContext, bExecute);
        }

        private bool ValidateInternalItem(ItemsControl itemsControl, PageHandlingInfo sourceItem, DragEventArgs e, object? targetContext, bool bExecute)
        {
            if (targetContext is not IPageCollectionContainer pagesContainer ||
                itemsControl.GetVisualAt(e.GetPosition(itemsControl)) is not Control targetControl ||
                targetControl.DataContext is not PageHandlingInfo targetItem)
            {
                return false;
            }

            int sourceItemIndex = pagesContainer.Pages.IndexOf(sourceItem);
            int targetItemIndex = pagesContainer.Pages.IndexOf(targetItem);

            if (sourceItemIndex < 0 || targetItemIndex < 0)
                return false;

            switch (e.DragEffects)
            {
                case DragDropEffects.Move:
                    if (bExecute) MoveItem(pagesContainer.Pages, sourceItemIndex, targetItemIndex);
                    return true;

                case DragDropEffects.Link:
                    if (bExecute) SwapItem(pagesContainer.Pages, sourceItemIndex, targetItemIndex);
                    return true;

                default:
                    return false;
            }
        }
    }
}
