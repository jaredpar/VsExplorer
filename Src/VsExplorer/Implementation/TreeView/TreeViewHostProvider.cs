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
        ITreeViewHost ITreeViewHostProvider.Create()
        {
            return new TreeViewController();
        }
    }
}
