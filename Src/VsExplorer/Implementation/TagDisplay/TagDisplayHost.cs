using Microsoft.VisualStudio.Text;
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
        private readonly IBufferTagAggregatorFactoryService _tagAggregatorService;
        private readonly TagDisplayControl _tagDisplayControl;
        private ITextBuffer _textBuffer;
        private ITagAggregator<ITag> _tagAggregator;

        internal TagDisplayHost(IBufferTagAggregatorFactoryService tagAggregatorService)
        {
            _tagDisplayControl = new TagDisplayControl();
            _tagAggregatorService = tagAggregatorService;
        }

        private void UpdateTextBuffer(ITextBuffer textBuffer)
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
                _tagAggregator = null;
                return;
            }

            _textBuffer = textBuffer;
            _tagAggregator = _tagAggregatorService.CreateTagAggregator<ITag>(textBuffer);
            WeakEventManager<ITagAggregator<ITag>, TagsChangedEventArgs>.AddHandler(_tagAggregator, "TagsChanged", OnTagsChanged);
            WeakEventManager<ITagAggregator<ITag>, BatchedTagsChangedEventArgs>.AddHandler(_tagAggregator, "BatchedTagsChanged", OnBatchedTagsChanged);
            RebuildTags();
        }

        private void RebuildTags()
        {
            _tagDisplayControl.TagGroupCollection.Clear();

            var span = new SnapshotSpan(_textBuffer.CurrentSnapshot, 0, _textBuffer.CurrentSnapshot.Length);
            foreach (var grouping in _tagAggregator.GetTags(span).GroupBy(x => x.Tag.GetType()))
            {
                var snapshot = _textBuffer.CurrentSnapshot;
                // TODO: GetSpans can throw IIRC
                var tagInfos = grouping
                    .SelectMany(x => x.Span.GetSpans(snapshot))
                    .Select(x => new TagInfo(x))
                    .ToList()
                    .AsReadOnly();
                var tagGroup = new TagGroup(grouping.Key.Name, tagInfos);
                _tagDisplayControl.TagGroupCollection.Add(tagGroup);
            }
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
            set { UpdateTextBuffer(value); }
        }

        #endregion
    }
}
