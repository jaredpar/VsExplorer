using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class SourceBufferInfo
    {
        private readonly ITextBuffer _textBuffer;
        private readonly string _id;

        public string Name { get; set; }

        public SourceBufferInfo(string id, ITextBuffer textBuffer)
        {
            _id = id;
            _textBuffer = textBuffer;
        }
    }
}
