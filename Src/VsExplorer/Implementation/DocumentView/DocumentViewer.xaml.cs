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

namespace VsExplorer.Implementation.DocumentView
{
    /// <summary>
    /// Interaction logic for DocumentViewer.xaml
    /// </summary>
    public partial class DocumentViewer : UserControl
    {
        private readonly ObservableCollection<TextBufferInfo> _textBufferInfoCollection = new ObservableCollection<TextBufferInfo>();

        public ObservableCollection<TextBufferInfo> TextBufferCollection
        {
            get { return _textBufferInfoCollection; }
        }

        public DocumentViewer()
        {
            InitializeComponent();
        }
    }
}
