namespace auth2.Middleware.Exceptions
{
    public class AppConflictException : AppException
    {
        public AppConflictException(string message)
            : base(message, 409) { }
    }
}
