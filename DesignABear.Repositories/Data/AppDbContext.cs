using Microsoft.EntityFrameworkCore;
using DesignABear.Repositories.Models;

namespace DesignABear.Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Auth
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        // Catalogue
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<ProductAccessory> ProductAccessories { get; set; }
        public DbSet<ProductDynamicStats> ProductDynamicStats { get; set; }
        public DbSet<ProductMedia> ProductMedias { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCollection> ProductCollections { get; set; }
        public DbSet<ProductCharacter> ProductCharacters { get; set; }
        public DbSet<PersonalizationGroup> PersonalizationGroups { get; set; }
        public DbSet<PersonalizationRule> PersonalizationRules { get; set; }
        public DbSet<ProductComboImage> ProductComboImages { get; set; }

        // Build & Inventory
        public DbSet<Build> Builds { get; set; }
        public DbSet<BuildComponent> BuildComponents { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        // Commerce
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderPromotion> OrderPromotions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Fulfillment> Fulfillments { get; set; }
        public DbSet<OrderRefund> OrderRefunds { get; set; }
        public DbSet<ProductIssueReport> ProductIssueReports { get; set; }

        // Production
        public DbSet<ProductionJob> ProductionJobs { get; set; }

        // Social
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewReply> ReviewReplies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        // D. Shadow Properties — auto-stamp CreatedAt / UpdatedAt on SaveChanges
        public override int SaveChanges()
        {
            StampAuditDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            StampAuditDates();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void StampAuditDates()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                        entry.Property("CreatedAt").CurrentValue = now;
                    if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                        entry.Property("UpdatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                        entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sequential GUIDs — avoids page splits on clustered indexes
            modelBuilder.Entity<User>().Property(u => u.UserId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<UserToken>().Property(ut => ut.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Address>().Property(a => a.AddressId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Build>().Property(b => b.BuildId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Cart>().Property(c => c.CartId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Order>().Property(o => o.OrderId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.OrderItemId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Payment>().Property(p => p.PaymentId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Fulfillment>().Property(f => f.FulfillmentId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ProductionJob>().Property(j => j.JobId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Product>().Property(p => p.ProductId).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<ProductMedia>().Property(pm => pm.MediaId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Review>().Property(r => r.ReviewId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ReviewReply>().Property(rr => rr.ReplyId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Promotion>().Property(p => p.PromotionId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<PersonalizationGroup>().Property(g => g.GroupId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<PersonalizationRule>().Property(r => r.RuleId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ProductComboImage>().Property(c => c.ComboId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Location>().Property(l => l.LocationId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<OrderRefund>().Property(r => r.RefundId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ProductIssueReport>().Property(r => r.ReportId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Favorite>().Property(f => f.FavoriteId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ProductVariant>().Property(v => v.VariantId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Accessory>().Property(a => a.AccessoryId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Inventory>().Property(i => i.InventoryId).HasDefaultValueSql("gen_random_uuid()");

            // D. Default values
            modelBuilder.Entity<Product>()
                .Property(p => p.TotalSales)
                .HasDefaultValue(0);

            modelBuilder.Entity<ProductDynamicStats>()
                .Property(s => s.ViewCountIn10Min)
                .HasDefaultValue(0);

            // A. Decimal precision — explicit to avoid silent truncation across locales
            modelBuilder.Entity<Order>().Property(o => o.Subtotal).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(o => o.DiscountTotal).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(o => o.TaxTotal).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(o => o.ShippingTotal).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(o => o.GrandTotal).HasPrecision(18, 4);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 4);
            modelBuilder.Entity<Build>().Property(b => b.CalculatedPrice).HasPrecision(18, 4);
            modelBuilder.Entity<ProductionJob>().Property(j => j.ActualProductionCost).HasPrecision(18, 4);
            modelBuilder.Entity<ProductionJob>().Property(j => j.TotalAssemblyCost).HasPrecision(18, 4);
            modelBuilder.Entity<ProductionJob>().Property(j => j.CraftsmanCommission).HasPrecision(18, 4);
            modelBuilder.Entity<ProductionJob>().Property(j => j.QCCommission).HasPrecision(18, 4);
            modelBuilder.Entity<OrderPromotion>().Property(op => op.DiscountAmount).HasPrecision(18, 4);
            modelBuilder.Entity<Promotion>().Property(p => p.Value).HasPrecision(18, 4);
            modelBuilder.Entity<BuildComponent>().Property(bc => bc.AdditionalPrice).HasPrecision(18, 4);
            modelBuilder.Entity<CartItem>().Property(ci => ci.UnitPriceSnapshot).HasPrecision(18, 4);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasPrecision(18, 4);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.LineTotal).HasPrecision(18, 4);
            modelBuilder.Entity<OrderRefund>().Property(r => r.Amount).HasPrecision(18, 4);
            modelBuilder.Entity<ProductVariant>().Property(v => v.Price).HasPrecision(18, 4);
            modelBuilder.Entity<ProductVariant>().Property(v => v.BaseCost).HasPrecision(18, 4);
            modelBuilder.Entity<ProductVariant>().Property(v => v.AssemblyCost).HasPrecision(18, 4);
            modelBuilder.Entity<Accessory>().Property(a => a.BaseCost).HasPrecision(18, 4);
            modelBuilder.Entity<Accessory>().Property(a => a.AssemblyCost).HasPrecision(18, 4);
            modelBuilder.Entity<Accessory>().Property(a => a.TargetPrice).HasPrecision(18, 4);
            modelBuilder.Entity<User>().Property(u => u.TotalEarned).HasPrecision(18, 4);

            // B. PrintData — jsonb for arbitrary JSON payloads in PostgreSQL
            modelBuilder.Entity<ProductionJob>()
                .Property(j => j.PrintData)
                .HasColumnType("jsonb");

            modelBuilder.Entity<ProductionJob>()
                .Property(j => j.EvidenceImages)
                .HasColumnType("jsonb");

            // Composite / junction keys
            // C. Kept as explicit composite keys (junction tables may need extra columns later)
            modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCollection>().HasKey(pc => new { pc.ProductId, pc.CollectionId });
            modelBuilder.Entity<ProductCharacter>().HasKey(pc => new { pc.ProductId, pc.CharacterId });
            modelBuilder.Entity<ProductAccessory>().HasKey(pa => new { pa.ProductId, pa.AccessoryId });
            modelBuilder.Entity<OrderPromotion>().HasKey(op => new { op.OrderId, op.PromotionId });

            // Unique indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique()
                .HasDatabaseName("IX_users_email");

            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasConversion<string>();

            modelBuilder.Entity<UserToken>()
                .HasIndex(ut => ut.Token).IsUnique()
                .HasDatabaseName("IX_user_tokens_token");

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName).IsUnique()
                .HasDatabaseName("IX_roles_role_name");

            // Performance indexes
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UserId)
                .HasDatabaseName("IX_orders_user_id");

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Status)
                .HasDatabaseName("IX_orders_status");

            modelBuilder.Entity<Build>()
                .HasIndex(b => b.UserId)
                .HasDatabaseName("IX_builds_user_id");

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.UserId)
                .HasDatabaseName("IX_reviews_user_id");

            modelBuilder.Entity<ProductionJob>()
                .HasIndex(j => j.Status)
                .HasDatabaseName("IX_production_jobs_status");

            // Shopee-style filter indexes
            modelBuilder.Entity<Product>().HasIndex(p => p.TotalSales).HasDatabaseName("IX_products_total_sales");
            modelBuilder.Entity<Product>().HasIndex(p => p.Slug).IsUnique().HasDatabaseName("IX_products_slug");
            


            // Product stats indexes (Featured)
            modelBuilder.Entity<ProductDynamicStats>().HasIndex(s => s.ViewCountIn10Min).HasDatabaseName("IX_stats_view_count");

            // Unique rule per base+addon+group
            modelBuilder.Entity<PersonalizationRule>()
                .HasIndex(pr => new { pr.BaseProductId, pr.AddonProductId, pr.GroupId })
                .IsUnique();

            // Unique combo per base product and combo key
            modelBuilder.Entity<ProductComboImage>()
                .HasIndex(c => new { c.BaseProductId, c.CombinationKey })
                .IsUnique()
                .HasDatabaseName("IX_product_combo_images_base_key");

            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.ProductId })
                .IsUnique()
                .HasDatabaseName("IX_favorites_user_product");

            modelBuilder.Entity<ProductVariant>()
                .HasIndex(v => v.Sku)
                .IsUnique()
                .HasDatabaseName("IX_product_variants_sku");

            modelBuilder.Entity<Accessory>()
                .HasIndex(a => a.Sku)
                .IsUnique()
                .HasDatabaseName("IX_accessories_sku");
            
            // Unique index to prevent duplicate items in same cart (Concurrency protection)
            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.CartId, ci.ProductId, ci.BuildId, ci.IncludesSmartChip })
                .IsUnique()
                .HasDatabaseName("IX_cart_items_unique_composite");

            // Self-referencing category tree
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Dynamic Stats (One-to-One)
            modelBuilder.Entity<ProductDynamicStats>()
                .HasOne(s => s.Product)
                .WithOne(p => p.DynamicStats)
                .HasForeignKey<ProductDynamicStats>(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product personalization rules (two separate FKs to Product)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.BasePersonalizationRules)
                .WithOne(pr => pr.BaseProduct)
                .HasForeignKey(pr => pr.BaseProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.AddonPersonalizationRules)
                .WithOne(pr => pr.AddonProduct)
                .HasForeignKey(pr => pr.AddonProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order address FKs — Restrict to avoid multiple cascade paths
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.UpdatedByUser)
                .WithMany()
                .HasForeignKey(o => o.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Address / Build / Cart / Review — cascade or nullify on user delete
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Build>()
                .HasOne(b => b.User)
                .WithMany(u => u.Builds)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ProductionJob — two FKs to User, both Restrict to prevent cascade conflict
            modelBuilder.Entity<ProductionJob>()
                .HasOne(pj => pj.Technician)
                .WithMany()
                .HasForeignKey(pj => pj.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionJob>()
                .HasOne(pj => pj.UpdatedByUser)
                .WithMany()
                .HasForeignKey(pj => pj.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderRefund relationships
            modelBuilder.Entity<OrderRefund>()
                .HasOne(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderRefund>()
                .HasOne(r => r.Requester)
                .WithMany()
                .HasForeignKey(r => r.RequestedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderRefund>()
                .HasOne(r => r.Approver)
                .WithMany()
                .HasForeignKey(r => r.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ProductIssueReport relationships
            modelBuilder.Entity<ProductIssueReport>()
                .HasOne(r => r.OrderItem)
                .WithMany()
                .HasForeignKey(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductIssueReport>()
                .HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductIssueReport>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductIssueReport>()
                .HasOne(r => r.AssignedStaff)
                .WithMany()
                .HasForeignKey(r => r.AssignedStaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // BuildComponent FKs — Restrict on option_variant_id to break the cascade cycle:
            // product_variants → builds (CASCADE) → build_components (CASCADE)
            // product_variants → build_components.option_variant_id must be Restrict (not CASCADE)
            modelBuilder.Entity<BuildComponent>()
                .HasOne(bc => bc.Build)
                .WithMany(b => b.BuildComponents)
                .HasForeignKey(bc => bc.BuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuildComponent>()
                .HasOne(bc => bc.OptionProduct)
                .WithMany(p => p.BuildComponents)
                .HasForeignKey(bc => bc.OptionProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItem FKs — Restrict both variant and build to avoid cycles
            // carts → cart_items (CASCADE); builds → cart_items must be Restrict
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Build)
                .WithMany(b => b.CartItems)
                .HasForeignKey(ci => ci.BuildId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem FKs — Restrict both FK to avoid cycles from orders/builds
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Build)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.BuildId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review — OrderItem FK Restrict (orders → order_items CASCADE; already cascades to reviews via order)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.OrderItem)
                .WithMany(oi => oi.Reviews)
                .HasForeignKey(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReviewReply — staff FK: user delete should not cascade to replies (Restrict)
            modelBuilder.Entity<ReviewReply>()
                .HasOne(rr => rr.StaffUser)
                .WithMany()
                .HasForeignKey(rr => rr.StaffUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- SEED DATA ---

            // 1. Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Software Administrator", Priority = 1, IsSystem = true },
                new Role { RoleId = 2, RoleName = "Staff", Description = "Shop Staff", Priority = 2, IsSystem = true },
                new Role { RoleId = 3, RoleName = "Customer", Description = "Parent / Customer", Priority = 3, IsSystem = true }
            );

            // 2. Users (Passwords: admin123, staff123, user123)
            var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var staffId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000003");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = adminId,
                    Email = "admin@designabear.vn",
                    PasswordHash = "$2a$11$qJ6xaA.cPe0DoGkBFhVXRub5REPJ/60fS1qXra8JYiQcXt4T5hAne",
                    FullName = "Admin DesignABear",
                    Status = UserStatus.Active,
                    RoleId = 1,
                    Provider = "Local",
                    Language = "vi",
                    Timezone = "Asia/Ho_Chi_Minh"
                },
                new User
                {
                    UserId = staffId,
                    Email = "staff@designabear.vn",
                    PasswordHash = "$2a$11$ehYMrDAhwzH07nsnILL3M.2yQgWKRdRxmhDUFlIgdIsmQBMqf.bKq",
                    FullName = "Staff DesignABear",
                    Status = UserStatus.Active,
                    RoleId = 2,
                    Provider = "Local",
                    Language = "vi",
                    Timezone = "Asia/Ho_Chi_Minh"
                },
                new User
                {
                    UserId = userId,
                    Email = "user@designabear.vn",
                    PasswordHash = "$2a$11$lcSr4ykln5Di0r5ZnxugROEl0yDBT6m6rd6UJMhf0fA//IBE5Qr7W",
                    FullName = "Sample User",
                    Status = UserStatus.Active,
                    RoleId = 3,
                    Provider = "Local",
                    Language = "vi",
                    Timezone = "Asia/Ho_Chi_Minh"
                }
            );

            // 3. Sample Catalog Data
            var catId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var charId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var prodId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = catId, Name = "Gấu Nhồi Bông", Slug = "gau-nhoi-bong" }
            );

            modelBuilder.Entity<Character>().HasData(
                new Character { CharacterId = charId, Name = "Gấu Nâu Classical", Slug = "gau-nau-classical", Description = "Gấu bông cổ điển mềm mại", IsActive = true }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = prodId,
                    Name = "Gấu Teddy Đặc Biệt",
                    Slug = "gau-teddy-dac-biet",
                    Description = "Sản phẩm gấu Teddy cao cấp nhất",
                    IsActive = true,
                    IsPersonalizable = true,
                    TotalSales = 0
                }
            );

            var aiProcessorId = Guid.Parse("33333333-3333-3333-3333-333333333334");
            modelBuilder.Entity<Accessory>().HasData(
                new Accessory
                {
                    AccessoryId = aiProcessorId,
                    Name = "AI Processor",
                    Sku = "CORE-ESP32-AI",
                    Description = "Chip xử lý AI cao cấp cho gấu thông minh",
                    BaseCost = 50000,
                    AssemblyCost = 20000,
                    TargetPrice = 150000,
                    IsActive = true
                }
            );

            modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory { ProductId = prodId, CategoryId = catId });
            modelBuilder.Entity<ProductCharacter>().HasData(new ProductCharacter { ProductId = prodId, CharacterId = charId });

            // Seed ProductMedia — multiple images per product (1-to-N)
            modelBuilder.Entity<ProductMedia>().HasData(
                new ProductMedia
                {
                    MediaId = Guid.Parse("aaaa0001-0001-0001-0001-000000000001"),
                    ProductId = prodId,
                    Url = "https://images.unsplash.com/photo-1559715541-5daf8a0296d0?w=600",
                    SortOrder = 1,
                    AltText = "Gấu Teddy Đặc Biệt - Mặt trước"
                },
                new ProductMedia
                {
                    MediaId = Guid.Parse("aaaa0001-0001-0001-0001-000000000002"),
                    ProductId = prodId,
                    Url = "https://images.unsplash.com/photo-1602734846297-9299fc2d4f98?w=600",
                    SortOrder = 2,
                    AltText = "Gấu Teddy Đặc Biệt - Góc nghiêng"
                },
                new ProductMedia
                {
                    MediaId = Guid.Parse("aaaa0001-0001-0001-0001-000000000003"),
                    ProductId = prodId,
                    Url = "https://images.unsplash.com/photo-1562040506-a9b32cb51b94?w=600",
                    SortOrder = 3,
                    AltText = "Gấu Teddy Đặc Biệt - Chi tiết vải"
                }
            );

            // 4. Product Dynamic Stats Seed
            modelBuilder.Entity<ProductDynamicStats>().HasData(
                new ProductDynamicStats
                {
                    ProductId = prodId,
                    ViewCountIn10Min = 0,
                    DiscountRate = 0
                }
            );

            // 5. Inventory & Locations Seed
            var mainWarehouseId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    LocationId = mainWarehouseId,
                    LocationType = "WAREHOUSE",
                    Name = "Kho chính DesignABear",
                    AddressText = "123 Đường Sáng Tạo, TP. Thủ Đức, TP. HCM",
                    Phone = "028-1234-5678"
                }
            );

            modelBuilder.Entity<Inventory>().HasData(
                new Inventory
                {
                    InventoryId = Guid.Parse("cccc0000-0000-0000-0000-000000000001"),
                    LocationId = mainWarehouseId,
                    IdentityId = aiProcessorId,
                    IsAccessory = true,
                    ProductId = null,
                    OnHand = 999,
                    Reserved = 0,
                    UpdatedAt = DateTime.UtcNow
                },
                new Inventory
                {
                    InventoryId = Guid.Parse("cccc0000-0000-0000-0000-000000000002"),
                    LocationId = mainWarehouseId,
                    IdentityId = prodId, // Assuming prodId is used as variantId for seed
                    IsAccessory = false,
                    ProductId = prodId, // Gấu Teddy Đặc Biệt
                    OnHand = 50,
                    Reserved = 0,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
