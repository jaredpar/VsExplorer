using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TextBufferDisplay
{
    [Export(typeof(ITextBufferDisplayHostProvider))]
    internal sealed class TextBufferDisplayHostProvider : ITextBufferDisplayHostProvider
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;

        [ImportingConstructor]
        internal TextBufferDisplayHostProvider(ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
        }

        public ITextBufferDisplayHost Create()
        {
            return new TextBufferDisplayHost(_textDocumentFactoryService);
        }
    }
}
