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
            // A második érték a ComboBox adatai, amikből az első elemet vesszük, ha létezik
            var referenceList = values[1] as IList<string>;
            string selectedReference = values[2] as string;

            bool ok = isChecked && referenceList != null && referenceList.Count > 0;

            if (ok)
            {
                selectedReference = referenceList[0];
                return referenceList[0]; // Az első elem visszaadása
            }

            return selectedReference ?? string.Empty; // Ha nincs kiválasztva, visszaadunk null-t
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // A ConvertBack funkció itt csak a kiválasztott elem frissítésére használható
            return new object[] { Binding.DoNothing, Binding.DoNothing, value };
        }

    }
}
