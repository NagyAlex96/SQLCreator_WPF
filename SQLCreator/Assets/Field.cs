using SQLCreator.Interfaces;

namespace SQLCreator.Assets
{
    public class Field : IField
    {
        public Field()
        {

        }

        public static readonly string DEFAULT_VALUE= "VARCHAR(64)";

        /// <summary>
        /// Egy mező lehetséges típusa
        /// </summary>
        private static readonly Dictionary<string, string> PossibleTypesOfField = new Dictionary<string, string>
        {
            { "(szám)","INT"},
            { "(szöveg)","VARCHAR(64)"},
            { "(logikai)","BOOLEAN"},
            { "(dátum)","DATE"},
        };

        public string FieldName { get; private set; }

        public string TypeOfField { get; private set; }

        public bool IsPrimaryKey { get; private set; }

        public bool IsForeignKey { get; private set; }

        public string ReferenceTo { get; private set; }
        public string[] FieldValue { get; private set; }

        /// <summary>
        /// Hányadik indexre helyezzük be az adatot
        /// </summary>
        private int idx = 0;
        public void AddFieldValue(string value)
        {
            if (idx < this.FieldValue.Length)
            {
                this.FieldValue[idx] = value;
                idx++;
            }
            else
            {
                throw new InvalidOperationException("Cannot add more field value. The array has reached the limit!");
            }
        }

        public IField SetupFieldName(string fieldName)
        {
            this.FieldName = fieldName;
            return this;
        }

        public IField SetupTypeOfField(string line)
        {
            //a kulcsok közül kiválasztjuk azt, amelyik megtalálható az adott sorban
            var values = PossibleTypesOfField.Keys
            .Select((key, idx) => new { key, idx })
            .Where(x => line.Contains(x.key))
            .Select((x => (x.key, x.idx))).FirstOrDefault();
            
            if(values.key != null) //megtaláltuk a lehetséges opciók közül
            {
                this.TypeOfField = PossibleTypesOfField[values.key];
            }
            else //nem találtuk meg, alapértelmezett beállítása
            {
                this.TypeOfField = DEFAULT_VALUE;
            }

            return this;
        }

        public IField SetupIsPrimaryKey(bool isPrimaryKey)
        {
            this.IsPrimaryKey = isPrimaryKey;
            return this;
        }

        public IField SetupIsForeignKey(bool isForeignKey)
        {
            this.IsForeignKey = isForeignKey;
            return this;
        }

        public IField SetupReferenceTo(string referenceTo)
        {
            this.ReferenceTo = referenceTo;
            return this;
        }

        IField IField.SetupFieldNum(int fieldNum)
        {
            this.FieldValue = new string[fieldNum];
            return this;
        }

        public override string ToString()
        {
            return $"{this.FieldName} {this.TypeOfField} NOT NULL";
        }
    }
}
