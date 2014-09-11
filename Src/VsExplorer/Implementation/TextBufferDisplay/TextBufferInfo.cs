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
        private readonly string _name;
        private readonly string _documentPath;
        private readonly string _contentType;
        private string _text;
        private event PropertyChangedEventHandler _propertyChanged;

        public static TextBufferInfo Empty
        {
            get { return new TextBufferInfo("", "", "", ""); }
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
            get { return _contentType; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public TextBufferInfo(string name, string documentPath, string contentType, string text)
        {
            _name = name;
            _documentPath = documentPath;
            _contentType = contentType;
            _text = text;
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
