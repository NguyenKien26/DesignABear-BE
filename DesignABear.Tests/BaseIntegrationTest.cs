using AutoMapper;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using Xunit;
using Microsoft.Extensions.Caching.Distributed;

namespace DesignABear.Tests
{
    [Collection("SharedContainerCollection")]
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        protected readonly TestDatabaseFixture _fixture;
        protected readonly IServiceScope _scope;
        protected readonly AppDbContext _context;
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _cache;
        protected readonly IDistributedCache _distributedCache;
        protected readonly IConnectionMultiplexer _redis;

        protected BaseIntegrationTest(TestDatabaseFixture fixture)
        {
            _fixture = fixture;

            var services = new ServiceCollection();

            // DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_fixture.DbConnectionString));

            // Repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddRepositories();

            // Mapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DesignABear.Services.Mappings.MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            services.AddSingleton(_mapper);

            // Logging
            services.AddLogging(b => b.AddConsole());

            // Cache
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(_fixture.RedisConnectionString));

            var provider = services.BuildServiceProvider();
            _scope = provider.CreateScope();
            
            _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _uow = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _cache = _scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            _distributedCache = _scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            _redis = _scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _fixture.ResetDatabaseAsync();
            _scope.Dispose();
        }
    }
}
