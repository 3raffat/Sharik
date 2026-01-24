using System.Text.Json.Serialization;

namespace Sharik.Domain.Exchanges.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExchangeStatus
    {
        Pending,
        Accepted,
        InProgress,
        Completed,
        Cancelled,
        Rejected
    }
}
