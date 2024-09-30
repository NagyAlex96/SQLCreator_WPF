using iTextSharp.text.pdf.qrcode;
using SQLCreator.Assets;
using SQLCreator.Interfaces;
using SQLCreator.Model;
using System.Collections.ObjectModel;
using System.Configuration;

namespace SQLCreator.Logic
{
    public class DBCreatorLogic : IDbCreatorLogic
    {
        //TODO:
        //2010_május - 12-es feladat
        private IFileReaderLogic _readerLogic;
        public DBCreatorLogic()
        {
            this._readerLogic = new FileReaderLogic();
        }

        public DataBaseModel CreateDataBase(in DataBaseModel DBModel)
        {
            DataBaseModel NewDataBase = DBModel;
            ObservableCollection<TableModel> tempTables = new ObservableCollection<TableModel>();


            string[] PdfLinesData = this._readerLogic.PdfReader(NewDataBase);
            NewDataBase.NameOfDb = SetNameOfDb(PdfLinesData[0].Split('\n'));

            for (int i = 0; i < NewDataBase.TxtFileName.Count; i++)
            {
                TableModel table = new TableModel();
                table.TableName = NewDataBase.TxtFileName[i];
                table.FieldInfo = TxtDataProcessing(_readerLogic.TxtReader(NewDataBase)[i]);
                tempTables.Add(table);
            }

            PdfDataProcessing(tempTables, PdfLinesData, int.Parse(NewDataBase.PdfLineNum) + 1);


            SetReferencesAndFkeys(tempTables);

            Algorithms.SortTablesOrder(tempTables);
            NewDataBase.TablesInfo = tempTables;
            return NewDataBase;
        }

        /// <summary>
        /// Adatbázis nevének beállítása
        /// </summary>
        /// <param name="pdfLines">PDF Sorok</param>
        /// <returns>Adatbázis neve</returns>
        private string SetNameOfDb(in string[] pdfLines)
        {
            int i = 0;
            bool isDigit = false;

            do
            {
                isDigit = char.IsDigit(pdfLines[i], 0);
                i++;
            } while (i < pdfLines.Length && !isDigit);

            return pdfLines[i - 1].Substring(3);
        }

        /// <summary>
        /// Hivatkozások beállítása, táblák összekapcsolása
        /// </summary>
        /// <param name="tableModels">Táblák</param>
        private void SetReferencesAndFkeys(ObservableCollection<TableModel> tableModels)
        {
            //rendelkezésre álló táblák
            foreach (var selectedTable in tableModels)
            {
                List<TableModel> otherTables = tableModels.ToList(); //összes tábla
                otherTables.Remove(selectedTable); //kiválasztott tábla kivétele

                foreach (var otherTable in otherTables)
                {
                    //kiválasztjuk azt a mezőt, ahol a mezőnevek egyenlőek és sem az "A" táblának sem pedig a "B" táblának a mezője nem idegenkulcs
                    var matchResult = (from otherTableFieldInfo in otherTable.FieldInfo
                                       from selectedTableFieldInfo in selectedTable.FieldInfo
                                       where otherTableFieldInfo.FieldName == selectedTableFieldInfo.FieldName && !(otherTableFieldInfo.IsForeignKey || selectedTableFieldInfo.IsForeignKey)
                                       select (otherTableFieldInfo, selectedTableFieldInfo)).FirstOrDefault();

                    //amennyiben nincs ilyen mező
                    if (matchResult.otherTableFieldInfo == null || matchResult.selectedTableFieldInfo == null)
                    {
                        continue;
                    }

                    //amennyiben mind a két mező elsődlegeskulcs (furcsa)
                    if (matchResult.selectedTableFieldInfo.IsPrimaryKey && matchResult.otherTableFieldInfo.IsPrimaryKey)
                    {
                        //összehasonlítás eredménye
                        int compareResult = OrderComparer(matchResult.otherTableFieldInfo, matchResult.selectedTableFieldInfo);
                        switch (compareResult)
                        {
                            //kiválasztott tábla mezőjének értékei előbb vannak, mint a másik tábla mezőjének elemei
                            case -1: SetReference(matchResult.selectedTableFieldInfo, otherTable, matchResult.selectedTableFieldInfo); break;
                            case 0:
                                //amennyiben egyenlő sorrendben vannak a táblák mezőjének értékei akkor, a sorrend lényegtelen, DE kiválasztjuk azt ahol, nincs még idegenkulcs
                                if (otherTable.FieldInfo.Any(x => x.IsForeignKey))
                                {
                                    SetReference(matchResult.selectedTableFieldInfo, otherTable, matchResult.selectedTableFieldInfo);
                                }
                                else
                                {
                                    SetReference(matchResult.otherTableFieldInfo, selectedTable, matchResult.selectedTableFieldInfo);
                                }
                                break;
                            case 1: SetReference(matchResult.otherTableFieldInfo, selectedTable, matchResult.selectedTableFieldInfo); break;
                            default:
                                break;
                        }
                    }
                    else //nem elsődleges kulcs mind a kettő
                    {
                        if (otherTable.FieldInfo.Any(x => x.IsPrimaryKey))
                        {
                            SetReference(matchResult.selectedTableFieldInfo, otherTable, matchResult.selectedTableFieldInfo);
                        }
                        else
                        {
                            SetReference(matchResult.otherTableFieldInfo, selectedTable, matchResult.selectedTableFieldInfo);
                        }
                    }
                }
                SetRemainingReference(selectedTable.FieldInfo, otherTables);
            }

        }
        private void SetRemainingReference(ObservableCollection<FieldModel> fieldModels, List<TableModel> tableRef)
        {
            foreach (var model in fieldModels)
            {
                foreach (var table in tableRef)
                {
                    SetReference(model, table, table.FieldInfo);
                }
            }
        }

