namespace Shared.Exceptions;

public class ItemInConfigurationNotFoundException : Exception
{
    public ItemInConfigurationNotFoundException(string key) 
        : base($"Item with key: `{key}` was not found in configuration")
    { }
}