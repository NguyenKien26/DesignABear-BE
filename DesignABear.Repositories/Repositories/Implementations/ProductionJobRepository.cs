using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ProductionJobRepository : GenericRepository<ProductionJob>, IProductionJobRepository
    {
        public ProductionJobRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductionJob>> GetByStatusAsync(string status)
            => await _dbSet
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(j => j.Technician)
                .Include(j => j.Build)
                    .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p.Media)
                .Include(j => j.Build).ThenInclude(b => b!.BaseVariant)
                .Include(j => j.Build).ThenInclude(b => b!.BuildComponents).ThenInclude(bc => bc.OptionProduct)
                .Where(j => j.Status == status)
                .ToListAsync();

        public async Task<IEnumerable<ProductionJob>> GetByTechnicianAsync(Guid technicianId)
            => await _dbSet
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(j => j.Build)
                    .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p.Media)
                .Include(j => j.Build).ThenInclude(b => b!.BaseVariant)
                .Include(j => j.Build).ThenInclude(b => b!.BuildComponents).ThenInclude(bc => bc.OptionProduct)
                .Where(j => j.TechnicianId == technicianId)
                .ToListAsync();

        public async Task<ProductionJob?> GetByOrderItemAsync(Guid orderItemId)
            => await _dbSet
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(j => j.Build)
                    .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p.Media)
                .Include(j => j.Build).ThenInclude(b => b!.BaseVariant)
                .Include(j => j.Build).ThenInclude(b => b!.BuildComponents).ThenInclude(bc => bc.OptionProduct)
                .FirstOrDefaultAsync(j => j.OrderItemId == orderItemId);

        public override async Task<ProductionJob?> GetByIdAsync(object id)
            => await _dbSet
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseProduct)
                            .ThenInclude(p => p.Media)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BaseVariant)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b!.BuildComponents)
                            .ThenInclude(bc => bc.OptionProduct)
                .Include(j => j.OrderItem)
                    .ThenInclude(oi => oi.Variant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Media)
                .Include(j => j.Technician)
                .Include(j => j.Build)
                    .ThenInclude(b => b!.BaseProduct)
                        .ThenInclude(p => p.Media)
                .Include(j => j.Build).ThenInclude(b => b!.BaseVariant)
                .Include(j => j.Build).ThenInclude(b => b!.BuildComponents).ThenInclude(bc => bc.OptionProduct)
                .FirstOrDefaultAsync(j => j.JobId == (Guid)id);
    }
}
