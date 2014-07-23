using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class SourceBufferInfo
    {
        private readonly ITextBuffer _textBuffer;
        private readonly string _name;
        private readonly ObservableCollection<SourceBufferInfo> _children = new ObservableCollection<SourceBufferInfo>();

        public string Name { get { return _name; } }

        public ITextBuffer TextBuffer { get { return _textBuffer; }}

        public ObservableCollection<SourceBufferInfo> Children { get { return _children; } }

        public SourceBufferInfo(string name, ITextBuffer textBuffer)
        {
            _name = name;
            _textBuffer = textBuffer;
        }
    }
}
