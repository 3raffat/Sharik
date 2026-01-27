using MediatR;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Skills.Commands.CreateSkill
{
    public sealed class CreateSkillCommandHandler(
        ILogger<CreateSkillCommandHandler> _logger,
        IAppDbContext _context) : IRequestHandler<CreateSkillCommand, Result<SkillDto>>
    {
        public Task<Result<SkillDto>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

        }
    }
}
