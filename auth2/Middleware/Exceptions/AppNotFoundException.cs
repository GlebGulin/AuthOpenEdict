namespace auth2.Middleware.Exceptions
{
    public class AppNotFoundException : AppException
    {
        public AppNotFoundException(string message)
            : base(message, 404) { }
    }
}
