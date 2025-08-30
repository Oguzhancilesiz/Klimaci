using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO.SettingDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;
        public SettingService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        public async Task<PagedResult<Setting>> PagedAsync(int page = 1, int pageSize = 30, string? group = null, string? search = null)
        {
            Expression<Func<Setting, bool>> pred = x =>
                (string.IsNullOrWhiteSpace(group) || x.Group == group)
                && (string.IsNullOrWhiteSpace(search) || x.Key.Contains(search) || (x.Value ?? "").Contains(search));

            var (items, total) = await _uow.Repository<Setting>().PagedAsync(page, pageSize, pred, iq => iq.OrderBy(x => x.Group).ThenBy(x => x.Key));
            return new(items, total, page, pageSize);
        }

        public Task<Setting?> GetByIdAsync(Guid id) => _uow.Repository<Setting>().GetById(id);

        public async Task<string?> GetValueAsync(string key)
        {
            var q = await _uow.Repository<Setting>().GetBy(x => x.Key == key);
            return (await q.Select(x => x.Value).FirstOrDefaultAsync());
        }

        public async Task UpsertAsync(SettingUpsertDTO dto)
        {
            var repo = _uow.Repository<Setting>();
            // ignoreQueryFilter: Deleted kayıt varsa diriltelim
            var existing = await _ctx.Set<Setting>().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Key == dto.Key);
            if (existing is null)
            {
                var e = dto.Adapt<Setting>(); e.Id = Guid.NewGuid();
                await repo.AddAsync(e);
            }
            else
            {
                existing.Value = dto.Value;
                existing.Group = dto.Group;
                existing.Description = dto.Description;
                existing.Status = Core.Enums.Status.Active;
                await repo.Update(existing);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task UpsertManyAsync(IEnumerable<SettingUpsertDTO> items)
        {
            foreach (var it in items) await UpsertAsync(it);
        }

        public async Task UpdateAsync(SettingUpdateDTO dto)
        {
            var repo = _uow.Repository<Setting>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            // Key değişiyorsa unique ihlal olmasın
            if (!string.Equals(e.Key, dto.Key, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _ctx.Set<Setting>().AnyAsync(x => x.Key == dto.Key);
                if (exists) throw new InvalidOperationException("Key zaten var.");
                e.Key = dto.Key;
            }
            e.Value = dto.Value; e.Group = dto.Group; e.Description = dto.Description;
            await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<Setting>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }
    }
}