        private void SetReference(FieldModel fModel, TableModel tableRef, ObservableCollection<FieldModel> fieldRefs)
        {
            foreach(var fRefs in fieldRefs)
            {
                SetReference(fModel, tableRef, fRefs, fModel.IsForeignKey ? true : false);
            }
        }
        private void SetReference(FieldModel fModel, TableModel tableRef, FieldModel fieldRef, bool setFKey = true)
        {
            fModel.References.Add(ReferencTo(tableRef, fieldRef));
            fModel.IsForeignKey = setFKey;
        }

        private string ReferencTo(TableModel tModel, FieldModel fModel)
        {
            return $"{tModel.TableName} ({fModel.FieldName})";
        }

        /// <summary>
        /// Megvizsgáljuk, hogy az elemek növekvő sorrendben követik-e egymást
        /// </summary>
        /// <param name="fieldA">Mit hasonlítunk</param>
        /// <param name="fieldB">Mihez hasonlítunk</param>
        /// <returns>CompareTo eredményét adja vissza</returns>
        private int OrderComparer(FieldModel fieldA, FieldModel fieldB)
        {
            int orderNum = 0;
            int index = 0;
            do
            {
                orderNum = fieldA.FieldValue[index++].CompareTo(fieldB.FieldValue[index++]); //egyenlő-e a két érték

                if (fieldA.FieldValue[index - 1].CompareTo(fieldA.FieldValue[index]) > -1) //az értékek sorrendben követik-e egymást az "A" mező esetén
                {
                    orderNum = 1;
                }
                else if (fieldB.FieldValue[index - 1].CompareTo(fieldB.FieldValue[index]) > -1)//az értékek sorrendben követik-e egymást az "B" mező esetén
                {
                    orderNum = -1;
                }
            } while (index < fieldA.FieldValue.Count && orderNum == 0);

            return orderNum;
        }

        #region PDF feldolgozás

        /// <summary>
        /// Egyéb kapcsolatok és adatok beállítása a pdf-ben található adatok alapján
        /// </summary>
        /// <param name="allTablesModel">Rendelkezésünkre álló táblák</param>
        /// <param name="pdfDataOnPage">PDF adott oldalán található adatok <c>soronként</c></param>
        /// <param name="index">Hányadik sortól indul a táblák rész</param>
        private void PdfDataProcessing(ObservableCollection<TableModel> allTablesModel, in string[] pdfDataOnPage, in int index)
        {
            foreach (var table in allTablesModel)
            {
                foreach (var dataOnPage in pdfDataOnPage) //ez a foreach arra kell, ha az információk több oldalon lennének
                {
                    SetFields(allTablesModel, table, dataOnPage.Split('\n'), index);
                }
            }
        }

        /// <summary>
        /// Mezők beállítása
        /// </summary>
        /// <param name="allTablesModel">Összes tábla</param>
        /// <param name="settableTable">Beállítandó tábla</param>
        /// <param name="pdfDataOnPage">PDF adott oldalán található adatok <c>soronként</c></param>
        /// <param name="fromIndex">Hányadik sortól indul a táblák rész</param>
        private void SetFields(ObservableCollection<TableModel> allTablesModel, TableModel settableTable, string[] pdfDataOnPage, int fromIndex)
        {
            string fullTableName = $"{settableTable.TableName} (";

            for (int i = 0; i < settableTable.FieldInfo.Count; i++)
            {
                if (i == settableTable.FieldInfo.Count - 1)
                {
                    fullTableName += $"{settableTable.FieldInfo[i].FieldName})";
                }
                else
                {
                    fullTableName += $"{settableTable.FieldInfo[i].FieldName}, ";
                }
            }

            fromIndex = FindTableLineIndex(fullTableName, pdfDataOnPage, fromIndex);
            //fromIndex = FindTableLineIndex(settableTable.FieldInfo[0].FieldName, pdfDataOnPage, fromIndex);
            SetTypeOfFieldAndPKeys(fromIndex, (fromIndex + settableTable.FieldInfo.Count + 1), settableTable.FieldInfo, pdfDataOnPage);
        }

