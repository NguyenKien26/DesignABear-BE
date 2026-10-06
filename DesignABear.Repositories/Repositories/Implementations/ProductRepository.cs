using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Paging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<Product>> GetPagedProductsAsync(int pageIndex, int pageSize, string? sortBy = null, bool onlyActive = false)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Media)
                .Include(p => p.Variants) // Updated
                .Include(p => p.Inventories)
                .Include(p => p.DynamicStats)
                .Include(p => p.Reviews);

            if (onlyActive)
            {
                query = query.Where(p => p.IsActive);
            }

            // FILTER: Explicitly exclude accessories that might be in the products table
            query = query.Where(p => !p.Name.Contains("AI Processor") && !p.Sku.Contains("AI-PROCESSOR"));

            // Sorting logic
            query = sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Variants.Min(v => (decimal?)v.Price) ?? 0),
                "price_desc" => query.OrderByDescending(p => p.Variants.Max(v => (decimal?)v.Price) ?? 0),
                "sales_desc" => query.OrderByDescending(p => p.TotalSales),
                "views_desc" => query.OrderByDescending(p => p.DynamicStats != null ? p.DynamicStats.ViewCountIn10Min : 0),
                "rating_desc" => query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task IncrementViewCountAsync(Guid productId)
        {
            // Efficiency: Using atomic SQL increment to avoid fetch-then-update
            // This is also a fallback for cases where direct DB update is desired
            var sql = @"
                INSERT INTO product_dynamic_stats (product_id, view_count_in_10min, discount_rate)
                VALUES (@productId, 1, 0)
                ON CONFLICT (product_id) DO UPDATE 
                SET view_count_in_10min = product_dynamic_stats.view_count_in_10min + 1,
                    updated_at = NOW();";

            await _context.Database.ExecuteSqlRawAsync(sql, new Npgsql.NpgsqlParameter("@productId", productId));
        }

        public async Task BulkIncrementViewCountsAsync(Dictionary<Guid, long> viewCounts)
        {
            if (viewCounts == null || !viewCounts.Any()) return;

            // Using raw SQL for high-performance bulk update in PostgreSQL
            // This avoids N trips to the DB and handles both new and existing stat records
            var ids = viewCounts.Keys.ToArray();
            var deltas = viewCounts.Values.ToArray();

            var sql = @"
                INSERT INTO product_dynamic_stats (product_id, view_count_in_10min, discount_rate)
                SELECT v.product_id, v.delta, 0
                FROM UNNEST(@ids::uuid[], @deltas::int[]) AS v(product_id, delta)
                JOIN products p ON p.product_id = v.product_id
                ON CONFLICT (product_id) DO UPDATE 
                SET view_count_in_10min = product_dynamic_stats.view_count_in_10min + EXCLUDED.view_count_in_10min;";

            await _context.Database.ExecuteSqlRawAsync(sql, 
                new Npgsql.NpgsqlParameter("@ids", ids),
                new Npgsql.NpgsqlParameter("@deltas", deltas));
        }

        public async Task<Product?> GetBySlugAsync(string slug)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants)
                .Include(p => p.Inventories)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.ProductCollections).ThenInclude(pc => pc.Collection)
                .Include(p => p.ProductCharacters).ThenInclude(pc => pc.Character)
                .Include(p => p.ProductAccessories).ThenInclude(pa => pa.Accessory)
                .Include(p => p.Reviews)
                .Include(p => p.ComboImages)
                .FirstOrDefaultAsync(p => p.Slug == slug);

        public async Task<Product?> GetWithDetailsAsync(Guid productId)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants)
                .Include(p => p.Inventories)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.ProductCollections).ThenInclude(pc => pc.Collection)
                .Include(p => p.ProductCharacters).ThenInclude(pc => pc.Character)
                .Include(p => p.ProductAccessories).ThenInclude(pa => pa.Accessory)
                .Include(p => p.Reviews) // Add Reviews Include
                .Include(p => p.ComboImages)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

        public async Task<Product?> GetFullDetailsAsync(Guid productId)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants)
                .Include(p => p.Inventories).ThenInclude(i => i.Location)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.ProductCollections).ThenInclude(pc => pc.Collection)
                .Include(p => p.ProductCharacters).ThenInclude(pc => pc.Character)
                .Include(p => p.ProductAccessories).ThenInclude(pa => pa.Accessory)
                .Include(p => p.BasePersonalizationRules).ThenInclude(pr => pr.AddonProduct)
                .Include(p => p.BasePersonalizationRules).ThenInclude(pr => pr.Group)
                .Include(p => p.AddonPersonalizationRules).ThenInclude(pr => pr.BaseProduct)
                .Include(p => p.AddonPersonalizationRules).ThenInclude(pr => pr.Group)
                .Include(p => p.Reviews)
                .Include(p => p.ComboImages)
                .Include(p => p.DynamicStats)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants) // Updated
                .Include(p => p.Inventories)
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCollectionAsync(Guid collectionId)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants) // Updated
                .Include(p => p.Inventories)
                .Where(p => p.ProductCollections.Any(pc => pc.CollectionId == collectionId))
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCharacterAsync(Guid characterId)
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants) // Updated
                .Include(p => p.Inventories)
                .Where(p => p.ProductCharacters.Any(pc => pc.CharacterId == characterId))
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
            => await _dbSet
                .Include(p => p.Media)
                .Include(p => p.Variants) // Updated
                .Include(p => p.Inventories)
                .Where(p => p.IsActive)
                .ToListAsync();

        public async Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql,
                parameters.Select((p, i) => new Npgsql.NpgsqlParameter($"@p{i}", p)).ToArray());
        }
    }
}
