using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer
{
    internal interface IBufferViewHost
    {
        UIElement Visual { get; }

        ITextView TextView { get; set; }
    }

    internal interface IBufferViewHostProvider
    {
        IBufferViewHost Create();
    }

    internal interface ITreeViewHost
    {
        UIElement Visual { get; }

        ITextView TextView { get; set; }
    }

    internal interface ITreeViewHostProvider
    {
        ITreeViewHost Create();
    }

    internal interface ITextAdapter
    {
        ITextView ActiveTextViewOpt { get; }

        event EventHandler<ActiveTextViewChangedEventArgs> ActiveTextViewChanged;
    }

    internal sealed class ActiveTextViewChangedEventArgs : EventArgs
    {
        internal readonly ITextView OldTextViewOpt;
        internal readonly ITextView NewTextViewOpt;

        internal ActiveTextViewChangedEventArgs(ITextView oldTextViewOpt, ITextView newTextViewOpt)
        {
            OldTextViewOpt = oldTextViewOpt;
            NewTextViewOpt = newTextViewOpt;
        }
    }
}
