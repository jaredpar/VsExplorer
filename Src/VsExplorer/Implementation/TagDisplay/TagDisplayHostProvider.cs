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
        private readonly IBufferTagAggregatorFactoryService _tagAggregatorService;

        [ImportingConstructor]
        internal TagDisplayHostProvider(IBufferTagAggregatorFactoryService tagAggregatorService)
        {
            _tagAggregatorService = tagAggregatorService;
        }

        ITagDisplayHost ITagDisplayHostProvider.Create()
        {
            return new TagDisplayHost(_tagAggregatorService);
        }
    }
}
