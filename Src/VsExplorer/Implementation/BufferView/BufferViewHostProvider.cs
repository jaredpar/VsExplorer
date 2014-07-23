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

namespace VsExplorer.Implementation.BufferView
{
    [Export(typeof(IBufferViewHostProvider))]
    internal sealed class BufferViewHostProvider : IBufferViewHostProvider
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        private readonly _DTE _dte;

        [ImportingConstructor]
        internal BufferViewHostProvider(ITextDocumentFactoryService textDocumentFactoryService, SVsServiceProvider serviceProvider)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _dte = (_DTE)serviceProvider.GetService(typeof(SDTE));
        }

        public IBufferViewHost Create()
        {
            return new BufferViewController(_textDocumentFactoryService, _dte);
        }
    }
}
