namespace Reserveit.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceIdentifier) : Exception($"{resourceType} with ID: {resourceIdentifier} doesn't exist")
{

}
