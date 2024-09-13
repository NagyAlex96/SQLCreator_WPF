using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Assets
{
    public static class FieldTypes
    {
        public static readonly string DEFAULT_VALUE = "VARCHAR(64)";

        /// <summary>
        /// Egy mező lehetséges típusa
        /// </summary>
        public static readonly Dictionary<string, string> PossibleTypesOfField = new Dictionary<string, string>
        {
            { "(szám)","INT NOT NULL"},
            { "(szöveg)","VARCHAR(64) NOT NULL"},
            { "(logikai)","BOOLEAN NOT NULL"},
            { "(dátum)","DATE NOT NULL"},
            {"(számláló)", "INT AUTO_INCREMENT" }
        };
    }
}
