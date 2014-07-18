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
    internal sealed class DocumentViewerController : IDocumentViewerHost
    {
        private readonly DocumentViewer _documentViewer;
        private ITextView _textView;

        public DocumentViewerController()
        {
            _documentViewer = new DocumentViewer();
        }

        private void UpdateModel()
        {
            _documentViewer.TextBufferCollection.Clear();
            _documentViewer.DocumentRoles = string.Empty;

            if (_textView == null)
            {
                return;
            }

            _documentViewer.DocumentRoles = String.Join(", ", _textView.Roles.ToArray());

            var textBuffers = GetTextBuffersRecursive(_textView.TextBuffer);
            foreach (var textBuffer in textBuffers)
            {
                var textBufferInfo = new TextBufferInfo() { 
                    ContentType = textBuffer.ContentType.TypeName,
                    Text = textBuffer.CurrentSnapshot.GetText()
                };
                _documentViewer.TextBufferCollection.Add(textBufferInfo);
            }
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

        UIElement IDocumentViewerHost.Visual
        {
            get { return _documentViewer; }
        }

        ITextView IDocumentViewerHost.TextView
        {
            get { return _textView; }
            set
            {
                if (_textView != value)
                {
                    _textView = value;
                    UpdateModel();
                }
            }
        }

        #endregion
    }
}
