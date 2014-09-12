using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TagDisplay
{
    [Export(typeof(ITagDisplayHostProvider))]
    internal sealed class TagDisplayHostProvider : ITagDisplayHostProvider
    {
        private readonly IBufferTagAggregatorFactoryService _bufferTagAggregatorService;
        private readonly IViewTagAggregatorFactoryService _viewTagAggregatorService;

        [ImportingConstructor]
        internal TagDisplayHostProvider(IBufferTagAggregatorFactoryService bufferTagAggregatorService, IViewTagAggregatorFactoryService viewTagAggregatorService)
        {
            _bufferTagAggregatorService = bufferTagAggregatorService;
            _viewTagAggregatorService = viewTagAggregatorService;
        }

        ITagDisplayHost ITagDisplayHostProvider.Create()
        {
            return new TagDisplayHost(_bufferTagAggregatorService, _viewTagAggregatorService);
        }
    }
}
