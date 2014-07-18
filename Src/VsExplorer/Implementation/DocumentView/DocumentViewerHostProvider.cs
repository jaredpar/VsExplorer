using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.DocumentView
{
    [Export(typeof(IDocumentViewerHostProvider))]
    internal sealed class DocumentViewerHostProvider : IDocumentViewerHostProvider
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        private readonly _DTE _dte;

        [ImportingConstructor]
        internal DocumentViewerHostProvider(ITextDocumentFactoryService textDocumentFactoryService, SVsServiceProvider serviceProvider)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _dte = (_DTE)serviceProvider.GetService(typeof(SDTE));
        }

        public IDocumentViewerHost Create()
        {
            return new DocumentViewerController(_textDocumentFactoryService, _dte);
        }
    }
}
