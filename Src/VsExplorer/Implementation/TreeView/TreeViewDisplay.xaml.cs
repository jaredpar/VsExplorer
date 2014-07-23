﻿using System;
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
        private readonly ObservableCollection<NamedBufferInfo> _namedBufferInfoCollection = new ObservableCollection<NamedBufferInfo>();

        public ObservableCollection<NamedBufferInfo> NamedBufferInfoCollection
        {
            get { return _namedBufferInfoCollection; }
        }

        public TreeViewDisplay()
        {
            InitializeComponent();
        }
    }
}
