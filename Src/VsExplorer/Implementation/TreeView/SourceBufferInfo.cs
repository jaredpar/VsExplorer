using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class SourceBufferInfo : INotifyPropertyChanged
    {
        private readonly ITextBuffer _textBuffer;
        private readonly string _name;
        private readonly string _documentPath;
        private readonly ObservableCollection<SourceBufferInfo> _children = new ObservableCollection<SourceBufferInfo>();
        private event PropertyChangedEventHandler _propertyChanged;

        public static SourceBufferInfo Empty
        {
            get { return new SourceBufferInfo(); }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsEmpty
        {
            get { return _textBuffer == null; }
        }

        public string Text
        {
            get { return _textBuffer != null ? _textBuffer.CurrentSnapshot.GetText() : ""; }
        }

        public string DocumentPath
        {
            get { return _documentPath; }
        }

        public Visibility DocumentPathVisibility
        {
            get { return _documentPath.Length > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public ITextBuffer TextBuffer
        {
            get { return _textBuffer; }
        }

        public ObservableCollection<SourceBufferInfo> Children { get { return _children; } }

        public SourceBufferInfo(ITextBuffer textBuffer, string name, string documentPath)
        {
            _name = name;
            _documentPath = documentPath ?? "";
            _textBuffer = textBuffer;
            _textBuffer.Changed += OnTextBufferChanged;
        }

        private SourceBufferInfo()
        {
            _name = "";
            _documentPath = "";
        }

        public void Close()
        {
            if (_textBuffer != null)
            {
                _textBuffer.Changed -= OnTextBufferChanged;
            } 
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Text");
        }

        private void OnPropertyChanged(string name)
        {
            var handler = _propertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region INotifyPropertyChanged

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        #endregion
    }
}
