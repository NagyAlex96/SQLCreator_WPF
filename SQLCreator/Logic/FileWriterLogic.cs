using SQLCreator.Assets;
using SQLCreator.Interfaces;
using SQLCreator.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace SQLCreator.Logic
{
    public class FileWriterLogic : IFileWriterLogic
    {
        //TODO: szépíteni
        private StreamWriter _sWriter;
        private DataBaseModel _DBModel;

        public FileWriterLogic(DataBaseModel dbModel)
        {
            this._DBModel = dbModel;
        }
        public void SQLWriter()
        {
            using (this._sWriter = new StreamWriter($"{this._DBModel.OutPutDestination}\\{_DBModel.NameOfDb.TrimEnd()}.sql", false, Encoding.UTF8))
            {
                this._DBModel.TablesInfo = new ObservableCollection<TableModel>(this._DBModel.TablesInfo.OrderBy(x => x.Priority)); //csak az insert-nél legyen orderby?
                CreateDBase();
                CreateTables();
                AlterTable();
                Insert();
            }
        }

        /// <summary>
        /// Create Database parancs
        /// </summary>
        private void CreateDBase()
        {
            this._sWriter.WriteLine($"CREATE DATABASE {this._DBModel.NameOfDb}\n" +
                $"DEFAULT CHARACTER SET utf8\n" +
                $"COLLATE utf8_hungarian_ci;\n" +
                $"use {this._DBModel.NameOfDb.TrimEnd()};\n");
        }

        /// <summary>
        /// Create table parancs
        /// </summary>
        private void CreateTables()
        {
            foreach (TableModel tableInfo in this._DBModel.TablesInfo)
            {
                //táblák létrehozása
                this._sWriter.WriteLine($"CREATE TABLE {tableInfo.TableName}");
                this._sWriter.WriteLine("(");
                this._sWriter.WriteLine(FieldNamesAndPKeyWriter(tableInfo.FieldInfo) + ");\n");
            }
        }

        /// <summary>
        /// Mezőnevek és elsődleges kulcs(ok) kiírása
        /// </summary>
        /// <param name="fields">Rendelkezésre álló mezők</param>
        /// <returns>Kiírandó SQL parancs</returns>
        private string FieldNamesAndPKeyWriter(ObservableCollection<FieldModel> fields)
        {
            string outPut = "";

            foreach (FieldModel item in fields)
            {
                outPut += $"\t{item.FieldName} {item.TypeOfField},\n";
            }
            FieldModel pk = fields.Where(x => x.IsPrimaryKey).FirstOrDefault();
            outPut += $"\t\tPRIMARY KEY ({pk.FieldName})\n";


            return outPut;
        }

        /// <summary>
        /// Táblák összekapcsolási parancsa
        /// </summary>
        private void AlterTable()
        {
            foreach (TableModel tableInfo in this._DBModel.TablesInfo)
            {
                foreach (FieldModel fieldInfo in tableInfo.FieldInfo)
                {
                    if (fieldInfo.IsForeignKey && fieldInfo.ReferenceTo != null)
                    {
                        this._sWriter.WriteLine($"ALTER TABLE {tableInfo.TableName}");
                        this._sWriter.WriteLine($"ADD CONSTRAINT fk_{fieldInfo.FieldName}");
                        this._sWriter.WriteLine($"FOREIGN KEY ({fieldInfo.FieldName}) REFERENCES {fieldInfo.ReferenceTo};\n");
                    }
                }
            }
        }


        /// <summary>
        /// Adatok bemásolása parancs
        /// </summary>
        private void Insert()
        {
            foreach (TableModel tableInfo in this._DBModel.TablesInfo) //táblák
            {
                //adatok bemásolása
                string insert = $"INSERT INTO {tableInfo.TableName} (";

                for (int i = 0; i < tableInfo.FieldInfo.Count; i++) //mezők
                {
                    if (i == tableInfo.FieldInfo.Count - 1) //utolsó táblanév kiirása (vessző nem kell, mert nincsen több tábla)
                    {
                        insert += $"{tableInfo.FieldInfo[i].FieldName}";
                        continue;
                    }
                    insert += $"{tableInfo.FieldInfo[i].FieldName}, ";
                }

                insert += $")\nVALUES";
                this._sWriter.WriteLine(insert); //kiírjuk, hogy hova fognak bekerülni az adatok -> INSERT INTO {TÁBLA NEVE} (MEZŐ1, MEZŐ2...))

                //egy táblán belül a mezőkhöz tartozó adatok száma egyenlő (MEZŐ1.Length = MEZŐ2.Length)
                int maxLine = tableInfo.FieldInfo[0].FieldValue.Count;
                for (int i = 0; i < maxLine; i++)
                {
                    string data = "(";

                    for (int j = 0; j < tableInfo.FieldInfo.Count; j++)
                    {
                        string fValuesWriteout = "";

                        if (tableInfo.FieldInfo[j].TypeOfField.Contains(FieldTypes.DEFAULT_VALUE) || tableInfo.FieldInfo[j].TypeOfField.Contains("DATE"))
                        {
                            if (tableInfo.FieldInfo[j].FieldValue[i] != "")
                            {
                                fValuesWriteout += $"\"{tableInfo.FieldInfo[j].FieldValue[i]}\"";
                            }
                        }
                        else
                        {
                            fValuesWriteout += tableInfo.FieldInfo[j].FieldValue[i];
                        }

                        //Amennyiben nincs tárolt érték, akkor NULL-ként jelenjen meg
                        if (fValuesWriteout == "")
                        {
                            fValuesWriteout += $"NULL";
                        }

                        if (j != tableInfo.FieldInfo.Count - 1) //nem az utolsó mezőnél vagyunk
                        {
                            data += $"{fValuesWriteout}, ";
                        }
                        else //utolsó mező -> nincs vessző a végén pl: (1, 1, "Budapest")
                        {
                            data += $"{fValuesWriteout}";
                        }
                    }

                    if (i == maxLine - 1) //utolsó adat kerül kiírásra pl.: (20, 3, "Zala"); <-- pontosvessző kell
                    {
                        this._sWriter.WriteLine($"{data});\n");
                    }
                    else
                    {
                        this._sWriter.WriteLine($"{data}),");
                    }
                }

            }

        }
    }
}
