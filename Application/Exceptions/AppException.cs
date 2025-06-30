namespace Application.Exceptions;

public class AppException : Exception
{
    public dynamic Payload { get; set; }

    public AppException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }
}