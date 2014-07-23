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

namespace VsExplorer.Implementation.BufferView
{
    internal sealed class DocumentViewerController : IBufferViewHost
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
            _documentViewer.DocumentPath = string.Empty;

            if (_textView == null)
            {
                return;
            }

            _documentViewer.DocumentRoles = String.Join(", ", _textView.Roles.ToArray());

            ITextDocument primaryTextDocument;
            if (_textDocumentFactoryService.TryGetTextDocument(_textView.TextDataModel.DocumentBuffer, out primaryTextDocument))
            {
                _documentViewer.DocumentPath = primaryTextDocument.FilePath;
            }
            else
            {
                _documentViewer.DocumentPath = "{None}";
            }

            var textBuffers = VsUtil.GetTextBuffersRecursive(_textView);
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

        UIElement IBufferViewHost.Visual
        {
            get { return _documentViewer; }
        }

        ITextView IBufferViewHost.TextView
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
