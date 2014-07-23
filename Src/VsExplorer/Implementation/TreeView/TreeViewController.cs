using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.TreeView
{
    internal sealed class TreeViewController : ITreeViewHost
    {
        private ITextView _textView;
        private TreeViewDisplay _treeViewDisplay;

        internal TreeViewController()
        {
            _treeViewDisplay = new TreeViewDisplay();
        }

        private void UpdateDisplay()
        {
            _treeViewDisplay.NamedBufferInfoCollection.Clear();

            if (_textView == null)
            {
                return;
            }

            _treeViewDisplay.NamedBufferInfoCollection.Add(new NamedBufferInfo() { Name = "Visual" });
            _treeViewDisplay.NamedBufferInfoCollection.Add(new NamedBufferInfo() { Name = "Edit" });
        }

        #region ITreeViewHost

        UIElement ITreeViewHost.Visual
        {
            get { return _treeViewDisplay; }
        }

        ITextView ITreeViewHost.TextView
        {
            get { return _textView; }
            set
            {
                if (_textView != value)
                {
                    _textView = value;
                    UpdateDisplay();
                }
            }
        }

        #endregion
    }
}
