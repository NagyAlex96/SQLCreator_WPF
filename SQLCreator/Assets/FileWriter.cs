using SQLCreator.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Assets
{
    public class FileWriter : IFileWriter
    {
        //TODO:
        //kód átnézése + felkommentezés
        //legyen olyan megnyíló ablak, ahol lehet módosítani az idegenkulcsot, elsődleges kulcsot, egymás közötti referenciát

        readonly IDBCreator _dbCreator;
        StreamWriter _sWriter;
        public FileWriter(IDBCreator dBCreator)
        {
            this._dbCreator = dBCreator;
        }

        public void SQLWriter(string savePath)
        {
            using (this._sWriter = new StreamWriter($"{savePath}\\{this._dbCreator.NameOfDB.TrimEnd()}.sql", false, Encoding.UTF8))
            {
                CreateDBase();
                CreateTables();
                AlterTable();
                Insert();
            }
        }

        private void CreateDBase()
        {
            this._sWriter.WriteLine($"CREATE DATABASE {this._dbCreator.NameOfDB}\n" +
                $"DEFAULT CHARACTER SET utf8\n" +
                $"COLLATE utf8_hungarian_ci;\n" +
                $"use {this._dbCreator.NameOfDB.TrimEnd()};\n");
        }

        private void CreateTables()
        {
            foreach (ITable tableInfo in this._dbCreator.Tables)
            {
                //táblák létrehozása
                this._sWriter.WriteLine($"CREATE TABLE {tableInfo.TableName}");
                this._sWriter.WriteLine("(");
                this._sWriter.WriteLine(FieldNamesAndPKeyWriter(tableInfo.FieldData) + ");\n");

            }
        }

        private string FieldNamesAndPKeyWriter(IField[] field)
        {
            string outPut = "";

            foreach (var item in field)
            {
                outPut += $"\t{item.ToString()},\n";
            }
            IField pk = field.Where(x => x.IsPrimaryKey).FirstOrDefault();
            outPut += $"\t\tPRIMARY KEY ({pk.FieldName})\n";


            return outPut;
        }

        private void AlterTable()
        {
            foreach (ITable tableInfo in this._dbCreator.Tables)
            {
                foreach (IField fieldInfo in tableInfo.FieldData)
                {
                    if (fieldInfo.IsForeignKey)
                    {
                        this._sWriter.WriteLine($"ALTER TABLE {tableInfo.TableName}");
                        this._sWriter.WriteLine($"ADD CONSTRAINT fk_{fieldInfo.FieldName}");
                        this._sWriter.WriteLine($"FOREIGN KEY ({fieldInfo.FieldName}) REFERENCES {fieldInfo.ReferenceTo};\n");
                    }
                }
            }
        }

        private void Insert()
        {
            List<ITable> tables = SortedList();

            foreach (ITable tableInfo in tables)
            {
                //adatok bemásolása
                string insert = $"INSERT INTO {tableInfo.TableName} (";

                for (int i = 0; i < tableInfo.FieldData.Length; i++)
                {
                    if (i == tableInfo.FieldData.Length - 1)
                    {
                        insert += $"{tableInfo.FieldData[i].FieldName}";
                        continue;
                    }
                    insert += $"{tableInfo.FieldData[i].FieldName}, ";
                }

                insert += $")\nVALUES";
                this._sWriter.WriteLine(insert);

                int maxLine = tableInfo.FieldData[0].FieldValue.Length;
                for (int i = 0; i < maxLine; i++)
                {
                    string data = "(";

                    for (int j = 0; j < tableInfo.FieldData.Length; j++)
                    {
                        string s = (tableInfo.FieldData[j].TypeOfField == Field.DEFAULT_VALUE ?
                            $"\"{tableInfo.FieldData[j].FieldValue[i]}\""
                            : tableInfo.FieldData[j].FieldValue[i]);

                        if(s == "")
                        {
                            s += $"\"\"";
                        }

                        if (j != tableInfo.FieldData.Length - 1)
                        {
                            data += $"{s}, ";
                        }
                        else
                        {
                            data += $"{s}";
                        }
                    }

                    if (i == maxLine - 1)
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

        private List<ITable> SortedList()
        {
            List<ITable> tables = new List<ITable>();


            var first = this._dbCreator.Tables.Where(x => !x.HasForeignKey).FirstOrDefault();
            int i = 0;
            if (first != null)
            {
                tables.Add(first);
            }

            while (i < this._dbCreator.Tables.Length)
            {
                var v = (from X in this._dbCreator.Tables[i].FieldData
                         from Y in tables
                         from Y2 in Y.FieldData
                         where X.IsForeignKey && X.FieldName == Y2.FieldName
                         select this._dbCreator.Tables[i]).FirstOrDefault();

                if (v != null)
                {
                    tables.Add(v);
                }
                i++;
            }
            return tables;
        }
    }
}
