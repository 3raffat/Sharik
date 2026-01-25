using System.Text.Json.Serialization;

namespace Sharik.Domain.User.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProfileStatus
    {
        Incomplete,
        Complete
    }
}
