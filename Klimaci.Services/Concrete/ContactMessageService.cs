using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO.ContactMessageDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IUnitOfWork _uow;
        public ContactMessageService(IUnitOfWork uow) { _uow = uow; }

        public async Task AddAsync(ContactMessageAddDTO dto, string? ipAddress)
        {
            var repo = _uow.Repository<ContactMessage>();
            var e = dto.Adapt<ContactMessage>();
            e.Id = Guid.NewGuid();
            e.IsProcessed = false;
            e.IpAddress = ipAddress;
            await repo.AddAsync(e);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResult<ContactMessage>> PagedAsync(int page = 1, int pageSize = 20, string? search = null, bool onlyUnprocessed = false)
        {
            Expression<Func<ContactMessage, bool>> pred = x =>
                (string.IsNullOrWhiteSpace(search) || x.Name.Contains(search) || x.Email.Contains(search) || (x.Subject ?? "").Contains(search))
                && (!onlyUnprocessed || !x.IsProcessed);

            Func<IQueryable<ContactMessage>, IOrderedQueryable<ContactMessage>> order = iq => iq.OrderByDescending(x => x.CreatedDate);
            var (items, total) = await _uow.Repository<ContactMessage>().PagedAsync(page, pageSize, pred, order);
            return new(items, total, page, pageSize);
        }

        public Task<ContactMessage?> GetByIdAsync(Guid id) => _uow.Repository<ContactMessage>().GetById(id);

        public async Task ProcessAsync(ContactProcessDTO dto)
        {
            var repo = _uow.Repository<ContactMessage>();
            var e = await repo.GetById(dto.Id); if (e is null) return;
            e.IsProcessed = dto.IsProcessed;
            e.ProcessedAt = dto.IsProcessed ? DateTime.UtcNow : null;
            e.Notes = dto.Notes;
            await repo.Update(e);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<ContactMessage>();
            var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e);
            await _uow.SaveChangesAsync();
        }
    }
}
