using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class SourceBufferInfo : INotifyPropertyChanged
    {
        private readonly ITextBuffer _textBuffer;
        private readonly string _name;
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

        public ITextBuffer TextBuffer
        {
            get { return _textBuffer; }
        }

        public ObservableCollection<SourceBufferInfo> Children { get { return _children; } }

        public SourceBufferInfo(string name, ITextBuffer textBuffer)
        {
            _name = name;
            _textBuffer = textBuffer;
            _textBuffer.Changed += OnTextBufferChanged;
        }

        private SourceBufferInfo()
        {
            _name = "";
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
