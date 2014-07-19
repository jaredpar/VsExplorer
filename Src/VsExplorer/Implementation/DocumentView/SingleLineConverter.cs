using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VsExplorer.Implementation.DocumentView
{
    public sealed class SingleLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = (string)value;
            var builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                switch (c)
                {
                    case '\r':
                        if (i + 1 < text.Length && text[i + 1] == '\n')
                        {
                            i++;
                        }
                        builder.Append(" ");
                        break;
                    case '\n':
                    case '\u2028':
                    case '\u2029':
                        builder.Append(" ");
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
