using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Caching;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommandHandler(
        ILogger<UpdateProfileCommandHandler> _logger ,
        IAppDbContext _context , HybridCache _cache) : IRequestHandler<UpdateProfileCommand , Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(UpdateProfileCommand request , CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId , ct);

            if (user is null)
            {
                _logger.LogWarning("User not found with UserId: {UserId}" , request.userId);
                return ApplicationErrors.UserNotFound;
            }


            var userResult = user.UpdateProfile(request.FirstName ,
                                                   request.LastName ,
                                                   request.Bio);

            if (userResult.IsFailure)
                return userResult.Errors;

            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("User profile updated successfully for UserId: {UserId}" , request.userId);

            await _cache.RemoveAsync(CacheKeys.User.UserById(user.Id) , ct);

            return Result.Updated;
        }
    }
}
