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
    [Guid("5a1aa192-a905-4b5b-8196-0ea26874159b")]
    public sealed class LeakTextViewToolWindow : ToolWindowPane
    {
        public LeakTextViewToolWindow() :
            base(null)
        {
            this.Caption = "Leaked ITextView Detector";
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            base.Content = new TextBlock();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var exportProvider = componentModel.DefaultExportProvider;
            var leakTextViewHost = exportProvider.GetExportedValue<ILeakViewHost>();
            Content = leakTextViewHost.Visual;
        }
    }
}
