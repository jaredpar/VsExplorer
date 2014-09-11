using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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
        private readonly _DTE _dte;

        [ImportingConstructor]
        internal TextBufferDisplayHostProvider(ITextDocumentFactoryService textDocumentFactoryService, SVsServiceProvider vsServiceProvider)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _dte = (_DTE)vsServiceProvider.GetService(typeof(SDTE));
        }

        public ITextBufferDisplayHost Create()
        {
            return new TextBufferDisplayHost(_textDocumentFactoryService, _dte);
        }
    }
}
