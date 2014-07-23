using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsExplorer.Implementation.TreeView
{
    public sealed class TextBufferInfo
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static TextBufferInfo Empty
        {
            get { return new TextBufferInfo() { Name = "", Text = "" }; }
        }
    }

}
