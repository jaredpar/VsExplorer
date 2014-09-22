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

namespace VsExplorer.Implementation.LeakView
{
    public partial class LeakDisplayControl : UserControl
    {
        public static readonly DependencyProperty TextViewControlProperty = DependencyProperty.Register(
            "TextViewControl",
            typeof(UIElement),
            typeof(LeakDisplayControl));

        private readonly ObservableCollection<TextViewInfo> _leakTextViewInfoCollection = new ObservableCollection<TextViewInfo>();

        public ObservableCollection<TextViewInfo> LeakTextViewInfoCollection
        {
            get { return _leakTextViewInfoCollection; }
        }

        public UIElement TextViewControl
        {
            get { return (UIElement)GetValue(TextViewControlProperty); }
            set { SetValue(TextViewControlProperty, value); }
        }

        public event EventHandler RefreshClick;

        public event EventHandler ClearClick;

        public event EventHandler<TextViewInfoEventArgs> SelectedTextViewInfoChanged;

        public LeakDisplayControl()
        {
            InitializeComponent();
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            var handlers = RefreshClick;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }

        private void OnClearClick(object sender, EventArgs e)
        {
            var handlers = ClearClick;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }

        private void OnSelectedLeakTextViewInfoChanged(object sender, EventArgs e)
        {
            var current = _leakTextViewListBox.SelectedItem as TextViewInfo;
            var handlers = SelectedTextViewInfoChanged;
            if (handlers != null)
            {
                handlers(this, new TextViewInfoEventArgs(current));
            }
        }
    }
}
