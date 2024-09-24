using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using SQLCreator.Interfaces;
using System.IO;
using System.Text;
using SQLCreator.Model;
using IOPath = System.IO.Path;
using System.Runtime.Intrinsics.Arm;
using SQLCreator.Assets;

namespace SQLCreator.Logic
{
    public class FileReaderLogic : IFileReaderLogic
    {
        public DataBaseModel[] FileOpener()
        {
            return DirectoryReader();
        }

        public List<string[]> TxtReader(DataBaseModel dModelvalue)
        {
            string[] destinations = dModelvalue.TxtFileDestination.Split('\n');
            List<string[]> datas = new List<string[]>();
            for (int i = 0; i < destinations.Length; i++)
            {
                datas.Add(File.ReadAllLines(destinations[i], Encoding.UTF8)); 
                datas[i][0] = datas[i][0].ToLower();
            }
            return datas;
        }

        public string[] PdfReader(DataBaseModel DBaseModelValue)
        {
            string[] splittedPages = DBaseModelValue.PdfPageNum.Split(';'); //pdf oldalak
            string[] dataOnPages = new string[splittedPages.Length]; //pdf oldalokon található sorok

            using (PdfReader reader = new PdfReader(DBaseModelValue.PdfFileDestination))
            {
                ITextExtractionStrategy Strategy = new LocationTextExtractionStrategy();

                for (int i = 0; i < splittedPages.Length; i++)
                {
                    dataOnPages[i] = PdfTextExtractor.GetTextFromPage(reader, int.Parse(splittedPages[i]), Strategy).ToLower();
                }
            }

            return dataOnPages;
        }

        /// <summary>
        /// Mappa beolvasását valósítja meg
        /// </summary>
        /// <returns></returns>
        private DataBaseModel[] DirectoryReader()
        {
            OpenFolderDialog fDialog = new OpenFolderDialog();
            fDialog.Multiselect = true;
            fDialog.ShowDialog();
            DataBaseModel[] DBModelDatas = new DataBaseModel[fDialog.FolderNames.Length];
            int idx = 0;

            foreach (var item in GetFilesPathFromFolder(fDialog.FolderNames))
            {
                DBModelDatas[idx++] = DBModelConverter(item);
            }
            return DBModelDatas;
        }

        /// <summary>
        /// A mappában található fájlok útvonalát + nevét szedi ki
        /// </summary>
        /// <param name="folders">Mappák, ahol keresünk</param>
        /// <returns>Kiolvasott fájlok listája</returns>
        private List<string[]> GetFilesPathFromFolder(string[] folders)
        {
            List<string[]> files = new List<string[]>();

            foreach (var item in folders)
            {
                files.Add(Directory.GetFiles(item));
            }

            return files;
        }

        /// <summary>
        /// DataBaseModel típusá konvertáló
        /// </summary>
        /// <param name="files">Fájlok tömbje</param>
        /// <returns>Átkonvertált példány</returns>
        private DataBaseModel DBModelConverter(string[] files)
        {
            DataBaseModel DBModel = new DataBaseModel();
            foreach (var item in files)
            {
                if (IOPath.GetExtension(item) == ".txt")
                {
                    DBModel.TxtFileDestination += (DBModel.TxtFileDestination == null
                    ? $"{item}" //első item
                    : $"\n{item}"); //további itemek (feldolgozásnál és splittelésnél lesz szerepe)

                    DBModel.TxtFileName.Add(IOPath.GetFileNameWithoutExtension(item));
                }
                else if (IOPath.GetExtension(item) == ".pdf")
                {
                    DBModel.PdfFileDestination = item;
                    DBModel.PdfFileName = IOPath.GetFileNameWithoutExtension(item);
                }
            }

            (int, int) pdfvalues = GetTablePageNum(DBModel.PdfFileDestination);
            DBModel.PdfPageNum = pdfvalues.Item1.ToString();
            DBModel.PdfLineNum = pdfvalues.Item2.ToString();
            DBModel.OutPutDestination = IOPath.GetDirectoryName(DBModel.PdfFileDestination);

            return DBModel;
        }

        /// <summary>
        /// Megkeresi, hogy hányadik oldalon van az adatbázisos feladat, és hogy hányadik sorban van a táblák rész
        /// </summary>
        /// <param name="path">Pdf fájl teljes elérési útvonala</param>
        /// <returns><c>-1, -1</c> amennyiben nem találta meg, különben a <c>oldalszám/sorszám</c> formában adja vissza</returns>
        private (int, int) GetTablePageNum(string path)
        {
            string[] SEARCHED_ITEM = { "Táblák:", "Tábla", "adattáblák szerkezete:"}; //keresett szó
            using (PdfReader reader = new PdfReader(path))
            {
                ITextExtractionStrategy Strategy = new LocationTextExtractionStrategy();
                int i = 0;
                int index = -1;

                do
                {
                    i++;
                    //index = Array.FindIndex(PdfTextExtractor.GetTextFromPage(reader, i, Strategy).Split('\n'), x => SEARCHED_ITEM.Contains(x.TrimEnd()));
                    index = Algorithms.FindIndex(PdfTextExtractor.GetTextFromPage(reader, i, Strategy).Split('\n'), SEARCHED_ITEM);
                } while (i <= reader.NumberOfPages && index < 0);

                return index >= 0 ? (i, index) : (-1, -1);
            }


        }
    }
}
