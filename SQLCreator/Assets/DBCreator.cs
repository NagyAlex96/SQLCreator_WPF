using SQLCreator.Interfaces;

namespace SQLCreator.Assets
{
    public class DBCreator : IDBCreator
    {
        public ITable[] Tables { get; private set; }

        public string NameOfDB { get; private set; }

        public void AddTable(ITable table)
        {
            int i = 0;
            while(i<Tables.Length && Tables[i] != null)
            {
                i++;
            }

            if (i<Tables.Length)
            {
                Tables[i] = table;
            }
            else
            {
                throw new InvalidOperationException("Table array has reached the limit!");
            }
        }

        public IDBCreator SetupNameOfDb(string dbName)
        {
            this.NameOfDB = dbName;
            return this;
        }

        public IDBCreator SetupTables(int tableNum)
        {
            this.Tables = new ITable[tableNum];
            return this;
        }
    }
}
