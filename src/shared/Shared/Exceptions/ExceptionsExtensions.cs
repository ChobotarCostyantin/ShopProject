using System.Data.Common;
using Npgsql;

namespace Shared.Exceptions;

public static class ExceptionsExtensions
{
    public static InfrastructureException ToInfrastructureException(this DbException ex, string? context = null)
    {
        string prefix = string.IsNullOrEmpty(context) ? "" : $"{context}: ";

        return ex switch
        {
            PostgresException pgEx => (pgEx.SqlState switch
            {
                "23505" => new DatabaseConstraintException(prefix + "Unique constraint violation.", ex),
                "23503" => new DatabaseConstraintException(prefix + "Foreign key violation.", ex),
                "23502" => new DatabaseConstraintException(prefix + "Not-null violation.", ex),
                "57014" => new DatabaseUnavailableException(prefix + "Query canceled.", ex),
                "08006" or "08001" => new DatabaseUnavailableException(prefix + "Database connection error.", ex),
                _ => new DatabaseUnavailableException(prefix + "Database operation failed.", ex)
            }),
            _ => new DatabaseUnavailableException(prefix + "Database operation failed.", ex)
        };
    }
}