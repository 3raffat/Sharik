using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Errors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.AcceptExchanges;
using Sharik.Application.Featuers.Exchanges.CreateExchanges;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Exchanges.Validation
{
    public sealed class ExchangeBusinessValidator(IAppDbContext _context , ILogger<ExchangeBusinessValidator> _logger) : IExchangeBusinessValidator
    {
     
        public async Task<Result<Success>> ValidateCreateExchangeAsync(CreateExchangesCommand command , CancellationToken ct)
        {

            var duplicateResult = await ValidateDuplicateExchangeAsync(command , ct);

            if (duplicateResult.IsFailure)
                return duplicateResult;

            var providerResult = await ValidateProviderExistsAsync(command.providerId , ct);

            if (providerResult.IsFailure)
                return providerResult;

            var requesterSkillResult = await ValidateUserHasSkillAsync(command.requesterId , command.skillOfferedId , "Requester" , ct);

            if (requesterSkillResult.IsFailure)
                return requesterSkillResult;

            var providerSkillResult = await ValidateUserHasSkillAsync(command.providerId , command.skillRequestedId , "Provider" , ct);

            if (providerSkillResult.IsFailure)
                return providerSkillResult;

            return Result.Success;
        }
        private async Task<Result<Success>> ValidateDuplicateExchangeAsync(CreateExchangesCommand command , CancellationToken ct)
        {
            var exists = await _context.Exchanges
                .AnyAsync(e =>
                e.RequesterId == command.requesterId &&
                e.ProviderId == command.providerId &&
                e.SkillOfferedId == command.skillOfferedId &&
                e.SkillRequestedId == command.skillRequestedId &&
                e.Type == command.type , ct);

            if (exists)
            {
                _logger.LogWarning("Exchange already exists for requester {RequesterId} with provider {ProviderId} for skills {SkillOfferedId} -> {SkillRequestedId} and type {Type}" ,
                          command.requesterId , command.providerId , command.skillOfferedId , command.skillRequestedId , command.type);
                return ApplicationErrors.ExchangeAlreadyExists;
            }

            return Result.Success;
        }
        private async Task<Result<Success>> ValidateProviderExistsAsync(Guid providerId , CancellationToken ct)
        {
            var exists = await _context.Users.AnyAsync(u => u.Id == providerId , ct);

            if (!exists)
            {
                _logger.LogWarning("User with id {UserId} not found" , providerId);
                return ApplicationErrors.ProviderNotFound;
            }
            return Result.Success;
        }
        private async Task<Result<Success>> ValidateUserHasSkillAsync(Guid userId , Guid skillId , string userType , CancellationToken ct)
        {
            var hasSkill = await _context.UserSkills.AnyAsync(us => us.UserId == userId && us.SkillId == skillId , ct);

            if (!hasSkill)
            {
                _logger.LogWarning("{UserType} {UserId} does not have skill {SkillId}" ,
                            userType , userId , skillId);
                return ApplicationErrors.UserSkillNotFound;
            }
            return Result.Success;
        }

    }
}