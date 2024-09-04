using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Interfaces
{
    public interface IFileWriter
    {
        /// <summary>
        /// Fájlba írás
        /// </summary>
        void SQLWriter(string savePath);

    }
}
