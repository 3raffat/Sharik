using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sharik.Domain.Ratings.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RatingType
    {
        AsTeacher ,
        AsLearner 
    }
}
