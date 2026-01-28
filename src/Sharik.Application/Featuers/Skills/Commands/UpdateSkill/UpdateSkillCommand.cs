using MediatR;
using Sharik.Application.Featuers.Skills.Dtos;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Skills.Commands.UpdateSkill
{
    public sealed record UpdateSkillCommand(Guid SkillId,
                                            string Name,
                                            Guid SkillCategoryId) :IRequest<Result<SkillDto>>;
    
}
