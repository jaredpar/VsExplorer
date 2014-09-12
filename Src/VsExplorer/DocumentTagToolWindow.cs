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
    [Guid("723476fe-6544-42bc-ba4c-c2cb7399a980")]
    public class DocumentTagToolWindow : ToolWindowPane
    {
        private ITextAdapter _textAdapter;
        private ITagDisplayHost _tagDisplayHost;

        public DocumentTagToolWindow() :
            base(null)
        {
            this.Caption = "Document Tags";
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            base.Content = new TextBlock();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var exportProvider = componentModel.DefaultExportProvider;
            var tagDisplayHostProvider = exportProvider.GetExportedValue<ITagDisplayHostProvider>();
            _tagDisplayHost = tagDisplayHostProvider.Create();
            _textAdapter = exportProvider.GetExportedValue<ITextAdapter>();
            _textAdapter.ActiveTextViewChanged += OnActiveTextViewChanged;

            Content = _tagDisplayHost.Visual;
            Update(_textAdapter.ActiveTextViewOpt);
        }

        private void OnActiveTextViewChanged(object sender, ActiveTextViewChangedEventArgs e)
        {
            Update(e.NewTextViewOpt);
        }

        private void Update(ITextView textView)
        {
            _tagDisplayHost.TextView = textView;
        }
    }
}