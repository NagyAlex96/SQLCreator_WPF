using SQLCreator.Assets;
using SQLCreator.Interfaces;
using System.Linq;

namespace SQLCreator.Logic
{
    public class DBCreatorLogic : IDbCreatorLogic
    {
        IDBCreator _dbCreator;
        /// <summary>
        /// A táblák rész a pdf hányadik sorától indul
        /// </summary>
        private int _tableStartLineIndex;
        public IDBCreator DbCreator => this._dbCreator;

        public DBCreatorLogic()
        {
            this._dbCreator = new DBCreator();
        }

        public void CreateDataBase(in string[] pdfDataOnPage, in List<string[]> txtFileLines, in string txtFileName)
        {
            this._dbCreator.SetupTables(txtFileLines.Count);

            //pdf sorai
            string[] pdfLines = pdfDataOnPage.Select(x => x.Split('\n')).FirstOrDefault();
            this._tableStartLineIndex = pdfLines
                    .Select((line, index) => new { line, index })
                    .Where(x => x.line.Contains("táblák:"))
                    .Select(x => x.index)
                    .FirstOrDefault(-1);

            ITable[] tempTables = new ITable[txtFileLines.Count];
            for (int i = 0; i < tempTables.Length; i++)
            {
                ITable table = new Table(txtFileName.Split('\n')[i], txtFileLines[i][0].Split('\t').Length); //mindegyik txt-hez tartozó mezőnek ugyanannyi sora lesz
                IField[] fields = TxtDataProcessing(txtFileLines[i]);
                AddFieldToTable(table, fields);
                tempTables[i] = table;
            }

            for (int i = 0; i < tempTables.Length; i++) //0-1, 1-2, 2-3, 3-4
            {


                PdfDataProcessing(tempTables[i], tempTables, tempTables[i].FieldData, pdfDataOnPage);


                this._dbCreator.AddTable(tempTables[i]);
            }

            SetReferencesAndFkeys();
            ;
        }

        private void SetReferencesAndFkeys()
        {
            foreach (ITable tableInfo in this._dbCreator.Tables)
            {
                List<ITable> otherTables = this._dbCreator.Tables.ToList();
                otherTables.Remove(tableInfo);

                foreach (ITable otherTablesInfo in otherTables)
                {
                    var selectedField = (from x in otherTablesInfo.FieldData
                                         from y in tableInfo.FieldData
                                         where x.FieldName == y.FieldName && !(x.IsForeignKey || y.IsForeignKey)
                                         select y).FirstOrDefault();

                    if (selectedField != null)
                    {
                        selectedField.SetupReferenceTo($"{otherTablesInfo.TableName} ({selectedField.FieldName})");
                        selectedField.SetupIsForeignKey(true);
                    }
                }
            }
        }

        /// <summary>
        /// Egyéb kapcsolatok és adatok beállítása a pdf-ben található adatok alapján
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="pdfDataOnPage">A pdf adott oldalán található sorok (adatok)</param>
        private void PdfDataProcessing(in ITable table, in ITable[] tables, IField[] fields, in string[] pdfDataOnPage)
        {
            foreach (var page in pdfDataOnPage)
            {
                string[] lines = page.Split('\n');
                if (this._dbCreator.NameOfDB == null)
                {
                    SetNameOfDb(lines);
                }

                SetPKAndTypeOfField(table, tables, fields, lines, this._tableStartLineIndex);

                ;
            }

        }

        /// <summary>
        /// Adatbázis nevének beállítása
        /// </summary>
        /// <param name="lines">Sorok</param>
        private void SetNameOfDb(in string[] lines)
        {
            int i = 0;

            bool isDigit = false;

            do
            {
                isDigit = char.IsDigit(lines[i], 0);
                i++;
            } while (i < lines.Length && !isDigit);

            this._dbCreator.SetupNameOfDb(lines[i - 1].Substring(3));
        }

        private void SetPKAndTypeOfField(in ITable table, in ITable[] tables, IField[] fields, in string[] lines, int index)
        {
            int from = index;


            from = FindTableLineIndex(table.TableName, lines, from);


            //Amennyiben megtaláltuk a tábla sorát a pdf-ben
            if (from >= 0)
            {
                ProcessFields(fields, tables, lines, from);
            }
        }

        /// <summary>
        /// Megkeresi a pdf-ben a táblához tartozó sort
        /// </summary>
        /// <param name="tableName">Tábla neve</param>
        /// <param name="lines">Pdf-ben található sorok</param>
        /// <param name="startIndex">Indulóindex</param>
        /// <returns><c>InvalidOperationException</c> amennyiben nem található az adott táblához leírás a pdf-ben</returns>
        private int FindTableLineIndex(in string tableName, in string[] lines, in int startIndex)
        {
            for (int i = startIndex; i < lines.Length; i++)
            {
                if (lines[i].Contains(tableName))
                {
                    return i;
                }
            }

            throw new InvalidOperationException("The table was not found!");
        }

