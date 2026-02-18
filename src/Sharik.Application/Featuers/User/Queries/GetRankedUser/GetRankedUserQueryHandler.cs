using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.User.Queries.GetRankedUser
{
    public sealed class GetRankedUserQueryHandler(IAppDbContext _context) : IRequestHandler<GetRankedUserQuery , Result<List<RankedUserDto>>>
    {
        public async Task<Result<List<RankedUserDto>>> Handle(GetRankedUserQuery request , CancellationToken ct)
        {

            var data = await _context.Users
                .OrderByDescending(u => u.TotalPointsEarned)
                .Select(u=>new RankedUserDto(u.Id ,
                                             u.fullName ,
                                             u.TotalPointsEarned ,
                                             u.Rating))
                .ToListAsync(ct);


         return data;
        }
    }
}
