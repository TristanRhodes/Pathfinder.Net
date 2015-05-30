using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Pathfinder.UI.ViewModels;

namespace Pathfinder.UI.ValueConverters
{
    public class CellFillConverter : IValueConverter
    {
        private readonly Brush openNodeBrush = new SolidColorBrush(Colors.LightGray);
        private readonly Brush closedNodeBrush = new SolidColorBrush(Colors.Black);

        private readonly Brush sourceNodeBrush = new SolidColorBrush(Colors.PaleVioletRed);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var node = (NodeViewModel)value;

            if (node == null)
                return openNodeBrush;

            return node.Open ? openNodeBrush : closedNodeBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
