using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer
{
    internal interface ITreeViewHost
    {
        UIElement Visual { get; }

        ITextView TextView { get; set; }
    }

    internal interface ITreeViewHostProvider
    {
        ITreeViewHost Create();
    }

    /// <summary>
    /// The host for displaying a control that represents an ITextBuffer
    /// </summary>
    internal interface ITextBufferDisplayHost
    {
        UIElement Visual { get; }

        string Roles { get; set; }

        ITextBuffer TextBuffer { get; set; }
    }

    internal interface ITextBufferDisplayHostProvider
    {
        ITextBufferDisplayHost Create();
    }

    internal interface ITagDisplayHost
    {
        UIElement Visual { get; }

        ITextBuffer TextBuffer { get; set; }
    }

    internal interface ITagDisplayHostProvider
    {
        ITagDisplayHost Create();
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
