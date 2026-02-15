using MediatR;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.User.Dtos;
using Sharik.Domain.Common.Results;

namespace Sharik.Application.Featuers.User.Queries.GetNotification
{
    public sealed class GetNotificationQueryHandler(IAppDbContext _context) : IRequestHandler<GetNotificationQuery , Result<List<NotificationDto>>>
    {
        public async Task<Result<List<NotificationDto>>> Handle(GetNotificationQuery request , CancellationToken ct)
        {

            var data = await _context.Notifications.Where(n => n.UserId == request.userId)
                                                   .Select(n => new NotificationDto(n.Id , n.Type.ToString() , n.Message , n.IsRead , n.CreatedAt))
                                                   .ToListAsync(ct);

            return data;
        }
    }
}