        /// <summary>
        /// Megkeresi a pdf-ben a táblához tartozó sort
        /// </summary>
        /// <param name="searchedTable">Tábla neve</param>
        /// <param name="lines">Pdf-ben található sorok</param>
        /// <param name="startIndex">Indulóindex</param>
        /// <returns><c>InvalidOperationException</c> amennyiben nem található az adott táblához leírás a pdf-ben</returns>
        private int FindTableLineIndex(in string searchedTable, in string[] lines, in int startIndex)
        {
            for (int i = startIndex; i < lines.Length; i++)
            {
                if (lines[i].ToLower().Contains(searchedTable.ToLower()))
                {
                    return i;
                }
            }
            return -1;
            throw new InvalidOperationException("The table was not found!");
        }

        /// <summary>
        /// Elsődleges kulcs és mezőtípus beállítása
        /// </summary>
        /// <param name="fromIndex">Hányadik sortól található meg a pdf-ben</param>
        /// <param name="toIndex">Hányadik sorig tart a pdf-ben</param>
        /// <param name="fieldModels">Táblához tartozó mezők</param>
        /// <param name="pdfDataOnPage">PDF adott oldalán található adatok <c>soronként</c></param>
        private void SetTypeOfFieldAndPKeys(int fromIndex, int toIndex, ObservableCollection<FieldModel> fieldModels, string[] pdfDataOnPage)
        {
            int j = fromIndex + 1;
            for (int i = 0; i < fieldModels.Count; i++) //végigmegyünk a mezőkön
            {
                while (j < toIndex && fieldModels[i].TypeOfField == null)
                {
                    if (!fieldModels.Any(field => field.IsPrimaryKey) && pdfDataOnPage[j].Contains("ez a kulcs"))
                    {
                        fieldModels[i].IsPrimaryKey = true;
                    }
                    fieldModels[i].TypeOfField = FieldTypes.SetFieldType(pdfDataOnPage[j]);
                    j++;
                }
            }
        }

        #endregion

        #region Txt-s feldolgozás

        /// <summary>
        /// Txt-ben lévő adatok feldolgozása
        /// </summary>
        /// <param name="txtFileLines">Adott fájlban található sorok (adatok)</param>
        /// <returns>Mezőkhöz tartozó név + adatok</returns>
        private ObservableCollection<FieldModel> TxtDataProcessing(in string[] txtFileLines)
        {
            string[] fieldData = txtFileLines[0].Split('\t'); //kezdetben a 0-ik sorból kiszedjük a mezők neveit
            ObservableCollection<FieldModel> fieldModels = new ObservableCollection<FieldModel>();
            SetFieldNames(fieldModels, fieldData);

            for (int i = 1; i < txtFileLines.Length; i++)
            {
                fieldData = txtFileLines[i].Split('\t'); //itt már nem a mezők nevei lesznek, hanem a hozzájuk tartozó adatok
                SetFieldValues(fieldModels, fieldData);
            }

            return fieldModels;
        }

        /// <summary>
        /// Beállítjuk az mezőkhöz tartozó értékeket soronként
        /// </summary>
        /// <param name="fieldModels">Elérhető mezők</param>
        /// <param name="lineWithData">Egy sornyi adat</param>
        private void SetFieldValues(ObservableCollection<FieldModel> fieldModels, in string[] lineWithData)
        {
            for (int i = 0; i < fieldModels.Count; i++)
            {
                if (i >= lineWithData.Length) //amennyiben mondjuk a txt egyik sorából hiányzik a megfelelő mennyiségű adat
                {
                    fieldModels[i].FieldValue.Add("");
                    continue;
                }
                //tryParse azért kell, mert a double típusú értékeket vesszővel van megadva, de az SQL (is) ponttal értelmezik
                if (double.TryParse(lineWithData[i].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _))
                {
                    fieldModels[i].FieldValue.Add(lineWithData[i].Replace(',', '.'));
                    continue;
                }
                fieldModels[i].FieldValue.Add(lineWithData[i]);
            }
        }

        /// <summary>
        /// Beállítjuk a mezőknek a nevét
        /// </summary>
        /// <param name="fieldModels">Mezők</param>
        /// <param name="fieldNamesFromTxt">Txt-ből kiszedett mezőnevek</param>
        private void SetFieldNames(ObservableCollection<FieldModel> fieldModels, in string[] fieldNamesFromTxt)
        {
            for (int i = 0; i < fieldNamesFromTxt.Length; i++)
            {
                FieldModel fModel = new FieldModel();
                fModel.FieldName = fieldNamesFromTxt[i];
                fieldModels.Add(fModel);
            }
        }

        #endregion
    }
}
