using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace VsExplorer.Implementation.LeakView
{
    [Export(typeof(LeakDetector))]
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class LeakDetector : IWpfTextViewCreationListener
    {
        private List<WeakReference<ITextView>> _textViewList = new List<WeakReference<ITextView>>();

        internal List<ITextView> GetLeakedTextViewList(bool forceGc = true)
        {
            if (forceGc)
            {
                RunGarbageCollector();
            }

            var list = new List<ITextView>();
            foreach (var weakRef in _textViewList)
            {
                ITextView textView;
                if (weakRef.TryGetTarget(out textView) && textView.IsClosed)
                {
                    list.Add(textView);
                }
            }

            return list;
        }

        internal void RunGarbageCollector()
        {
            for (var i = 0; i < 15; i++)
            {
                DoEvents(Dispatcher.CurrentDispatcher);
                GC.Collect(2, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();
                GC.Collect(2, GCCollectionMode.Forced);
                GC.Collect();
            }
        }

        private static void DoEvents(Dispatcher dispatcher)
        {
            var frame = new DispatcherFrame();
            Action<DispatcherFrame> action = _ => { frame.Continue = false; };
            dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                action,
                frame);
            Dispatcher.PushFrame(frame);
        }

        void IWpfTextViewCreationListener.TextViewCreated(IWpfTextView textView)
        {
            _textViewList.Add(new WeakReference<ITextView>(textView, trackResurrection: false));

            // LeakList.Add(textView);
        }

        static List<object> LeakList = new List<object>();
    }
}
