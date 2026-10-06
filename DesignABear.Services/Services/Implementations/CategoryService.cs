using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public CategoryService(IUnitOfWork uow, IMapper mapper, ICacheService cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid categoryId, bool includeInactive = false)
        {
            string cacheKey = $"category:id:{categoryId}";
            
            // Only cache active categories
            if (!includeInactive)
            {
                var cached = await _cache.GetAsync<CategoryResponse>(cacheKey);
                if (cached != null) return cached;
            }

            var category = await _uow.Categories.GetByIdAsync(categoryId);
            if (category == null || (!includeInactive && !category.IsActive))
            {
                throw new NotFoundException("Category", categoryId);
            }
            
            var response = _mapper.Map<CategoryResponse>(category);
            
            if (!includeInactive && category.IsActive)
            {
                await _cache.SetAsync(cacheKey, response, TimeSpan.FromHours(1));
            }

            return response;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(bool includeInactive = false)
        {
            string cacheKey = "categories:all";
            
            if (!includeInactive)
            {
                var cached = await _cache.GetAsync<IEnumerable<CategoryResponse>>(cacheKey);
                if (cached != null) return cached;
            }

            var categories = await _uow.Categories.GetAllAsync();
            if (!includeInactive)
            {
                categories = categories.Where(c => c.IsActive).ToList();
            }
            
            var result = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

            if (!includeInactive)
            {
                 await _cache.SetAsync(cacheKey, result, TimeSpan.FromHours(1));
            }

            return result;
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            if (request.ParentId.HasValue)
            {
                var parentExists = await _uow.Categories.AnyAsync(c => c.CategoryId == request.ParentId.Value);
                if (!parentExists) throw new BadRequestException($"Parent Category {request.ParentId.Value} not found.");
            }

            var category = _mapper.Map<Category>(request);
            category.CategoryId = Guid.NewGuid();
            category.IsActive = true;

            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();

            await InvalidateCategoryCacheAsync(category.CategoryId);

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task UpdateAsync(Guid categoryId, UpdateCategoryRequest request)
        {
            var category = await _uow.Categories.GetByIdAsync(categoryId);
            if (category == null) throw new NotFoundException("Category", categoryId);

            if (request.ParentId.HasValue)
            {
                if (request.ParentId.Value == categoryId)
                {
                    throw new BadRequestException("A category cannot be its own parent.");
                }

                var parentExists = await _uow.Categories.AnyAsync(c => c.CategoryId == request.ParentId.Value);
                if (!parentExists) throw new BadRequestException($"Parent Category {request.ParentId.Value} not found.");
            }

            _mapper.Map(request, category);
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();

            await InvalidateCategoryCacheAsync(categoryId);
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _uow.Categories.GetByIdAsync(categoryId);
            if (category == null) throw new NotFoundException("Category", categoryId);

            await DeactivateRecursiveAsync(category);
            await _uow.SaveChangesAsync();

            await InvalidateCategoryCacheAsync(categoryId);
        }

        public async Task RestoreAsync(Guid categoryId)
        {
            var category = await _uow.Categories.GetByIdAsync(categoryId);
            if (category == null) throw new NotFoundException("Category", categoryId);

            category.IsActive = true;
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();

            await InvalidateCategoryCacheAsync(categoryId);
        }

        private async Task InvalidateCategoryCacheAsync(Guid categoryId)
        {
            await _cache.RemoveAsync("categories:all");
            await _cache.RemoveAsync($"category:id:{categoryId}");
            
            // Cross-service invalidation: Any category change might affect product lists
            await _cache.RemoveByPrefixAsync("products:page:");
        }

        private async Task DeactivateRecursiveAsync(Category category)
        {
            category.IsActive = false;
            _uow.Categories.Update(category);

            var children = await _uow.Categories.GetChildrenAsync(category.CategoryId);
            foreach (var child in children)
            {
                await DeactivateRecursiveAsync(child);
            }
        }
    }
}
