using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class TextViewInfo
    {
        private readonly ITextView _textView;
        private List<NamedBufferInfo> _namedBufferInfoList = new List<NamedBufferInfo>();

        public List<NamedBufferInfo> NamedBufferInfoList
        {
            get { return _namedBufferInfoList; }
        }

        public TextViewInfo(ITextView textView)
        {
            _textView = textView;
        }
    }
}
