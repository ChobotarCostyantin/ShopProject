using System.Text.Json.Serialization;

namespace Orders.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        New,
        Paid,
        Shipped,
        Cancelled
    }
}
