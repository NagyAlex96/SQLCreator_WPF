using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Interfaces
{
    public interface IDbCreatorLogic
    {
        public void CreateDataBase(in string[] pdfData, in List<string[]> txtFileLines, in string txtFileName);
        public IDBCreator DbCreator { get; }
    }
}
