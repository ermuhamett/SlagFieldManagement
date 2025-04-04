using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Exceptions;

public class DomainException:Exception
{
    public Error Error { get; }

    public DomainException(Error error) : base(error.Message)
    {
        Error = error;
    }

    public DomainException(Error error, Exception innerException) 
        : base(error.Message, innerException)
    {
        Error = error;
    }
}