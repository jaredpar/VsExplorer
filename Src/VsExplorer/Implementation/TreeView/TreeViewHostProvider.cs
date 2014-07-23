using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    [Export(typeof(ITreeViewHostProvider))]
    internal sealed class TreeViewHostProvider : ITreeViewHostProvider
    {
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        
        [ImportingConstructor]
        internal TreeViewHostProvider(ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
        }

        ITreeViewHost ITreeViewHostProvider.Create()
        {
            return new TreeViewController(_textDocumentFactoryService);
        }
    }
}
