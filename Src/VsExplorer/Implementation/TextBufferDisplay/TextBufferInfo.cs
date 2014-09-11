using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using System.Windows;
using System.ComponentModel;

namespace VsExplorer.Implementation.TextBufferDisplay
{
    public sealed class TextBufferInfo : INotifyPropertyChanged
    {
        private readonly ITextBuffer _textBuffer;
        private readonly string _name;
        private readonly string _documentPath;
        private string _cachedText;
        private event PropertyChangedEventHandler _propertyChanged;

        public static TextBufferInfo Empty
        {
            get { return new TextBufferInfo(null, "", ""); }
        }

        public string Name
        {
            get { return _name; }
        }

        public string DocumentPath
        {
            get { return _documentPath; }
        }

        public Visibility DocumentPathVisibility
        {
            get { return string.IsNullOrEmpty(_documentPath) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public string ContentType
        {
            get { return _textBuffer != null ? _textBuffer.ContentType.TypeName : ""; }
        }

        public ITextBuffer TextBuffer
        {
            get { return _textBuffer; }
        }

        public string Text
        {
            get
            {
                if (_cachedText == null)
                {
                    _cachedText = _textBuffer != null ? _textBuffer.CurrentSnapshot.GetText() : "";
                }

                return _cachedText;
            }
        }

        public TextBufferInfo(ITextBuffer textBuffer, string name, string documentPath)
        {
            _textBuffer = textBuffer;
            _name = name;
            _documentPath = documentPath;

            if (_textBuffer != null)
            {
                WeakEventManager<ITextBuffer, TextContentChangedEventArgs>.AddHandler(_textBuffer, "Changed", OnTextBufferChanged);
            }
        }

        public void Close()
        {
            if (_textBuffer != null)
            {
                WeakEventManager<ITextBuffer, TextContentChangedEventArgs>.RemoveHandler(_textBuffer, "Changed", OnTextBufferChanged);
            }
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            _cachedText = null;
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
