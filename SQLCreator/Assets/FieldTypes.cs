using SQLCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLCreator.Assets
{
    public class FieldTypes
    {
        public static readonly string DEFAULT_VALUE = "VARCHAR(64)";
        public static List<string> TypeOfFieldsValues =>PossibleTypesOfField.Select(x=>x.Value).ToList();

        /// <summary>
        /// Egy mező lehetséges típusa
        /// </summary>
        static readonly Dictionary<string, string> PossibleTypesOfField = new Dictionary<string, string>
        {
            {"(számláló)", "INT AUTO_INCREMENT" },
            { "(szám)","INT"},
            {"tizedes jegy pontosan", "DOUBLE" },
            { "(szöveg)","VARCHAR(64)"},
            { "VARCHAR(128)","VARCHAR(128)"}, //extra
            { "VARCHAR(256)","VARCHAR(256)"}, //extra
            { "(logikai)","BOOLEAN"},
            { "(dátum)","DATE"},
        };

        /// <summary>
        /// Beállítja a mező típusát
        /// </summary>
        /// <param name="line">Egy sor adat</param>
        /// <returns></returns>
        public static string SetFieldType(string line)
        {
            string matchingKey = PossibleTypesOfField.Keys.FirstOrDefault(key => line.Contains(key));
            if (!string.IsNullOrEmpty(matchingKey))
            {
                return PossibleTypesOfField[matchingKey];
            }
            return DEFAULT_VALUE;
        }
    }
}
