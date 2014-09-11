using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.TextBufferDisplay
{
    internal sealed class TextBufferDisplayHost : ITextBufferDisplayHost
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        private readonly TextBufferDisplayControl _textBufferDisplayControl;
        private ITextBuffer _textBuffer;

        internal TextBufferDisplayHost(ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _textBufferDisplayControl = new TextBufferDisplayControl();
        }

        private void UpdateTextBuffer(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
            _textBufferDisplayControl.TextBufferInfo.Close();
            _textBufferDisplayControl.TextBufferInfo = CreateTextBufferInfo(textBuffer);
        }

        private TextBufferInfo CreateTextBufferInfo(ITextBuffer textBuffer)
        {
            string name = null;
            string documentPath = null;
            ITextDocument textDocument;
            if (_textDocumentFactoryService.TryGetTextDocument(textBuffer, out textDocument))
            {
                documentPath = textDocument.FilePath;
                name = Path.GetFileName(documentPath);
            }
            else 
            {
                // TODO: Get a better predictable name here 
                name = "Unnamed Buffer";
            }


            return new TextBufferInfo(
                textBuffer,
                name,
                documentPath);
        }

        #region ITextBufferDisplayHost

        UIElement ITextBufferDisplayHost.Visual
        {
            get { return _textBufferDisplayControl; }
        }

        ITextBuffer ITextBufferDisplayHost.TextBuffer
        {
            get { return _textBuffer; }
            set { UpdateTextBuffer(value); }
        }

        #endregion
    }
}
