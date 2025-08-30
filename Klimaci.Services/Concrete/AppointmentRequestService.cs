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
    public class AppointmentRequestService : IAppointmentRequestService
    {
        private readonly IUnitOfWork _uow;
        public AppointmentRequestService(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<AppointmentRequest>> PagedAsync(int page, int pageSize, string? search, bool onlyUnprocessed)
        {
            var repo = _uow.Repository<AppointmentRequest>();

            Expression<Func<AppointmentRequest, bool>> predicate = x =>
                (string.IsNullOrWhiteSpace(search)
                    || (x.Name != null && x.Name.Contains(search))
                    || (x.Email != null && x.Email.Contains(search))
                    || (x.Phone != null && x.Phone.Contains(search)))
                && (!onlyUnprocessed || !x.IsProcessed);

            var (items, total) = await repo.PagedAsync(
                page, pageSize,
                predicate,
                q => q.OrderByDescending(x => x.CreatedDate));

            return new PagedResult<AppointmentRequest>(items, total, page, pageSize);
        }

        public Task<AppointmentRequest?> GetByIdAsync(Guid id)
            => _uow.Repository<AppointmentRequest>().GetById(id);

        public async Task ProcessAsync(Guid id, bool isProcessed, string? notes)
        {
            var repo = _uow.Repository<AppointmentRequest>();
            var e = await repo.GetById(id);
            if (e == null) return;

            e.IsProcessed = isProcessed;
            e.Notes = notes;

            await repo.Update(e);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<AppointmentRequest>();
            var e = await repo.GetById(id);
            if (e == null) return;

            await repo.Delete(e);
            await _uow.SaveChangesAsync();
        }
    }
}
