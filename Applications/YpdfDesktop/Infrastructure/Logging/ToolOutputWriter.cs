using System;
using System.Globalization;
using System.IO;
using System.Text;
using YpdfDesktop.Models.Informing;

namespace YpdfDesktop.Infrastructure.Logging
{
    internal class ToolOutputWriter : TextWriter
    {
        private readonly ToolExecutionInfo _taskExecutionInfo;

        public override Encoding Encoding => Encoding.Unicode;
        public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        public ToolOutputWriter(ToolExecutionInfo taskExecutionInfo)
        {
            _taskExecutionInfo = taskExecutionInfo;
        }

        public override void Write(string? value)
        {
            _taskExecutionInfo.ToolOutput += value;
        }

        public override void Write(char value)
        {
            _taskExecutionInfo.ToolOutput += value;
        }
    }
}
