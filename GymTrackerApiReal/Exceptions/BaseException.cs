namespace GymTrackerApiReal.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string title, string message)
             : base(message) =>
             Title = title;
        protected BaseException(string message)
            : base(message) =>
            Message = message;

        public string Title { get; }
        public string Message { get; }
    }
}
