namespace DesignABear.Services.Exceptions
{
    /// <summary>
    /// Thrown for business-rule violations or invalid input.
    /// ExceptionMiddleware maps this to 400 Bad Request.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
