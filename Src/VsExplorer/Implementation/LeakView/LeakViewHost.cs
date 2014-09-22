using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.LeakView
{
    [Export(typeof(ILeakViewHost))]
    internal sealed class LeakViewHost : ILeakViewHost
    {
        private readonly LeakDisplayControl _control;
        private readonly LeakDetector _leakDetector;
        private readonly ITextBufferDisplayHost _textBufferDisplayHost;
        private readonly ITextDocumentFactoryService _textDocumentFactoryService;

        [ImportingConstructor]
        internal LeakViewHost(LeakDetector leakDetector, ITextBufferDisplayHostProvider textBufferDisplayHostProvider, ITextDocumentFactoryService textDocumentFactoryService)
        {
            _textBufferDisplayHost = textBufferDisplayHostProvider.Create();
            _control = new LeakDisplayControl();
            _control.RefreshClick += delegate { Refresh(); };
            _control.ClearClick += delegate { Clear(); };
            _control.SelectedTextViewInfoChanged += OnSelectedTextViewInfoChanged;
            _control.TextViewControl = _textBufferDisplayHost.Visual;
            _leakDetector = leakDetector;
            _textDocumentFactoryService = textDocumentFactoryService;
        }

        private void Clear()
        {
            _control.LeakTextViewInfoCollection.Clear();
            _textBufferDisplayHost.TextBuffer = null;
            _leakDetector.RunGarbageCollector();
        }

        private void Refresh()
        {
            // Clear out all UI references to ITextView references first
            Clear();

            var list = _leakDetector.GetLeakedTextViewList(forceGc: true);
            foreach (var textView in list)
            {
                // TODO: Centralize the generation of identifiers for this project.  Too much adhoc usage here 
                string identifier = "Unknown";
                ITextDocument textDocument;
                if (_textDocumentFactoryService.TryGetTextDocument(textView.TextDataModel.DocumentBuffer, out textDocument))
                {
                    identifier = Path.GetFileName(textDocument.FilePath);
                }

                _control.LeakTextViewInfoCollection.Add(new TextViewInfo(identifier, textView));
            }
        }

        private void OnSelectedTextViewInfoChanged(object sender, TextViewInfoEventArgs e)
        {
            _textBufferDisplayHost.TextBuffer = e.TextViewInfo != null
                ? e.TextViewInfo.TextView.TextBuffer
                : null;
        }

        #region ILeakViewHost

        UIElement ILeakViewHost.Visual
        {
            get { return _control; }
        }

        void ILeakViewHost.Refresh()
        {
            Refresh();
        }

        #endregion
    }
}
