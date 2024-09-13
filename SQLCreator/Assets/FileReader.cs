using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using SQLCreator.Interfaces;
using SQLCreator.Model;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using IOPath = System.IO.Path;

namespace SQLCreator.Assets
{
    public class FileReader : IFileReader
    {
        public FileModel[] FileOpener()
        {
            return DirectoryReader();
        }

        public List<string[]> TxtReader(FileModel fModelValue)
        {
            string[] destinations = fModelValue.TxtFileDestination.Split('\n');
            List<string[]> datas = new List<string[]>();

            for (int i = 0; i < destinations.Length; i++)
            {
                datas.Add(File.ReadAllLines(destinations[i], Encoding.Latin1));
                datas[i][0] = datas[i][0].ToLower();
            }
            return datas;
        }

        public string[] PdfReader(FileModel fModelValue)
        {
            string[] splittedPages = fModelValue.PdfPageNum.Split(';'); //pdf oldalak
            string[] dataOnPages = new string[splittedPages.Length]; //pdf oldalokon található sorok

            using (PdfReader reader = new PdfReader(fModelValue.PdfFileDestination))
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
        private FileModel[] DirectoryReader()
        {
            OpenFolderDialog fDialog = new OpenFolderDialog();
            fDialog.Multiselect = true;
            fDialog.ShowDialog();
            FileModel[] fModel = new FileModel[fDialog.FolderNames.Length];
            int idx = 0;

            foreach (var item in GetFilesPathFromFolder(fDialog.FolderNames))
            {
                fModel[idx++] = FModelConverter(item);
            }
            return fModel;
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
        /// FileModel típusá konvertáló
        /// </summary>
        /// <param name="files">Fájlok tömbje</param>
        /// <returns>Átkonvertált példány</returns>
        private FileModel FModelConverter(string[] files)
        {
            FileModel fModel = new FileModel();
            foreach (var item in files)
            {
                if (IOPath.GetExtension(item) == ".txt")
                {
                    fModel.TxtFileDestination += (fModel.TxtFileName == null
                    ? $"{item}" //első item
                    : $"\n{item}"); //további itemek (feldolgozásnál és splittelésnél lesz szerepe)

                    fModel.TxtFileName += (fModel.TxtFileName == null
                    ? $"{IOPath.GetFileNameWithoutExtension(item)}" //első item
                    : $"\n{IOPath.GetFileNameWithoutExtension(item)}");
                }
                else if (IOPath.GetExtension(item) == ".pdf")
                {
                    fModel.PdfFileDestination = item;
                    fModel.PdfFileName = IOPath.GetFileNameWithoutExtension(item);
                }
            }

            fModel.PdfPageNum = GetTablePageNum(fModel.PdfFileDestination).ToString();
            fModel.OutPutDestination = IOPath.GetDirectoryName(fModel.PdfFileDestination);

            return fModel;
        }

        /// <summary>
        /// Megkeresi, hogy hányadik oldalon van az adatbázisos feladat
        /// </summary>
        /// <param name="path">Pdf fájl teljes elérési útvonala</param>
        /// <returns><c>0</c> amennyiben nem találtuk meg a megfelelő oldalt, <c>különben</c> az az oldal, ahol megtalálható az adatbázis leírását</returns>
        private int GetTablePageNum(string path)
        {
            //TODO: 2x keressük meg a táblák részt. Ezt javítani kellene és máshova tenni
            string[] SEARCHED_ITEM = { "Táblák:", "adattáblák", "tábla" }; //keresett szó
            using (PdfReader reader = new PdfReader(path))
            {
                ITextExtractionStrategy Strategy = new LocationTextExtractionStrategy();

                int i = 1;
                string[] aSplit = PdfTextExtractor.GetTextFromPage(reader, i, Strategy).Split('\n'); //nem működik
                var b = Algorithms.ContainsAny(aSplit, SEARCHED_ITEM);
                while (i <= reader.NumberOfPages && !PdfTextExtractor.GetTextFromPage(reader, i, Strategy).Split('\n').Any(x => SEARCHED_ITEM.Contains(x)))
                {
                    i++;
                }
                return i < reader.NumberOfPages ? i : -1;
            }


        }
    }
}
