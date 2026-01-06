namespace Reserveit.Domain.Exceptions;

public sealed class ForbiddenException : Exception
{
    public ForbiddenException() : base("Access denied.") { }
    public ForbiddenException(string message) : base(message) { }
}
