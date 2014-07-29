using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

namespace VsExplorer.Implementation.BufferView
{
    [Flags]
    public enum TextModelFlags
    {
        None = 0x0,
        Visual = 0x1,
        Data = 0x2,
        Document = 0x4,
        Edit = 0x8,
    }

    public sealed class TextBufferInfo
    {
        private ITextBuffer _textBuffer;

        public string ContentType { get; set; }

        public ITextBuffer TextBuffer
        {
            get { return _textBuffer; }
        }

        public string Text
        {
            get { return _textBuffer.CurrentSnapshot.GetText(); }
        }

        public string FileName
        {
            get { return FilePath != null ? Path.GetFileName(FilePath) : "{None}"; }
        }

        public string FilePath { get; set; }

        public TextModelFlags TextModelFlags { get; set; }

        public TextBufferInfo(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
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
