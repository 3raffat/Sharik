using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Auth.Dtos
{
    public sealed record TokenResponse(string AccessToken,string RefreshToken,DateTime ExpiresOnUtc);
  
}
