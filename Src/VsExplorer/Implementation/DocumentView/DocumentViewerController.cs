using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.DocumentView
{
    [Export(typeof(IDocumentViewerHost))]
    internal sealed class DocumentViewerController : IDocumentViewerHost
    {
        private DocumentViewer Create(ITextView textView)
        {
            var textBuffers = GetTextBuffersRecursive(textView.TextBuffer);
            var control = new DocumentViewer();

            foreach (var textBuffer in textBuffers)
            {
                var textBufferInfo = new TextBufferInfo() { Name = textBuffer.ContentType.TypeName };
                control.TextBufferCollection.Add(textBufferInfo);
            }

            return control;
        }

        private IEnumerable<ITextBuffer> GetTextBuffersRecursive(ITextBuffer textBuffer)
        {
            var foundSet = new HashSet<ITextBuffer>();
            var toVisitQueue = new Queue<ITextBuffer>();
            toVisitQueue.Enqueue(textBuffer);

            while (toVisitQueue.Count > 0)
            {
                var current = toVisitQueue.Dequeue();
                if (!foundSet.Add(current))
                {
                    continue;
                }

                var projectionBuffer = current as IProjectionBuffer;
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

        #region IDocumentViewerHost

        UIElement IDocumentViewerHost.Create(ITextView textView)
        {
            return Create(textView);
        }

        #endregion
    }
}
