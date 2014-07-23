using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer
{
    internal static class VsUtil
    {
        /// <summary>
        /// Get all ITextBuffer instances which contribute to this view 
        /// </summary>
        /// <param name="textview"></param>
        /// <returns></returns>
        internal static IEnumerable<ITextBuffer> GetTextBuffersRecursive(ITextView textview)
        {
            var foundSet = new HashSet<ITextBuffer>();
            var toVisitQueue = new Queue<ITextBuffer>();
            toVisitQueue.Enqueue(textview.TextViewModel.EditBuffer);
            toVisitQueue.Enqueue(textview.TextViewModel.VisualBuffer);
            toVisitQueue.Enqueue(textview.TextViewModel.DataBuffer);
            toVisitQueue.Enqueue(textview.TextViewModel.DataModel.DocumentBuffer);

            while (toVisitQueue.Count > 0)
            {
                var current = toVisitQueue.Dequeue();
                if (!foundSet.Add(current))
                {
                    continue;
                }

                var projectionBuffer = current as IProjectionBufferBase;
                if (projectionBuffer != null)
                {
                    foreach (var inner in projectionBuffer.SourceBuffers)
                    {
                        toVisitQueue.Enqueue(inner);
                    }
                }
            }

            return foundSet;
        }

        internal static IVsWindowFrame GetWindowFrame(IVsTextView vsTextView)
        {
            var other = vsTextView as IVsTextViewEx;
            return other != null ? GetWindowFrame(other) : null;
        }

        internal static IVsWindowFrame GetWindowFrame(IVsTextViewEx vsTextView)
        {
            object frame;
            if (!ErrorHandler.Succeeded(vsTextView.GetWindowFrame(out frame)))
            {
                return null;
            }

            return frame as IVsWindowFrame;
        }

        internal static IVsCodeWindow GetCodeWindow(IVsTextView vsTextView)
        {
            var vsWindowFrame = GetWindowFrame(vsTextView);
            if (vsWindowFrame == null)
            {
                return null;
            }

            var iid = typeof(IVsCodeWindow).GUID;
            var ptr = IntPtr.Zero;
            try
            {
                ErrorHandler.ThrowOnFailure(vsWindowFrame.QueryViewInterface(ref iid, out ptr));
                return (IVsCodeWindow)Marshal.GetObjectForIUnknown(ptr);
            }
            catch (Exception)
            {
                // Venus will throw when querying for the code window
                return null;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.Release(ptr);
                }
            }
        }
    }
}
