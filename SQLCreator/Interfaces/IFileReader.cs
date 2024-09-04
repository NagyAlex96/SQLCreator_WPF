using SQLCreator.Model;

namespace SQLCreator.Interfaces
{
    public interface IFileReader
    {
        /// <summary>
        /// Fájl megnyitása
        /// </summary>
        /// <returns></returns>
        FileModel[] FileOpener();

        /// <summary>
        /// Txtolvasó
        /// </summary>
        /// <param name="fModelValue"></param>
        /// <returns>Txt fájlokban található adatok, sorok</returns>
        List<string[]> TxtReader(FileModel fModelValue);

        /// <summary>
        /// Pdf olvasó
        /// </summary>
        /// <param name="fModelValue"></param>
        /// <returns>Az adott oldalakon található adatok</returns>
        string[] PdfReader(FileModel fModelValue);
    }
}
