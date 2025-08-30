using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO.NewsletterDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class NewsletterService : INewsletterService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;
        public NewsletterService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }
        public async Task<(bool ok, string? error)> SubscribeAsync(NewsletterSubscribeDTO dto)
        {
            // 1) Basit doğrulamalar + normalize
            var email = dto.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Geçersiz e-posta.");

            var name = string.IsNullOrWhiteSpace(dto.Name) ? null : dto.Name!.Trim();

            var repo = _uow.Repository<NewsletterSubscriber>();

            // 2) Soft-delete dahil hepsine bak
            var existing = await _ctx.Set<NewsletterSubscriber>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Email == email);

            // 3) Token oluşturucu
            static (string token, DateTime expiresAt) NewToken()
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(32);
                return (Convert.ToBase64String(tokenBytes), DateTime.UtcNow.AddHours(24));
            }

            if (existing is null)
            {
                // Yeni kayıt: aktif, onay bekliyor
                var (token, expires) = NewToken();

                await repo.AddAsync(new NewsletterSubscriber
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Name = name,
                    ConfirmedAt = null,
                    UnsubscribedAt = null,
                    ConfirmToken = token,
                    ConfirmTokenExpiresAt = expires,
                    Status = Core.Enums.Status.Active
                });
            }
            else
            {
                // Varsa güncelle: tekrar abone oluyorsa çıkışı kaldır, adı tazele
                existing.Email = email; // normalize edilmiş hali sabitle
                if (!string.IsNullOrWhiteSpace(name))
                    existing.Name = name;

                existing.Status = Core.Enums.Status.Active;
                existing.UnsubscribedAt = null;

                // Zaten onaylıysa token üretme; onaysızsa yeni token ver
                if (existing.ConfirmedAt is null)
                {
                    var (token, expires) = NewToken();
                    existing.ConfirmToken = token;
                    existing.ConfirmTokenExpiresAt = expires;
                }
                else
                {
                    // Onaylı abonelik: token gereksiz
                    existing.ConfirmToken = null;
                    existing.ConfirmTokenExpiresAt = null;
                }

                await repo.Update(existing);
            }

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Yarış durumu: aynı anda iki istek geldiyse
                // Unique index patlarsa "update"e düşelim
                var again = await _ctx.Set<NewsletterSubscriber>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (again is null) throw;

                if (!string.IsNullOrWhiteSpace(name))
                    again.Name = name;

                again.Status = Core.Enums.Status.Active;
                again.UnsubscribedAt = null;

                if (again.ConfirmedAt is null)
                {
                    var (token, expires) = NewToken();
                    again.ConfirmToken = token;
                    again.ConfirmTokenExpiresAt = expires;
                }
                else
                {
                    again.ConfirmToken = null;
                    again.ConfirmTokenExpiresAt = null;
                }

                await repo.Update(again);
                await _uow.SaveChangesAsync();
            }

            // Burada onay mailini gönder:
            // var confirmUrl = $"{_app.BaseUrl}/newsletter/confirm?id={e.Id}&token={Uri.EscapeDataString(e.ConfirmToken)}";
            // await _mail.SendAsync(...);

            return (true, null);
        }
        public Task<NewsletterSubscriber?> GetByIdAsync(Guid id) => _uow.Repository<NewsletterSubscriber>().GetById(id);
        public async Task UpdateAsync(NewsletterUpdateDTO dto)
        {
            var repo = _uow.Repository<NewsletterSubscriber>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            e.Email = dto.Email; e.Name = dto.Name; await repo.Update(e); await _uow.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<NewsletterSubscriber>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }
        public async Task<PagedResult<NewsletterSubscriber>> PagedAsync(
     int page = 1, int pageSize = 40, string? search = null, bool onlyActive = false)
        {
            var repo = _uow.Repository<NewsletterSubscriber>();
            var q = (await repo.GetAllActives());

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(x => x.Email.Contains(search) || (x.Name ?? "").Contains(search));

            if (onlyActive)
                q = q.Where(x => x.ConfirmedAt != null && x.UnsubscribedAt == null);

            var total = await q.CountAsync();
            var items = await q
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<NewsletterSubscriber>(items, page, pageSize, total);
        }

        public async Task<(bool ok, string? error)> UnsubscribeAsync(Guid id)
        {
            var repo = _uow.Repository<NewsletterSubscriber>();
            var e = await repo.GetById(id);
            if (e is null) return (false, "Kayıt bulunamadı.");
            e.UnsubscribedAt = DateTime.UtcNow;
            await repo.Update(e);
            await _uow.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool ok, string? error)> ConfirmAsync(Guid id, string token)
        {
            var repo = _uow.Repository<NewsletterSubscriber>();
            var e = await repo.GetById(id);
            if (e is null) return (false, "Kayıt bulunamadı.");

            if (string.IsNullOrWhiteSpace(e.ConfirmToken) || string.IsNullOrWhiteSpace(token))
                return (false, "Geçersiz onay isteği.");

            if (!string.Equals(e.ConfirmToken, token, StringComparison.Ordinal))
                return (false, "Onay anahtarı hatalı.");

            if (e.ConfirmTokenExpiresAt is null || e.ConfirmTokenExpiresAt < DateTime.UtcNow)
                return (false, "Onay linkinin süresi dolmuş.");

            e.ConfirmedAt = DateTime.UtcNow;
            e.UnsubscribedAt = null;           // varsa çıkışı temizle
            e.ConfirmToken = null;             // tek kullanımlık
            e.ConfirmTokenExpiresAt = null;

            await repo.Update(e);
            await _uow.SaveChangesAsync();
            return (true, null);
        }
        public async Task<(bool ok, string? error)> AdminConfirmAsync(Guid id)
        {
            var repo = _uow.Repository<NewsletterSubscriber>();
            var e = await repo.GetById(id);
            if (e is null) return (false, "Kayıt bulunamadı.");

            e.ConfirmedAt = DateTime.UtcNow;
            e.UnsubscribedAt = null;
            e.ConfirmToken = null;
            e.ConfirmTokenExpiresAt = null;

            await repo.Update(e);
            await _uow.SaveChangesAsync();
            return (true, null);
        }
    }
}
