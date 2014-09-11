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

namespace VsExplorer.Implementation.TagDisplay
{
    /// <summary>
    /// Interaction logic for TagDisplayControl.xaml
    /// </summary>
    public partial class TagDisplayControl : UserControl
    {
        private readonly ObservableCollection<TagGroup> _tagGroupCollection = new ObservableCollection<TagGroup>();

        public ObservableCollection<TagGroup> TagGroupCollection
        {
            get { return _tagGroupCollection; }
        }

        public TagDisplayControl()
        {
            InitializeComponent();
        }
    }
}
