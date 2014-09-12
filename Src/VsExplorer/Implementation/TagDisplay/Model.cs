using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TagDisplay
{
    public sealed class TagGroup
    {
        private readonly string _name;
        private readonly ReadOnlyCollection<TagInfo> _tagInfoCollection;

        public string Name
        {
            get { return _name; }
        }

        public ReadOnlyCollection<TagInfo> TagInfoCollection
        {
            get { return _tagInfoCollection; }
        }

        public TagGroup(string name, ReadOnlyCollection<TagInfo> tagInfoCollection)
        {
            _name = name;
            _tagInfoCollection = tagInfoCollection;
        }
    }

    public sealed class TagInfo
    {
        private readonly SnapshotSpan _span;

        public string Span
        {
            get { return _span.ToString(); }
        }

        public string Text
        {
            get { return _span.GetText(); }
        }

        public string DisplayLine
        {
            get { return string.Format("[{0} - {1}) - {2}", _span.Start.Position, _span.End.Position, _span.GetText()); }
        }

        public TagInfo(SnapshotSpan span)
        {
            _span = span;
        }
    }
}
