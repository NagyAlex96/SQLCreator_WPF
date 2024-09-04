namespace SQLCreator.Interfaces
{
    public interface IDBCreator
    {
        /// <summary>
        /// Adatbázis neve
        /// </summary>
        string NameOfDB { get; }
        /// <summary>
        /// Táblákat tartalmazó tömb
        /// </summary>
        ITable[] Tables { get; }

        /// <summary>
        /// Tábla hozzáadása
        /// </summary>
        /// <param name="table">Tábla információ(i)</param>
        void AddTable(ITable table);

        /// <summary>
        /// Beállítjuk az adatbázis nevét
        /// </summary>
        /// <param name="dbName">Adatbázis név</param>
        /// <returns></returns>
        IDBCreator SetupNameOfDb(string dbName);

        /// <summary>
        /// Beállítjuk az adatbázis tábláinak számát
        /// </summary>
        /// <param name="tableNum">Táblák száma</param>
        /// <returns></returns>
        IDBCreator SetupTables(int tableNum);
    }
}
