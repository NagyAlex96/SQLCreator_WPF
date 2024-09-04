namespace SQLCreator.Exceptions
{
    internal sealed class FieldValueCannotBeAddedException : BaseException
    {
        public FieldValueCannotBeAddedException(string message, string classInfo) : base(message, classInfo)
        {
        }
    }
}
