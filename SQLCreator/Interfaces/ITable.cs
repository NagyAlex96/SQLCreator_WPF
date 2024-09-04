namespace SQLCreator.Interfaces
{
    public interface ITable
    {
        /// <summary>
        /// Táblanév
        /// </summary>
        string TableName { get; }
        /// <summary>
        /// Mezőinformáció pl.: elsődleges kulcs, mezőnév, mezőtípus
        /// </summary>
        IField[] FieldData { get; }
        /// <summary>
        /// Mező rögzítése
        /// </summary>
        /// <param name="field">Egy konkrét mezőhöz tartozó adat(ok)</param>
        void AddFieldInfo(IField field);
        public bool HasForeignKey { get; }
    }
}
