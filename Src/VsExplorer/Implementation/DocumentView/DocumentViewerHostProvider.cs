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
        public IDocumentViewerHost Create()
        {
            return new DocumentViewerController();
        }
    }
}
