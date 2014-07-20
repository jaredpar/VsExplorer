using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace VsExplorer.Implementation.Misc
{
    [Export(typeof(ITextAdapter))]
    internal sealed class TextAdapter : ITextAdapter, IVsSelectionEvents
    {
        private readonly IVsEditorAdaptersFactoryService _vsEditorAdaptersFactoryService;
        private readonly IVsTextManager _vsTextManager;
        private uint _eventCookie;
        private ITextView _activeTextView;
        private event EventHandler<ActiveTextViewChangedEventArgs> _activeTextViewChanged;

        [ImportingConstructor]
        internal TextAdapter(
            SVsServiceProvider serviceProvider,
            IVsEditorAdaptersFactoryService vsEditorAdaptersFactoryService)
        {
            _vsEditorAdaptersFactoryService = vsEditorAdaptersFactoryService;
            _vsTextManager = (IVsTextManager)serviceProvider.GetService(typeof(SVsTextManager));
            _activeTextView = QueryActiveTextView();

            var vsMonitorSelection = (IVsMonitorSelection)serviceProvider.GetService(typeof(SVsShellMonitorSelection));
            vsMonitorSelection.AdviseSelectionEvents(this, out _eventCookie);
        }

        private ITextView QueryActiveTextView()
        {
            try
            {
                IVsTextView vsTextView;
                if (ErrorHandler.Failed(_vsTextManager.GetActiveView(0, null, out vsTextView)) || vsTextView == null)
                {
                    return null;
                }

                var textView =  _vsEditorAdaptersFactoryService.GetWpfTextView(vsTextView);
                if (textView == null)
                {
                    return null;
                }

                // If the view doesn't have aggregate focus then check to see if the peek window has focus
                if (!textView.HasAggregateFocus)
                {
                    var peekTextView = QueryPeekWindow(vsTextView);
                    if (peekTextView != null && peekTextView.HasAggregateFocus)
                    {
                        return peekTextView;
                    }
                }

                return textView;
            }
            catch
            {
                // GetWpfTextView can throw even for non-null values
                return null;
            }
        }

        private ITextView QueryPeekWindow(IVsTextView vsTextView)
        {
            var vsCodeWindow = VsUtil.GetCodeWindow(vsTextView) as IVsCodeWindow2;
            if (vsCodeWindow == null)
            {
                return null;
            }

            IVsCodeWindow peekCodeWindow;
            IVsTextView peekVsTextView;
            if (ErrorHandler.Failed(vsCodeWindow.GetEmbeddedCodeWindow(0, out peekCodeWindow)) ||
                peekCodeWindow == null ||
                ErrorHandler.Failed(peekCodeWindow.GetLastActiveView(out peekVsTextView)) ||
                peekVsTextView == null)
            {
                return null;
            }

            try
            {
                return _vsEditorAdaptersFactoryService.GetWpfTextView(peekVsTextView);
            }
            catch
            {
                // GetWpfTextView can throw for even non-null values
                return null;
            }
        }

        private void CheckForActiveTextViewChange()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action)CheckForActiveTextViewChangeCore, DispatcherPriority.ApplicationIdle);
        }

        private void OnTextViewFocusChanged(object sender, EventArgs e)
        {
            CheckForActiveTextViewChange();
        }

        private void CheckForActiveTextViewChangeCore()
        {
            var textView = QueryActiveTextView();
            if (textView != _activeTextView)
            {
                // There is no great global event to listen for when the peek window is 
                // displayed.  Instead we just listen for changes to the focus as a clue
                // to re-check the active IVsTextView
                if (_activeTextView != null)
                {
                    _activeTextView.LostAggregateFocus -= OnTextViewFocusChanged;
                    _activeTextView.GotAggregateFocus -= OnTextViewFocusChanged;
                }

                if (textView != null)
                {
                    textView.LostAggregateFocus += OnTextViewFocusChanged;
                    textView.GotAggregateFocus += OnTextViewFocusChanged;
                }

                var args = new ActiveTextViewChangedEventArgs(_activeTextView, textView);
                _activeTextView = textView;

                var e = _activeTextViewChanged;
                if (e != null)
                {
                    e(this, args);
                }
            }
        }

        #region IVSelectionEvents

        int IVsSelectionEvents.OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        int IVsSelectionEvents.OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            if (elementid == (uint)VSConstants.VSSELELEMID.SEID_DocumentFrame ||
                elementid == (uint)VSConstants.VSSELELEMID.SEID_WindowFrame)
            {
                CheckForActiveTextViewChange();
            }

            return VSConstants.S_OK;
        }

        int IVsSelectionEvents.OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region ITextAdapter

        ITextView ITextAdapter.ActiveTextViewOpt
        {
            get { return _activeTextView; }
        }

        event EventHandler<ActiveTextViewChangedEventArgs> ITextAdapter.ActiveTextViewChanged
        {
            add { _activeTextViewChanged += value; }
            remove { _activeTextViewChanged -= value; }
        }

        #endregion
    }
}
