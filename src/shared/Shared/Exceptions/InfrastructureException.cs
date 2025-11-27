namespace Shared.Exceptions;

public abstract class InfrastructureException : Exception
{
    protected InfrastructureException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class DatabaseConstraintException : InfrastructureException
{
    public DatabaseConstraintException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class DatabaseUnavailableException : InfrastructureException
{
    public DatabaseUnavailableException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class EntityNotFoundException : InfrastructureException
{
    public EntityNotFoundException(string message, Exception? inner = null)
        : base(message, inner) { }
}