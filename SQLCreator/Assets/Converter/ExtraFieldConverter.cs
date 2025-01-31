using SQLCreator.Logic;
using SQLCreator.Model;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace SQLCreator.Assets.Converter
{
    public class ExtraFieldConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool isExtraFieldNeeded && values[1] != null && (values[1] as ICollection).Count > 0 && values[2] != null)
            {
                values[0] = !isExtraFieldNeeded;
                var tableModelCollection = values[1] as ObservableCollection<TableModel>;
                var tableModel = values[2] as TableModel;

                if (!isExtraFieldNeeded)
                {
                    DBCreatorLogic.RemoveExtraField(tableModel);
                }
                else
                {
                    DBCreatorLogic.AddExtraField(tableModelCollection, tableModel);
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
