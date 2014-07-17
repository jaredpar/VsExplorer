using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer
{
    internal interface IDocumentViewerHost
    {
        UIElement Create(ITextView textView);
    }

    internal interface ITextAdapter
    {
        ITextView ActiveTextViewOpt { get; }
    }
}
