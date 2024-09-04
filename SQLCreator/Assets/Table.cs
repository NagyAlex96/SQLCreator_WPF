using SQLCreator.Interfaces;

namespace SQLCreator.Assets
{
    public class Table : ITable
    {
        public Table(string tableName, int fieldNum)
        {
            TableName = tableName;
            this.FieldData = new IField[fieldNum];
        }

        public string TableName { get; }

        public IField[] FieldData { get; }

        public void AddFieldInfo(IField field)
        {
            int i = 0;

            while (i < FieldData.Length && this.FieldData[i] != null)
            {
                i++;
            }

            if (i < FieldData.Length)
            {
                this.FieldData[i] = field;
            }
            else
            {
                throw new InvalidOperationException("Field array has reached the limit!");
            }
        }

        public bool HasForeignKey => this.FieldData.Where(x => x.IsForeignKey).Count() != 0;
    }
}
