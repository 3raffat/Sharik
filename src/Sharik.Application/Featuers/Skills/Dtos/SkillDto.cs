using MediatR;
using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Featuers.Skills.Dtos
{
    public sealed record SkillDto(Guid Id,string Name);
    
}
