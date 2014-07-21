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
    [Guid("1cd2ad05-d2d2-4151-958b-a1d13c547171")]
    public sealed class DocumentViewToolWindow : ToolWindowPane
    {
        private ITextAdapter _textAdapter;
        private IDocumentViewerHost _documentViewerHost;

        public DocumentViewToolWindow() :
            base(null)
        {
            Caption = Resources.ToolWindowTitle;
            BitmapResourceID = 301;
            BitmapIndex = 1;
            Content = new TextBlock();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var exportProvider = componentModel.DefaultExportProvider;
            var documentViewerHostProvider = exportProvider.GetExportedValue<IDocumentViewerHostProvider>();
            _documentViewerHost = documentViewerHostProvider.Create();
            _textAdapter = exportProvider.GetExportedValue<ITextAdapter>();
            _textAdapter.ActiveTextViewChanged += OnActiveTextViewChanged;

            Content = _documentViewerHost.Visual;
            _documentViewerHost.TextView = _textAdapter.ActiveTextViewOpt;
        }

        private void OnActiveTextViewChanged(object sender, ActiveTextViewChangedEventArgs e)
        {
            _documentViewerHost.TextView = e.NewTextViewOpt;
        }
    }
}