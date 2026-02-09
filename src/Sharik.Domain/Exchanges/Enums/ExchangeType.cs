using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sharik.Domain.Exchanges.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExchangeType
    {
        Swap ,     
        Points 
    }
}
