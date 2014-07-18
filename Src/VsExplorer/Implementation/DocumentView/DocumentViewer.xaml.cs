using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsExplorer.Implementation.DocumentView
{
    /// <summary>
    /// Interaction logic for DocumentViewer.xaml
    /// </summary>
    public partial class DocumentViewer : UserControl
    {
        public static readonly DependencyProperty DocumentRolesProperty = DependencyProperty.Register(
            "DocumentRoles",
            typeof(string),
            typeof(DocumentViewer));

        private readonly ObservableCollection<TextBufferInfo> _textBufferInfoCollection = new ObservableCollection<TextBufferInfo>();

        public event EventHandler<TextBufferInfoEventArgs> OpenRawTextClicked;

        public string DocumentRoles
        {
            get { return (string)GetValue(DocumentRolesProperty); }
            set { SetValue(DocumentRolesProperty, value); }
        }

        public ObservableCollection<TextBufferInfo> TextBufferCollection
        {
            get { return _textBufferInfoCollection; }
        }

        public DocumentViewer()
        {
            InitializeComponent();
        }

        private void OnOpenRawTextClick(object sender, EventArgs e)
        {
            var textBufferInfo = _textBufferInfoDataGrid.CurrentCell.Item as TextBufferInfo;
            if (textBufferInfo != null)
            {
                var handlers = OpenRawTextClicked;
                if (handlers != null)
                {
                    handlers(this, new TextBufferInfoEventArgs(textBufferInfo));
                }
            }
        }
    }
}
