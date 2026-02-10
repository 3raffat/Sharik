using System.Text.Json.Serialization;

namespace Sharik.Domain.Exchanges.Enums
{
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
