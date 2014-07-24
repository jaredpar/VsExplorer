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
        public static readonly DependencyProperty TextBufferInfoProperty = DependencyProperty.Register(
            "TextBufferInfo",
            typeof(SourceBufferInfo),
            typeof(TreeViewDisplay),
            new PropertyMetadata(SourceBufferInfo.Empty));

        private readonly ObservableCollection<NamedBufferInfo> _namedBufferInfoCollection = new ObservableCollection<NamedBufferInfo>();

        public ObservableCollection<NamedBufferInfo> NamedBufferInfoCollection
        {
            get { return _namedBufferInfoCollection; }
        }

        public SourceBufferInfo TextBufferInfo
        {
            get { return (SourceBufferInfo)GetValue(TextBufferInfoProperty); }
            set { SetValue(TextBufferInfoProperty, value); }
        }

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

            TextBufferInfo = sourceBufferInfo != null ? sourceBufferInfo : SourceBufferInfo.Empty;
        }
    }
}
