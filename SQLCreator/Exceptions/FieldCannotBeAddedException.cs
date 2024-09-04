namespace SQLCreator.Exceptions
{
    internal sealed class FieldCannotBeAddedException : BaseException
    {
        public FieldCannotBeAddedException(string message, string classInfo) : base(message, classInfo)
        {
        }
    }
}
