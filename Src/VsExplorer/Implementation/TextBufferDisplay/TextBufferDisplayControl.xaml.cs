using System;
using System.Collections.Generic;
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

namespace VsExplorer.Implementation.TextBufferDisplay
{
    /// <summary>
    /// Interaction logic for TextBufferDisplayControl.xaml
    /// </summary>
    public partial class TextBufferDisplayControl : UserControl
    {
        public static readonly DependencyProperty TextBufferInfoProperty = DependencyProperty.Register(
            "TextBufferInfo",
            typeof(TextBufferInfo),
            typeof(TextBufferDisplayControl),
            new PropertyMetadata(TextBufferInfo.Empty));

        public TextBufferInfo TextBufferInfo
        {
            get { return (TextBufferInfo)GetValue(TextBufferInfoProperty); }
            set { SetValue(TextBufferInfoProperty, value); }
        }

        public TextBufferDisplayControl()
        {
            InitializeComponent();
        }
    }
}
