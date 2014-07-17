using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.Misc
{
    [Export(typeof(ITextAdapter))]
    internal sealed class TextAdapter : ITextAdapter
    {
        private readonly IVsEditorAdaptersFactoryService _vsEditorAdaptersFactoryService;
        private readonly IVsTextManager _vsTextManager;

        public ITextView ActiveTextViewOpt
        {
            get
            {
                try
                {
                    IVsTextView vsTextView;
                    if (ErrorHandler.Failed(_vsTextManager.GetActiveView(0, null, out vsTextView)) || vsTextView == null)
                    {
                        return null;
                    }

                    return _vsEditorAdaptersFactoryService.GetWpfTextView(vsTextView);
                }
                catch
                {
                    // GetWpfTextView can throw even for non-null values
                    return null;
                }
            }
        }

        [ImportingConstructor]
        internal TextAdapter(
            SVsServiceProvider serviceProvider,
            IVsEditorAdaptersFactoryService vsEditorAdaptersFactoryService)
        {
            _vsEditorAdaptersFactoryService = vsEditorAdaptersFactoryService;
            _vsTextManager = (IVsTextManager)serviceProvider.GetService(typeof(SVsTextManager));
        }
    }
}
