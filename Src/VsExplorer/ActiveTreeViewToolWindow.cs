using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Controls;

namespace VsExplorer
{
    [Guid("9c8b8515-ec51-4717-800e-f265c5505c0e")]
    public sealed class ActiveTreeViewToolWindow : ToolWindowPane
    {
        private ITextAdapter _textAdapter;
        private ITreeViewHost _treeViewHost;

        public ActiveTreeViewToolWindow() :
            base(null)
        {
            this.Caption = "Active Tree View";
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            base.Content = new TextBlock();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var exportProvider = componentModel.DefaultExportProvider;
            var treeViewHostProvider = exportProvider.GetExportedValue<ITreeViewHostProvider>();
            _treeViewHost = treeViewHostProvider.Create();
            _textAdapter = exportProvider.GetExportedValue<ITextAdapter>();
            _textAdapter.ActiveTextViewChanged += OnActiveTextViewChanged;

            Content = _treeViewHost.Visual;
            _treeViewHost.TextView = _textAdapter.ActiveTextViewOpt;
        }

        private void OnActiveTextViewChanged(object sender, ActiveTextViewChangedEventArgs e)
        {
            _treeViewHost.TextView = e.NewTextViewOpt;
        }
    }
}
