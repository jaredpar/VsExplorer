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

namespace VsExplorer
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("1cd2ad05-d2d2-4151-958b-a1d13c547171")]
    public class MyToolWindow : ToolWindowPane
    {
        private ITextAdapter _textAdapter;
        private IDocumentViewerHost _documentViewerHost;

        public MyToolWindow() :
            base(null)
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            base.Content = new MyControl();
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