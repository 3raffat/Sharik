using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Common.Responses
{
    public sealed record StandardErrorResponse(string Timestamp,
                                                    int Status,
                                                    string Message,
                                                    string Error,
                                                    string TraceId);

}
