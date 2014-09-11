﻿using System;
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
    public class DocumentBufferViewToolWindow : ToolWindowPane
    {
        private ITextAdapter _textAdapter;
        private ITextBufferDisplayHost _textBufferDisplayHost;

        public DocumentBufferViewToolWindow() :
            base(null)
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            base.Content = new TextBlock();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
            var exportProvider = componentModel.DefaultExportProvider;
            var textBufferDisplayHostProvider = exportProvider.GetExportedValue<ITextBufferDisplayHostProvider>();
            _textBufferDisplayHost = textBufferDisplayHostProvider.Create();
            _textAdapter = exportProvider.GetExportedValue<ITextAdapter>();
            _textAdapter.ActiveTextViewChanged += OnActiveTextViewChanged;

            Content = _textBufferDisplayHost.Visual;
            Update(_textAdapter.ActiveTextViewOpt);
        }

        private void OnActiveTextViewChanged(object sender, ActiveTextViewChangedEventArgs e)
        {
            Update(e.NewTextViewOpt);
        }

        private void Update(ITextView textView)
        {
            if (textView == null)
            {
                _textBufferDisplayHost.TextBuffer = null;
            }
            else
            {
                _textBufferDisplayHost.TextBuffer = textView.TextBuffer;
                _textBufferDisplayHost.Roles = string.Join(",", textView.Roles);
            }
        }
    }
}