using SQLCreator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Assets
{
    class Algorithms
    {
        private static int priority;
        public static int FindIndex(string[] lines, string[] searchedElements)
        {
            int index = -1;

            foreach (string line in lines)
            {
                index++;
                foreach (string searchedElement in searchedElements)
                {
                    if (line.ToLower().Replace(" ", "").Contains(searchedElement.ToLower().Replace(" ", "")))
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public static void SortTablesOrder(ObservableCollection<TableModel> tables)
        {
            priority = 1;
            SetPriority(tables);

            for (int i = 0; i < tables.Count-1; i++)
            {
                for (int j = i+1; j < tables.Count; j++)
                {
                    if (tables[i].Priority > tables[j].Priority)
                    {
                        var temp = tables[i];
                        tables[i] = tables[j];
                        tables[j] = temp;
                    }
                }
            }
        }

        private static void SetPriority(ObservableCollection<TableModel> tables)
        {
            List<TableModel> sortedTables = tables.ToList();
            foreach (var item in tables.Where(x => !x.FieldInfo.Any(y => y.IsForeignKey)))
            {
                item.Priority = priority++;
                sortedTables.Remove(item);
            }

            var remainingTables = (from X in tables
                                   from Y in sortedTables
                                   where X.TableName == Y.TableName
                                   select X);


            foreach (var table in remainingTables)
            {
                table.Priority = priority++;
            }
        }
    }
}
