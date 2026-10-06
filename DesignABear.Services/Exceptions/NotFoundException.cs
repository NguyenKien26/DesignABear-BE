namespace DesignABear.Services.Exceptions
{
    /// <summary>
    /// Thrown when a requested resource cannot be found.
    /// ExceptionMiddleware maps this to 404 Not Found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string entityName, object id)
            : base($"{entityName} with id '{id}' was not found.") { }
    }
}
