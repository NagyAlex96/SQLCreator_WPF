using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Interfaces
{
    public interface IDbCreatorLogic
    {
        /// <summary>
        /// Adatbázis elkészítése
        /// </summary>
        /// <param name="pdfData">Pdf fájl sorai</param>
        /// <param name="txtFileLines">Txt fájlok és a bennük található sorok</param>
        /// <param name="txtFileName">Txt fájlok nevei <c>\n</c>-el elválasztva</param>
        public void CreateDataBase(in string[] pdfData, in List<string[]> txtFileLines, in string txtFileName);
        public IDBCreator DbCreator { get; }
    }
}
