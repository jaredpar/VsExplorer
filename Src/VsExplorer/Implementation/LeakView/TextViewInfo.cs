using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.LeakView
{
    public sealed class TextViewInfo
    {
        private readonly string _identifier;
        private readonly ITextView _textView;

        public string Identifier
        {
            get { return _identifier; }
        }

        public ITextView TextView
        {
            get { return _textView; }
        }

        public TextViewInfo(string identifier, ITextView textView)
        {
            _identifier = identifier;
            _textView = textView;
        }
    }

    public sealed class TextViewInfoEventArgs : EventArgs
    {
        public readonly TextViewInfo TextViewInfo;

        public TextViewInfoEventArgs(TextViewInfo textViewInfo)
        {
            TextViewInfo = textViewInfo;
        }
    }
}
