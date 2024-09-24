using SQLCreator.Model;

namespace SQLCreator.Interfaces
{
    public interface IFileReaderLogic
    {
        /// <summary>
        /// Fájl megnyitása
        /// </summary>
        /// <returns></returns>
        DataBaseModel[] FileOpener();

        /// <summary>
        /// Txtolvasó
        /// </summary>
        /// <param name="DBaseModelValue"></param>
        /// <returns>Txt fájlokban található adatok, sorok</returns>
        List<string[]> TxtReader(DataBaseModel DBaseModelValue);

        /// <summary>
        /// Pdf olvasó
        /// </summary>
        /// <param name="DBaseModelValue"></param>
        /// <returns>Az adott oldalakon található adatok</returns>
        string[] PdfReader(DataBaseModel DBaseModelValue);
    }
}
