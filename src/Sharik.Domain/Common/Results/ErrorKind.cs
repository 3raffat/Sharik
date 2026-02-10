using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sharik.Domain.Common.Results
{
    public enum ErrorKind
    {
        Failure,
        Unexpected,
        Validation,
        Conflict,
        NotFound,
        Unauthorized,
        Forbidden,
    }
}
