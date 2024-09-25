using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SQLCreator.Assets
{
    public class BoolConverterForHiding : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Az első érték a CheckBox állapota (bool)
            bool isChecked = (bool)values[0];
            var referenceList = values[1] as IList<string>;

            if (isChecked && referenceList != null && referenceList.Count > 0)
            {
                values[2] = referenceList[0];
                return referenceList[0]; // Az első elem visszaadása
            }

            values[2] = null;
            return values[2]; // Ha nincs kiválasztva, visszaadunk null-t
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // A ConvertBack funkció itt csak a kiválasztott elem frissítésére használható
            return new object[] { Binding.DoNothing, Binding.DoNothing, value };
        }

    }
}
