using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO;
using Klimaci.DTO.OfficeDTOs;
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
    public class OfficeService : IOfficeService
    {
        private readonly IUnitOfWork _uow;
        public OfficeService(IUnitOfWork uow) { _uow = uow; }

        public async Task Add(OfficeAddDTO dto)
        {
            var repo = _uow.Repository<Office>(); var e = dto.Adapt<Office>(); e.Id = Guid.NewGuid();
            await repo.AddAsync(e); await _uow.SaveChangesAsync();
        }

        public async Task Update(OfficeUpdateDTO dto)
        {
            var repo = _uow.Repository<Office>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e); await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var repo = _uow.Repository<Office>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }

        public Task<List<Office>> GetAll() => _uow.Repository<Office>().GetAll();
        public Task<Office> GetById(Guid id) => _uow.Repository<Office>().GetById(id);

        public async Task<PagedResult<Office>> PagedAsync(int page = 1, int pageSize = 30, string? search = null)
        {
            Expression<Func<Office, bool>> pred = x => string.IsNullOrWhiteSpace(search) || x.Title.Contains(search) || (x.City ?? "").Contains(search);
            var (items, total) = await _uow.Repository<Office>().PagedAsync(page, pageSize, pred, iq => iq.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Title));
            return new(items, total, page, pageSize);
        }

        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items)
        {
            var repo = _uow.Repository<Office>();
            foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>())
            {
                var e = await repo.GetById(it.Id); if (e is null) continue;
                e.DisplayOrder = it.DisplayOrder; await repo.Update(e);
            }
            await _uow.SaveChangesAsync();
        }
    }
}
