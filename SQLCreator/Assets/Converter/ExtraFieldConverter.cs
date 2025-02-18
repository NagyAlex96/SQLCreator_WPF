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
            //value értékei
            //0. index: az első érték egy bool, ami megmondja, hogy szükséges-e egy újabb mező mondjuk az id-nak
            //1. index az összes tábla
            //2. index az a tábla, ahova az extra mezőt fogjuk szúrni

            if (values[0] is bool isExtraFieldNeeded && values[1] != null && (values[1] as ICollection).Count > 0 && values[2] != null)
            {
                values[0] = !isExtraFieldNeeded;
                var tableModelCollection = values[1] as ObservableCollection<TableModel>;
                var tableModel = values[2] as TableModel;

                if (isExtraFieldNeeded && !tableModel.FieldInfo.Where(x => x.IsExtraField).Any())
                {
                    DBCreatorLogic.AddExtraField(tableModelCollection, tableModel);
                }
                else
                {
                    DBCreatorLogic.RemoveExtraField(tableModel);
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
