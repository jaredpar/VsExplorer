using EnvDTE;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.DocumentView
{
    internal sealed class DocumentViewerController : IDocumentViewerHost
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        private readonly _DTE _dte;
        private readonly DocumentViewer _documentViewer;
        private ITextView _textView;

        public DocumentViewerController(ITextDocumentFactoryService textDocumentFactoryService, _DTE dte)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _dte = dte;
            _documentViewer = new DocumentViewer();
            _documentViewer.OpenRawTextClicked += OnOpenRawTextClicked;
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

            var textBuffers = GetTextBuffersRecursive(_textView);
            foreach (var textBuffer in textBuffers)
            {
                string filePath = "";
                ITextDocument textDocument;
                if (_textDocumentFactoryService.TryGetTextDocument(textBuffer, out textDocument))
                {
                    filePath = textDocument.FilePath;
                }

                var textBufferInfo = new TextBufferInfo(textBuffer) {
                    ContentType = textBuffer.ContentType.TypeName,
                    FilePath = filePath,
                    TextModelFlags = GetTextModelFlags(_textView, textBuffer),
                };
                _documentViewer.TextBufferCollection.Add(textBufferInfo);
            }
        }

        private static TextModelFlags GetTextModelFlags(ITextView textView, ITextBuffer textBuffer)
        {
            var flags = TextModelFlags.None;
            if (textView.TextViewModel.VisualBuffer == textBuffer)
            {
                flags |= TextModelFlags.Visual;
            }

            if (textView.TextViewModel.EditBuffer == textBuffer)
            {
                flags |= TextModelFlags.Edit;
            }

            if (textView.TextDataModel.DataBuffer == textBuffer)
            {
                flags |= TextModelFlags.Data;
            }

            if (textView.TextDataModel.DocumentBuffer == textBuffer)
            {
                flags |= TextModelFlags.Document;
            }

            return flags;
        }

        private static IEnumerable<ITextBuffer> GetTextBuffersRecursive(ITextView textview)
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

        private void OnOpenRawTextClicked(object sender, TextBufferInfoEventArgs e)
        {
            try
            {
                var filePath = Path.GetTempFileName();
                File.WriteAllText(filePath, e.TextBufferInfo.Text);

                _dte.ItemOperations.OpenFile(filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
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
