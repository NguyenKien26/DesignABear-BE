using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesignABear.Services.Services.Implementations
{
    public class BuildService : IBuildService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BuildService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<BuildResponse?> GetByIdAsync(Guid buildId)
        {
            var build = await _uow.Builds.GetByIdAsync(buildId);
            if (build == null) throw new NotFoundException("Build", buildId);
            return _mapper.Map<BuildResponse>(build);
        }

        public async Task<BuildResponse?> GetWithComponentsAsync(Guid buildId)
        {
            var build = await _uow.Builds.GetWithComponentsAsync(buildId);
            if (build == null) throw new NotFoundException("Build", buildId);
            return _mapper.Map<BuildResponse>(build);
        }

        public async Task<IEnumerable<BuildResponse>> GetByUserIdAsync(Guid userId)
        {
            var builds = await _uow.Builds.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BuildResponse>>(builds);
        }

        public async Task<BuildResponse> CreateAsync(CreateBuildRequest request)
        {
            var build = _mapper.Map<Build>(request);
            build.BuildId = Guid.NewGuid();
            build.CreatedAt = DateTime.UtcNow;
            build.UpdatedAt = DateTime.UtcNow;
            build.Status = "DRAFT";

            // IMPROVEMENT: If variant is known, sync its details to build table for easier tracking
            if (build.BaseVariantId.HasValue && (string.IsNullOrEmpty(build.SizeTag) || string.IsNullOrEmpty(build.SizeDescription)))
            {
                var variant = await _uow.ProductVariants.GetByIdAsync(build.BaseVariantId.Value);
                if (variant != null)
                {
                    if (string.IsNullOrEmpty(build.SizeTag)) build.SizeTag = variant.SizeTag;
                    if (string.IsNullOrEmpty(build.SizeDescription)) build.SizeDescription = variant.SizeDescription;
                }
            }

            // Fallback for manually provided tags
            if (string.IsNullOrEmpty(build.SizeTag) && !string.IsNullOrEmpty(request.SizeTag)) build.SizeTag = request.SizeTag;
            if (string.IsNullOrEmpty(build.SizeDescription) && !string.IsNullOrEmpty(request.SizeDescription)) build.SizeDescription = request.SizeDescription;
            
            await _uow.Builds.AddAsync(build);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<BuildResponse>(build);
        }

        public async Task<BuildComponentResponse> AddComponentAsync(CreateBuildComponentRequest request)
        {
            var build = await _uow.Builds.GetByIdAsync(request.BuildId);
            if (build == null) throw new NotFoundException("Build", request.BuildId);

            var component = _mapper.Map<BuildComponent>(request);
            component.BuildComponentId = Guid.NewGuid();
            
            await _uow.BuildComponents.AddAsync(component);
            await _uow.SaveChangesAsync();
            await CalculatePriceAsync(request.BuildId);

            return _mapper.Map<BuildComponentResponse>(component);
        }

        public async Task RemoveComponentAsync(Guid buildComponentId)
        {
            var component = await _uow.BuildComponents.GetByIdAsync(buildComponentId);
            if (component is not null)
            {
                _uow.BuildComponents.Remove(component);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Guid buildId, UpdateBuildRequest request)
        {
            var build = await _uow.Builds.GetByIdAsync(buildId);
            if (build == null) throw new NotFoundException("Build", buildId);

            _mapper.Map(request, build);
            build.UpdatedAt = DateTime.UtcNow;
            
            _uow.Builds.Update(build);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid buildId)
        {
            var build = await _uow.Builds.GetByIdAsync(buildId);
            if (build is not null)
            {
                _uow.Builds.Remove(build);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<decimal> CalculatePriceAsync(Guid buildId)
        {
            var build = await _uow.Builds.GetWithComponentsAsync(buildId);
            if (build is null) throw new NotFoundException("Build", buildId);

            var baseProduct = await _uow.Products.GetWithDetailsAsync(build.BaseProductId);
            var baseVariant = baseProduct?.Variants.FirstOrDefault(v => v.IsActive);
            var basePrice = baseVariant?.Price ?? 0;
            var baseWeight = baseVariant?.WeightGram ?? 0;
            
            var addonsPrice = build.BuildComponents.Sum(c => c.AdditionalPrice ?? 0);
            var addonsWeight = build.BuildComponents.Sum(c => (c.OptionProduct?.Variants.FirstOrDefault()?.WeightGram == 0 ? 500f : c.OptionProduct?.Variants.FirstOrDefault()?.WeightGram ?? 500f) * c.Quantity);

            var total = basePrice + addonsPrice;
            
            if (build.IncludesSmartChip && baseProduct != null)
            {
                var smartChip = await _uow.Accessories.GetQueryable()
                    .FirstOrDefaultAsync(a => a.Sku == "SMART-CHIP-001" && a.IsActive);
                total += smartChip?.TargetPrice ?? 999000;
            }

            build.CalculatedPrice = total;
            build.TotalWeightGram = baseWeight + (float)addonsWeight;
            
            build.UpdatedAt = DateTime.UtcNow;
            _uow.Builds.Update(build);
            await _uow.SaveChangesAsync();
            return total;
        }
    }
}
