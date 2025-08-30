using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Klimaci.Core.Abstracts;
using Klimaci.DTO.DashboardDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Klimaci.Services.Concrete
{
    public sealed class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _uow;
        public AdminDashboardService(IUnitOfWork uow) => _uow = uow;

        public async Task<AdminDashboardVM> GetAsync(int days = 30)
        {
            if (days < 1) days = 1;
            if (days > 180) days = 180;

            var nowUtc = DateTime.UtcNow;
            var fromUtc = nowUtc.Date.AddDays(-(days - 1));
            var toUtc = nowUtc.Date;

            var repo = _uow.Repository<NewsletterSubscriber>();

            // Repository'nin döndürdüğü IQueryable'ı kullan (aktifleri getiriyor varsayımıyla).
            // Eğer sende GetAllActives yoksa, Query/All/Table benzeri bir üyeyi kullan.
            var q = (await repo.GetAllActives()).AsNoTracking();

            // EF Core context'i thread-safe değil; paralel Count yapma.
            var total = await q.CountAsync();
            var confirmed = await q.CountAsync(x => x.ConfirmedAt != null && x.UnsubscribedAt == null);
            var unsubscribed = await q.CountAsync(x => x.UnsubscribedAt != null);
            var last24 = await q.CountAsync(x => x.CreatedDate > nowUtc.AddDays(-1));
            var pending = await q.CountAsync(x => x.ConfirmedAt == null && x.UnsubscribedAt == null);

            // Günlük abonelik grafiği
            var dailyRaw = await q
                .Where(x => x.CreatedDate >= fromUtc && x.CreatedDate <= toUtc)
                .GroupBy(x => x.CreatedDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var map = dailyRaw.ToDictionary(k => k.Date, v => v.Count);
            var daily = new List<ChartPointDTO>(days);
            for (int i = 0; i < days; i++)
            {
                var d = fromUtc.AddDays(i);
                daily.Add(new ChartPointDTO(d.ToString("yyyy-MM-dd"), map.TryGetValue(d, out var c) ? c : 0));
            }

            // Son 10 abone
            var recent = await q
                .OrderByDescending(x => x.CreatedDate)
                .Take(10)
                .Select(x => new RecentSubscriberDTO(
                    x.Id, x.Email, x.Name, x.CreatedDate, x.ConfirmedAt, x.UnsubscribedAt))
                .ToListAsync();

            return new AdminDashboardVM
            {
                Counters = new DashboardCountersDTO(
                    TotalSubscribers: total,
                    Confirmed: confirmed,
                    Pending: pending,
                    Unsubscribed: unsubscribed,
                    NewLast24h: last24
                ),
                DailySignups = daily,
                RecentSubscribers = recent
            };
        }
    }
}
