using MediatR;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.Auth.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommandHandler(IUserService _service) : IRequestHandler<ConfirmEmailCommand , Result<Success>>
    {
        public async Task<Result<Success>> Handle(ConfirmEmailCommand request , CancellationToken ct)
        {
            var result = await _service.ConfirmEmailAsync(request.userId , request.token , ct);

            if (result.IsFailure)
                    return result.Errors;

            return Result.Success;
        }
    }
}
