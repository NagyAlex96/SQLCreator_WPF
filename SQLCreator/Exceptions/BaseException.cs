namespace SQLCreator.Exceptions
{
    internal abstract class BaseException : Exception
    {
        public BaseException(string message, string classInfo) : base(message)
        {
            ClassInfo = classInfo;
        }

        public string ClassInfo { get; private set; }
    }
}
