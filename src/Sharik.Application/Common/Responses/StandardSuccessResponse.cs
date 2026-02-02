using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Common.Responses
{
    public sealed record StandardSuccessResponse<T>(T? Data,
                                                        int Status,
                                                        string Message);
}
