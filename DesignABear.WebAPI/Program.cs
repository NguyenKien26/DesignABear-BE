using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.Repositories.Implementations;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Services.Services.Implementations;
using DesignABear.Repositories.UnitOfWork;
using StackExchange.Redis;
using DesignABear.WebAPI.Workers;
using DesignABear.Services.Configurations;

namespace DesignABear.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Clear default claim mapping to keep JWT claim names as is (e.g. "id")
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddControllers();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                // Allow sending JWT in Swagger UI
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name         = "Authorization",
                    Type         = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme       = "Bearer",
                    BearerFormat = "JWT",
                    In           = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description  = "Enter: Bearer {your JWT token}"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Include XML comments for Swagger
                var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // -- Supabase -------------------------------------------
            var supabaseUrl = builder.Configuration["Supabase:Url"]?.Trim() ?? throw new InvalidOperationException("Supabase:Url is not configured.");
            var supabaseServiceKey = builder.Configuration["Supabase:ServiceRoleKey"]?.Trim() ?? throw new InvalidOperationException("Supabase:ServiceRoleKey is not configured.");
            var supabaseOptions = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = false,
                AutoConnectRealtime = false
            };
            builder.Services.AddSingleton(provider => new Supabase.Client(supabaseUrl, supabaseServiceKey, supabaseOptions));

            // -- Database -------------------------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                          .EnableRetryOnFailure()));

            // -- Redis ----------------------------------------------
            var redisConfig = builder.Configuration.GetSection("Redis");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig["Configuration"];
                options.InstanceName = redisConfig["InstanceName"];
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(redisConfig["Configuration"]!);
                options.AbortOnConnectFail = false; // Allow app to start even if Redis is down
                return ConnectionMultiplexer.Connect(options);
            });

            // -- Business Settings ----------------------------------
            builder.Services.Configure<BusinessSettings>(builder.Configuration.GetSection("Business"));
            builder.Services.Configure<GhtkSettings>(builder.Configuration.GetSection("GhtkSettings"));
            builder.Services.AddMemoryCache();

            // -- AutoMapper -----------------------------------------
            builder.Services.AddAutoMapper(new[] { typeof(Program).Assembly, typeof(DesignABear.Services.Mappings.MappingProfile).Assembly });

            // -- JWT Authentication ---------------------------------
            var jwtKey = builder.Configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
                        ValidAudience            = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

            // -- Repositories ---------------------------------------
            builder.Services.AddRepositories();

            // -- Unit of Work ---------------------------------------
            builder.Services.AddScoped<IUnitOfWork, DesignABear.Repositories.UnitOfWork.UnitOfWork>();

            // -- Auth Services --------------------------------------
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // -- Domain Services ------------------------------------
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICollectionService, CollectionService>();
            builder.Services.AddScoped<ICharacterService, CharacterService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAccessoryService, AccessoryService>();

            builder.Services.AddScoped<IProductMediaService, ProductMediaService>();
            builder.Services.AddScoped<IPersonalizationGroupService, PersonalizationGroupService>();
            builder.Services.AddScoped<IPersonalizationRuleService, PersonalizationRuleService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IBuildService, BuildService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPromotionService, PromotionService>();
            builder.Services.AddScoped<IPayOSService, PayOSService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IFulfillmentService, FulfillmentService>();
            builder.Services.AddScoped<IProductionJobService, ProductionJobService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IRefundService, RefundService>();
            builder.Services.AddScoped<IProductIssueReportService, ProductIssueReportService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<ICacheService, RedisCacheService>();
            builder.Services.AddHostedService<ViewCountSyncWorker>();

            // -- GHTK Shipping --------------------------------------
            var ghtkBaseUrl = builder.Configuration["GHTK:BaseUrl"] ?? "https://services.giaohangtietkiem.vn";
            var ghtkToken = builder.Configuration["GHTK:Token"] ?? throw new InvalidOperationException("GHTK:Token is not configured.");
            var ghtkPartnerCode = builder.Configuration["GHTK:PartnerCode"] ?? throw new InvalidOperationException("GHTK:PartnerCode is not configured.");

            builder.Services.AddHttpClient<IGhtkRepository, GhtkRepository>(client =>
            {
                client.BaseAddress = new Uri(ghtkBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("Token", ghtkToken);
                client.DefaultRequestHeaders.Add("X-Client-Source", ghtkPartnerCode);
            });

            builder.Services.AddScoped<IGhtkShippingService, GhtkShippingService>();

            var app = builder.Build();

            // -- HTTP Pipeline --------------------------------------
            if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Global Exception Handler
            app.UseMiddleware<DesignABear.WebAPI.Middlewares.ExceptionMiddleware>();

            app.UseCors("AllowAll");

            // app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // -- Cache Warmup ---------------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("\x1b[35m[Cache] Warming up product cache...\x1b[0m");
                    
                    // Step 1: Flush all stale cached data from previous runs
                    var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    await cacheService.FlushAllCachedDataAsync();

                    // Step 2: Warm up with fresh data from DB
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    
                    // Warm up first page and top products
                    await productService.GetProductsAsync(1, 10);
                    await productService.GetTopProductsAsync();

                    // Warm up categories list
                    var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
                    await categoryService.GetAllAsync();
                    
                    logger.LogInformation("\x1b[35m[Cache] Warmup complete.\x1b[0m");

                    logger.LogInformation("\x1b[35m[Cache] Warmup complete.\x1b[0m");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "[Cache] Failed to warm up cache during startup.");
                }
            }

            // -- Database Migration ----------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Checking for database schema updates...");
                    var context = services.GetRequiredService<AppDbContext>();
                    
                    string sql = @"
                        ALTER TABLE production_jobs 
                        ADD COLUMN IF NOT EXISTS esp_technician_id uuid,
                        ADD COLUMN IF NOT EXISTS esp_status varchar(30) DEFAULT 'IN_QUEUE',
                        ADD COLUMN IF NOT EXISTS esp_finished_at timestamptz,
                        ADD COLUMN IF NOT EXISTS esp_handover_photo_urls varchar(1000),
                        ADD COLUMN IF NOT EXISTS esp_handover_note varchar(500),
                        ADD COLUMN IF NOT EXISTS shell_technician_id uuid,
                        ADD COLUMN IF NOT EXISTS shell_status varchar(30) DEFAULT 'IN_QUEUE',
                        ADD COLUMN IF NOT EXISTS shell_finished_at timestamptz,
                        ADD COLUMN IF NOT EXISTS shell_handover_photo_urls varchar(1000),
                        ADD COLUMN IF NOT EXISTS shell_handover_note varchar(500),
                        ADD COLUMN IF NOT EXISTS qc_inspector_id uuid,
                        ADD COLUMN IF NOT EXISTS qc_status varchar(30) DEFAULT 'IN_QUEUE',
                        ADD COLUMN IF NOT EXISTS qc_finished_at timestamptz,
                        ADD COLUMN IF NOT EXISTS qc_handover_photo_urls varchar(1000),
                        ADD COLUMN IF NOT EXISTS qc_handover_note varchar(500),
                        ADD COLUMN IF NOT EXISTS total_assembly_cost numeric(18,4) DEFAULT 0,
                        ADD COLUMN IF NOT EXISTS craftsman_commission numeric(18,4) DEFAULT 0,
                        ADD COLUMN IF NOT EXISTS qc_commission numeric(18,4) DEFAULT 0,
                        ADD COLUMN IF NOT EXISTS evidence_images jsonb;
                    ";
                    
                    await context.Database.ExecuteSqlRawAsync(sql);
                    logger.LogInformation("Database schema unified successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            app.Run();
        }
    }
}
