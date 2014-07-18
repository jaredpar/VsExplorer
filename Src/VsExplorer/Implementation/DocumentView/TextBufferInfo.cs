using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.DocumentView
{
    public sealed class TextBufferInfo
    {
        public string ContentType { get; set; }

        public string Text { get; set; }

        public string FileName
        {
            get { return FilePath != null ? Path.GetFileName(FilePath) : "{None}"; }
        }

        public string FilePath { get; set; }

        public TextBufferInfo()
        {

        }
    }

    public sealed class TextBufferInfoEventArgs : EventArgs
    {
        private readonly TextBufferInfo _textBufferInfo;

        public TextBufferInfo TextBufferInfo
        {
            get { return _textBufferInfo; }
        }

        public TextBufferInfoEventArgs(TextBufferInfo textBufferInfo)
        {
            _textBufferInfo = textBufferInfo;
        }
    }
}
