using SQLCreator.Model;

namespace SQLCreator.Interfaces
{
    public interface IFileLogic
    {
        /// <summary>
        /// Fájl(ok) hozzáadása
        /// </summary>
        /// <param name="fileModels">Collection, ahova mentésre kerül</param>
        void AddFile(IList<DataBaseModel> fileModels);
        /// <summary>
        /// Kijelölt fájlok módosítása
        /// </summary>
        /// <param name="fModel">Módosítandó item</param>
        void Modify(DataBaseModel fModel);
        /// <summary>
        /// Kijelölt fájl törlése
        /// </summary>
        /// <param name="removeFrom">Honnan szeretnénk törölni</param>
        /// <param name="removeWhat">Törlendő fájl</param>
        void Delete(IList<DataBaseModel> removeFrom, DataBaseModel removeWhat);
        /// <summary>
        /// Egy itemet szeretnénk átmozgatni egyik listából a másikba
        /// </summary>
        /// <param name="moveFrom">Ahonnan átmozgatjuk</param>
        /// <param name="moveTo">Ahova átmozgatjuk</param>
        /// <param name="item">Amit átmozgatunk</param>
        public void MoveFileFromTo(IList<DataBaseModel> moveFrom, IList<DataBaseModel> moveTo, DataBaseModel item);
    }
}