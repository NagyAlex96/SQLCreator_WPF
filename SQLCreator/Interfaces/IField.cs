namespace SQLCreator.Interfaces
{
    public interface IField
    {
        /// <summary>
        /// Mezőnév
        /// </summary>
        string FieldName { get; }
        /// <summary>
        /// Milyen típusú a mező (szöveg, egész, logikai etc..)
        /// </summary>
        string TypeOfField { get; }
        /// <summary>
        /// Elsődlegeskulcs-e az adott mező
        /// </summary>
        bool IsPrimaryKey { get; }
        /// <summary>
        /// Idegenkulcs-e az adott mező
        /// </summary>
        bool IsForeignKey { get; }
        /// <summary>
        /// Idegenkulcs esetén melyik táblára mutat
        /// </summary>
        string ReferenceTo { get; }
        /// <summary>
        /// Mezőértékek
        /// </summary>
        string[] FieldValue { get; }

        /// <summary>
        /// Mezőnév beállítása
        /// </summary>
        /// <param name="fieldName">Mezőnév</param>
        /// <returns></returns>
        public IField SetupFieldName(string fieldName);
        /// <summary>
        /// Mezőtípus beállítása
        /// </summary>
        /// <param name="typeOfField">Mezőtípus</param>
        /// <returns></returns>
        public IField SetupTypeOfField(string typeOfField);
        /// <summary>
        /// Az adott mező elsődleges kulcsának beállítása
        /// </summary>
        /// <param name="isPrimaryKey"><c>True</c> amennyiben elsődleges kulcs</param>
        /// <returns></returns>
        public IField SetupIsPrimaryKey(bool isPrimaryKey);
        /// <summary>
        /// Az adott mező idegen kulcsának beállítása
        /// </summary>
        /// <param name="isForeignKey"><c>True</c> amennyiben idegenkulcs</param>
        /// <returns></returns>
        public IField SetupIsForeignKey(bool isForeignKey);
        /// <summary>
        /// Mező hivatkozás beállítása
        /// </summary>
        /// <param name="referenceTo">Melyik mezőre hivatkozik</param>
        /// <returns></returns>
        public IField SetupReferenceTo(string referenceTo);
        /// <summary>
        /// Az adott mező mennyi rekord (sor) adatot tartalmaz
        /// </summary>
        /// <param name="fieldNum">Rekordok száma</param>
        public IField SetupFieldNum(int fieldNum);
        /// <summary>
        /// Mezőhöz tartozó adatok
        /// </summary>
        /// <param name="value">Konkrét adat</param>
        void AddFieldValue(string value);
    }
}
