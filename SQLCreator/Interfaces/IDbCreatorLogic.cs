using SQLCreator.Model;
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
        /// <param name="DBModel">Adatbázis létrehozásához kapcsolódó adatok</param>
        /// <returns>Elkészített adatbázis</returns>
        public DataBaseModel CreateDataBase(in DataBaseModel DBModel);
    }
}