        /// <summary>
        /// Beállítja a mezők értékeit
        /// </summary>
        /// <param name="fields">Mezők</param>
        /// <param name="lines">Ahol az adatok vannak</param>
        /// <param name="fromTableIndex">Melyik sortól kezdve nézzük</param>
        private void ProcessFields(IField[] fields, ITable[] tables, string[] lines, int fromTableIndex)
        {
            //TODO
            int toTableIndex = fromTableIndex + 1;
            List<ITable> otherTables = tables.ToList();
            var a = tables.Where(x => lines[fromTableIndex].Contains(x.TableName)).FirstOrDefault();
            otherTables.Remove(a);

            //megkeresi, hogy meddig tart a leírása az adott táblánál
            while (toTableIndex < lines.Length && !otherTables.Any(table => lines[toTableIndex].Contains(table.TableName)))
            {
                toTableIndex++;
            }

            int j = fromTableIndex + 1;
            for (int i = 0; i < fields.Length; i++)
            {
                List<IField> otherFields = fields.ToList();
                otherFields.Remove(fields[i]);
                while (j < toTableIndex && !otherFields.Any(f => lines[j].SequenceEqual(f.FieldName)) && fields[i].TypeOfField == null)
                {
                    //amennyiben még nem lett beállítva semelyik mezőre sem az elsődleges kulcs
                    if (!fields.Any(x => x.IsPrimaryKey))
                    {
                        bool isPrimaryKey = lines[j].Contains("ez a kulcs");
                        if (isPrimaryKey)
                        {
                            fields[i].SetupIsPrimaryKey(true);
                        }
                    }
                    //minden esetben van a mezőnek alapértelmezett típusa
                    fields[i].SetupTypeOfField(lines[j]);
                    j++;
                }
            }

            //for (int j = 0; j < fields.Length; j++)
            //{
            //    //amennyiben még nem lett beállítva semelyik mezőre sem az elsődleges kulcs
            //    if (!fields.Any(x => x.IsPrimaryKey))
            //    {
            //        bool isPrimaryKey = lines[tableLineIndex + j + 1].Contains("ez a kulcs");
            //        if (isPrimaryKey)
            //        {
            //            fields[j].SetupIsPrimaryKey(true);
            //        }
            //    }
            //    //minden esetben van a mezőnek alapértelmezett típusa
            //    //TODO:
            //    fields[j].SetupTypeOfField(lines[tableLineIndex + j + 1]);
            //}
        }

        /// <summary>
        /// Txt-ben lévő adatok feldolgozása
        /// </summary>
        /// <param name="txtFileLines">Adatok</param>
        /// <returns>Mezőkhöz tartozó név + adatok</returns>
        private IField[] TxtDataProcessing(in string[] txtFileLines)
        {
            string[] fieldInfo = txtFileLines[0].Split('\t'); //kezdetben a 0-ik sorból kiszedjük a mezők neveit
            IField[] fields = new IField[fieldInfo.Length]; //amennyi mezőnév található az első sorban, annyi mezőnk lesz
            SetFieldNames(fields, fieldInfo, txtFileLines.Length - 1);

            for (int i = 1; i < txtFileLines.Length; i++)
            {
                fieldInfo = txtFileLines[i].Split('\t'); //itt már nem a mezők nevei lesznek, hanem a hozzájuk tartozó adatok
                SetFieldValues(fields, fieldInfo);
            }

            return fields;
        }

        /// <summary>
        /// Beállítjuk az mezőkhöz tartozó értékeket soronként
        /// </summary>
        /// <param name="fields">Elérhető mezők</param>
        /// <param name="lineWithData">Egy sornyi adat</param>
        private void SetFieldValues(IField[] fields, in string[] lineWithData)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].AddFieldValue(lineWithData[i]);
            }
        }

        /// <summary>
        /// Beállítjuk a mezőknek a nevét
        /// </summary>
        /// <param name="fields">Mezők</param>
        /// <param name="fieldNamesFromTxt">Mezőnevek</param>
        /// <param name="maxLine">Mezők száma</param>
        private void SetFieldNames(IField[] fields, in string[] fieldNamesFromTxt, in int maxLine)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new Field();
                fields[i].SetupFieldName(fieldNamesFromTxt[i]);
                fields[i].SetupFieldNum(maxLine);
            }
        }

        /// <summary>
        /// Mezők hozzárendelése a táblához
        /// </summary>
        /// <param name="table">Tábla amihez hozzárendelünk</param>
        /// <param name="fields">Mezők, amiket a táblához rendelünk</param>
        private void AddFieldToTable(in ITable table, in IField[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                table.AddFieldInfo(fields[i]);
            }
        }

    }
}
