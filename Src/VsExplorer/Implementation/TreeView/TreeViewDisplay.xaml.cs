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

namespace VsExplorer.Implementation.TreeView
{
    /// <summary>
    /// Interaction logic for TreeViewDisplay.xaml
    /// </summary>
    public partial class TreeViewDisplay : UserControl
    {
        public static readonly DependencyProperty TextBufferControlProperty = DependencyProperty.Register(
            "TextBufferControl",
            typeof(UIElement),
            typeof(TreeViewDisplay));

        private readonly ObservableCollection<NamedBufferInfo> _namedBufferInfoCollection = new ObservableCollection<NamedBufferInfo>();

        public ObservableCollection<NamedBufferInfo> NamedBufferInfoCollection
        {
            get { return _namedBufferInfoCollection; }
        }

        public UIElement TextBufferControl
        {
            get { return (UIElement)GetValue(TextBufferControlProperty); }
            set { SetValue(TextBufferControlProperty, value); }
        }

        public event EventHandler<SourceBufferInfoEventArgs> SelectedSourceBufferInfoChanged;

        public TreeViewDisplay()
        {
            InitializeComponent();
        }

        private void OnTreeViewItemChanged(object sender, EventArgs e)
        {
            var sourceBufferInfo = _treeView.SelectedItem as SourceBufferInfo;
            if (sourceBufferInfo == null)
            {
                var namedBufferInfo = _treeView.SelectedItem as NamedBufferInfo;
                if (namedBufferInfo != null)
                {
                    sourceBufferInfo = namedBufferInfo.SourceBufferInfo;
                }
            }

            var list = SelectedSourceBufferInfoChanged;
            if (sourceBufferInfo != null && list != null)
            {
                list(this, new SourceBufferInfoEventArgs(sourceBufferInfo));

            }
        }
    }
}
