using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class NamedBufferInfo
    {
        public string Name { get; set; }

        public SourceBufferInfo SourceBufferInfo { get; set; }
    }
}
