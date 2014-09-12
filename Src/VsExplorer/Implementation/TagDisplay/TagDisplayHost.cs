using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VsExplorer.Implementation.TagDisplay
{
    internal sealed class TagDisplayHost : ITagDisplayHost
    {
        private readonly IBufferTagAggregatorFactoryService _bufferTagAggregatorService;
        private readonly IViewTagAggregatorFactoryService _viewTagAggregatorService;
        private readonly TagDisplayControl _tagDisplayControl;
        private ITextView _textView;
        private ITextBuffer _textBuffer;
        private ITagAggregator<ITag> _tagAggregator;

        internal TagDisplayHost(IBufferTagAggregatorFactoryService bufferTagAggregatorService, IViewTagAggregatorFactoryService viewTagAggregatorService)
        {
            _tagDisplayControl = new TagDisplayControl();
            _bufferTagAggregatorService = bufferTagAggregatorService;
            _viewTagAggregatorService = viewTagAggregatorService;
        }

        private void UpdateTextBuffer(ITextBuffer textBuffer, ITextView textView)
        {
            if (_textBuffer != null)
            {
                Debug.Assert(_tagAggregator != null);
                WeakEventManager<ITagAggregator<ITag>, TagsChangedEventArgs>.RemoveHandler(_tagAggregator, "TagsChanged", OnTagsChanged);
                WeakEventManager<ITagAggregator<ITag>, BatchedTagsChangedEventArgs>.RemoveHandler(_tagAggregator, "BatchedTagsChanged", OnBatchedTagsChanged);
                _tagAggregator.Dispose();
            }

            if (textBuffer == null)
            {
                _textBuffer = null;
                _textView = null;
                _tagAggregator = null;
                return;
            }

            _textBuffer = textBuffer;
            _textView = textView;
            _tagAggregator = textView == null
                ? _bufferTagAggregatorService.CreateTagAggregator<ITag>(textBuffer)
                : _viewTagAggregatorService.CreateTagAggregator<ITag>(textView);
            WeakEventManager<ITagAggregator<ITag>, TagsChangedEventArgs>.AddHandler(_tagAggregator, "TagsChanged", OnTagsChanged);
            WeakEventManager<ITagAggregator<ITag>, BatchedTagsChangedEventArgs>.AddHandler(_tagAggregator, "BatchedTagsChanged", OnBatchedTagsChanged);
            RebuildTags();
        }

        private void RebuildTags()
        {
            _tagDisplayControl.TagGroupCollection.Clear();

            var span = new SnapshotSpan(_textBuffer.CurrentSnapshot, 0, _textBuffer.CurrentSnapshot.Length);
            foreach (var grouping in _tagAggregator.GetTags(span).GroupBy(GetTagGroupName))
            {
                var snapshot = _textBuffer.CurrentSnapshot;
                // TODO: GetSpans can throw IIRC
                var tagInfos = grouping
                    .SelectMany(x => x.Span.GetSpans(snapshot))
                    .Select(x => new TagInfo(x))
                    .ToList()
                    .AsReadOnly();
                var tagGroup = new TagGroup(grouping.Key, tagInfos);
                _tagDisplayControl.TagGroupCollection.Add(tagGroup);
            }
        }

        private static string GetTagGroupName(IMappingTagSpan<ITag> span)
        {
            var tag = span.Tag;
            var classificationTag = tag as IClassificationTag;
            if (classificationTag != null)
            {
                return string.Format("Classification ({0})", classificationTag.ClassificationType.Classification);
            }

            var textMarkerTag = tag as ITextMarkerTag;
            if (textMarkerTag != null)
            {
                return string.Format("TextMarkerTag ({0})", textMarkerTag.Type);
            }

            return tag.GetType().Name;
        }

        private void OnTagsChanged(object sender, TagsChangedEventArgs e)
        {
            RebuildTags();
        }

        private void OnBatchedTagsChanged(object sender, BatchedTagsChangedEventArgs e)
        {
            RebuildTags();
        }

        #region ITagDisplayHost

        UIElement ITagDisplayHost.Visual
        {
            get { return _tagDisplayControl; }
        }

        ITextBuffer ITagDisplayHost.TextBuffer
        {
            get { return _textBuffer; }
            set { UpdateTextBuffer(value, null); }
        }

        ITextView ITagDisplayHost.TextView
        {
            get { return _textView; }
            set { UpdateTextBuffer(value != null ? value.TextBuffer : null, value); }
        }

        #endregion
    }
}
