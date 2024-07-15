

using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class NotificationRepository : SQLExtendRepository<Notification>, INotificationRepository
    {
        private readonly EventManagementContext _context;
        private readonly ICacheRepository _cacheRepository;

        public NotificationRepository(EventManagementContext context, ICacheRepository cacheRepository) : base(context)
        {
            _context = context;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Notification>> GetAllNotiByUserId(Guid userId)
        {
            var cacheKey = $"GetNoti_{userId}";
            var cachedNoti = await _cacheRepository.GetAsync<IEnumerable<Notification>>(cacheKey);

            if (cachedNoti != null)
            {
                return cachedNoti;
            }

            IEnumerable<Notification> entities = 
                await _context
                .Notifications
                .Include(a => a.UserId)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            await _cacheRepository.SetAsync(cacheKey, entities);
            return entities;
        }

        public async Task<IEnumerable<Notification>> GetAllNoti()
        {
            var cacheKey = $"GetAllNoti";
            var cachedNoti = await _cacheRepository.GetAsync<IEnumerable<Notification>>(cacheKey);

            if (cachedNoti != null)
            {
                return cachedNoti;
            }

            IEnumerable<Notification> entities = await _context.Notifications.Include(a => a.UserId).ToListAsync();
            await _cacheRepository.SetAsync(cacheKey, entities);
            return entities;
        }
    }
}
