using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace YpdfDesktop.Infrastructure.Communication
{
    internal class QuickMessage : IQuickMessage
    {
        public string Message { get; set; }

        public QuickMessage(string? message = null)
        {
            Message = message ?? string.Empty;
        }

        public Task<ButtonResult> Show(string title = "", ButtonEnum buttons = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            return GetMessageBoxWindow(title, buttons, icon).Show();
        }

        public Task<ButtonResult> ShowDialog(Window window, string title = "", ButtonEnum buttons = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            return GetMessageBoxWindow(title, buttons, icon).ShowDialog(window);
        }

        public Task<ButtonResult> ShowEmpty(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetEmptyWindow(buttons).Show();
        }

        public Task<ButtonResult> ShowEmptyDialog(Window window, ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetEmptyWindow(buttons).ShowDialog(window);
        }

        public Task<ButtonResult> ShowError(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetErrorWindow(buttons).Show();
        }

        public Task<ButtonResult> ShowErrorDialog(Window window, ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetErrorWindow(buttons).ShowDialog(window);
        }

        public Task<ButtonResult> ShowWarning(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetWarningWindow(buttons).Show();
        }

        public Task<ButtonResult> ShowWarningDialog(Window window, ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetWarningWindow(buttons).ShowDialog(window);
        }

        public Task<ButtonResult> ShowQuestion(ButtonEnum buttons = ButtonEnum.YesNo)
        {
            return GetQuestionWindow(buttons).Show();
        }

        public Task<ButtonResult> ShowQuestionDialog(Window window, ButtonEnum buttons = ButtonEnum.YesNo)
        {
            return GetQuestionWindow(buttons).ShowDialog(window);
        }

        public Task<ButtonResult> ShowInformation(ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetInfoWindow(buttons).Show();
        }

        public Task<ButtonResult> ShowInformationDialog(Window window, ButtonEnum buttons = ButtonEnum.Ok)
        {
            return GetInfoWindow(buttons).ShowDialog(window);
        }

        protected IMsBoxWindow<ButtonResult> GetMessageBoxWindow(string title, ButtonEnum buttons, Icon icon)
        {
            return MessageBoxManager.GetMessageBoxStandardWindow(title, Message, buttons, icon);
        }

        private IMsBoxWindow<ButtonResult> GetEmptyWindow(ButtonEnum buttons)
        {
            return GetMessageBoxWindow(string.Empty, buttons, Icon.None);
        }

        private IMsBoxWindow<ButtonResult> GetErrorWindow(ButtonEnum buttons)
        {
            return GetMessageBoxWindow("Error", buttons, Icon.Error);
        }

        private IMsBoxWindow<ButtonResult> GetWarningWindow(ButtonEnum buttons)
        {
            return GetMessageBoxWindow("Warning", buttons, Icon.Warning);
        }

        private IMsBoxWindow<ButtonResult> GetQuestionWindow(ButtonEnum buttons)
        {
            return GetMessageBoxWindow("Question", buttons, Icon.Question);
        }

        private IMsBoxWindow<ButtonResult> GetInfoWindow(ButtonEnum buttons)
        {
            return GetMessageBoxWindow("Info", buttons, Icon.Info);
        }
    }
}
