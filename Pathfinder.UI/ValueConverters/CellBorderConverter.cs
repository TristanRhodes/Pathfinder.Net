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
    public class CellBorderConverter : IValueConverter
    {
        private readonly Brush inQueueBrush = new SolidColorBrush(Colors.White);
        private readonly Brush notInQueueBrush;

        public CellBorderConverter()
        {
            var converter = new BrushConverter();

            inQueueBrush = new SolidColorBrush(Colors.SteelBlue);
            notInQueueBrush = (SolidColorBrush)converter.ConvertFromString("#FF2C2323");
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return notInQueueBrush;

            var inQueue = (bool)value;

            return inQueue ? inQueueBrush : notInQueueBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
