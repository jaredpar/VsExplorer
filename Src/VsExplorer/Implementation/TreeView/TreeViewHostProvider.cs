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
        private readonly ITextEditorFactoryService _textEditorFactoryService;
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;
        
        [ImportingConstructor]
        internal TreeViewHostProvider(ITextEditorFactoryService textEditorFactoryService, ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textEditorFactoryService = textEditorFactoryService;
            _textDocumentFactoryService = textDocumentFactoryService;
        }

        ITreeViewHost ITreeViewHostProvider.Create()
        {
            return new TreeViewController(_textEditorFactoryService, _textDocumentFactoryService);
        }
    }
}
