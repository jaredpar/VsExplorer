using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
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
        private readonly ITextBufferDisplayHostProvider _textBufferDisplayHostProvider;
        
        [ImportingConstructor]
        internal TreeViewHostProvider(ITextDocumentFactoryService textDocumentFactoryService, ITextBufferDisplayHostProvider textBufferDisplayHostProvider)
        {
            _textDocumentFactoryService = textDocumentFactoryService;
            _textBufferDisplayHostProvider = textBufferDisplayHostProvider;
        }

        ITreeViewHost ITreeViewHostProvider.Create()
        {
            return new TreeViewController(_textDocumentFactoryService, _textBufferDisplayHostProvider.Create());
        }
    }
}
