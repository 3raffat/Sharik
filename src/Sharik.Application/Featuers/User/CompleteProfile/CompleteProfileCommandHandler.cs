using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.CompleteProfile
{
    public sealed class CompleteProfileCommandHandler(
        ILogger<CompleteProfileCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<CompleteProfileCommand, Result<Updated>>
    {
        public async Task<Result<Updated>> Handle(CompleteProfileCommand request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId, ct);

            if (user is null)
            {
                _logger.LogWarning("User not found with UserId: {UserId}", request.userId);
                return ApplicationErrors.UserNotFound;
            }


            var userResult = user.CompleteProfile(request.firstName,
                                                   request.lastName,
                                                   request.bio);

            if (userResult.IsFailure)
                return userResult.Errors;

            await _context.SaveChangesAsync(ct);    

            _logger.LogInformation("User profile completed successfully for UserId: {UserId}", request.userId);

            return Result.Updated;
        }
    }
}
