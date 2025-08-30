using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class LeadService : ILeadService
    {
        private readonly IUnitOfWork _uow;
        public LeadService(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<Lead>> PagedAsync(int page, int pageSize, string? search, bool onlyUnprocessed)
        {
            var repo = _uow.Repository<Lead>();

            Expression<Func<Lead, bool>> predicate = x =>
                (string.IsNullOrWhiteSpace(search)
                    || (x.Name != null && x.Name.Contains(search))
                    || (x.Email != null && x.Email.Contains(search))
                    || (x.Phone != null && x.Phone.Contains(search))
                    || (x.Subject != null && x.Subject.Contains(search)))
                && (!onlyUnprocessed || !x.IsProcessed);

            var (items, total) = await repo.PagedAsync(
                page, pageSize,
                predicate,
                q => q.OrderByDescending(x => x.CreatedDate));

            return new PagedResult<Lead>(items, total, page, pageSize);
        }

        public Task<Lead?> GetByIdAsync(Guid id)
            => _uow.Repository<Lead>().GetById(id);

        public async Task ProcessAsync(Guid id, bool isProcessed, string? notes)
        {
            var repo = _uow.Repository<Lead>();
            var e = await repo.GetById(id);
            if (e is null) return;

            e.IsProcessed = isProcessed;
            e.Notes = notes;

            await repo.Update(e);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<Lead>();
            var e = await repo.GetById(id);
            if (e is null) return;

            await repo.Delete(e);
            await _uow.SaveChangesAsync();
        }

        public async Task AddAsync(string name, string? email, string? phone, string? source, string? subject, string? message)
        {
            var repo = _uow.Repository<Lead>();
            var e = new Lead
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Phone = phone,
                Source = source,
                Subject = subject,
                Message = message,
                IsProcessed = false,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            await repo.AddAsync(e);
            await _uow.SaveChangesAsync();
        }
    }
}
