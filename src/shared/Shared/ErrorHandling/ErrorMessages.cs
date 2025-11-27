namespace Shared.ErrorHandling;

public static class ErrorMessages
{
    public static string PropertyIsRequired(string property)
    {
        return $"{property} is required";
    }

    public const string NoError = "No error";
    public static string BadRequest(string reason) => $"{reason}";
    public static string Unauthorized(string reason) => $"{reason}";
    public static string Forbidden(string reason) => $"{reason}";
    public static string NotFound(object? key = null, string? entityName = null, string? alternativeErrorMessage = null) => 
        string.IsNullOrWhiteSpace(alternativeErrorMessage) 
            ? $"Entity '{entityName}' with ID of {key} was not found"
            : alternativeErrorMessage;
    public static string Conflict(string reason) => $"{reason}";
    public static string InternalServerError(string reason) => $"{reason}";
}