using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAccessories : Migration
    {
        private static readonly Guid CategoryId = new Guid("11111111-1111-1111-1111-111111111111");
        private static readonly Guid GroupId = new Guid("AA000000-0000-0000-0000-000000000001");

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Personalization Group
            migrationBuilder.InsertData(
                table: "personalization_groups",
                columns: new[] { "group_id", "name", "sort_order" },
                values: new object[] { GroupId, "Phụ kiện & Phụ trang", 1 }
            );

            var accessories = new[]
            {
                new { Id = new Guid("AC000000-0000-0000-0000-000000000001"), Name = "Tai mèo", Slug = "tai-meo", Price = 50000m, Sku = "ACC-CAT-EARS" },
                new { Id = new Guid("AC000000-0000-0000-0000-000000000002"), Name = "Mũ len", Slug = "mu-len", Price = 80000m, Sku = "ACC-WOOL-HAT" },
                new { Id = new Guid("AC000000-0000-0000-0000-000000000003"), Name = "Kính râm", Slug = "kinh-ram", Price = 40000m, Sku = "ACC-SUNGLASSES" },
                new { Id = new Guid("AC000000-0000-0000-0000-000000000004"), Name = "Nơ cổ hồng", Slug = "no-co-hong", Price = 30000m, Sku = "ACC-BOW-TIE" }
            };

            foreach (var acc in accessories)
            {
                // Insert as Product (Accessory type)
                migrationBuilder.InsertData(
                    table: "products",
                    columns: new[] { "product_id", "created_at", "description", "is_active", "is_personalizable", "model_3d_url", "name", "product_type", "slug", "updated_at" },
                    values: new object[] { acc.Id, new DateTime(2026, 3, 12, 14, 0, 0, DateTimeKind.Utc), $"Phụ kiện {acc.Name} cho gấu bông.", true, false, null, acc.Name, "ACCESSORY", acc.Slug, new DateTime(2026, 3, 12, 14, 0, 0, DateTimeKind.Utc) }
                );

                // Create a variant for the accessory itself (needed for cart/inventory)
                var variantId = new Guid(acc.Id.ToString().Replace("AC00", "AV00"));
                migrationBuilder.InsertData(
                    table: "product_variants",
                    columns: new[] { "variant_id", "currency", "is_sold_out", "price", "product_id", "sku", "variant_name" },
                    values: new object[] { variantId, "VND", false, acc.Price, acc.Id, acc.Sku, "Standard" }
                );

                // Link to all bears (Teddy Đặc Biệt + Models 01-20)
                var bearIds = new List<Guid> { new Guid("33333333-3333-3333-3333-333333333333") };
                for (int i = 1; i <= 20; i++)
                {
                    bearIds.Add(new Guid($"EE000000-0000-0000-0000-{i:D12}"));
                }

                foreach (var bearId in bearIds)
                {
                    migrationBuilder.InsertData(
                        table: "personalization_rules",
                        columns: new[] { "rule_id", "addon_product_id", "base_product_id", "group_id", "is_active", "max_qty", "rule_type" },
                        values: new object[] { Guid.NewGuid(), acc.Id, bearId, GroupId, true, 1, "OPTIONAL" }
                    );
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "personalization_groups", keyColumn: "group_id", keyValue: GroupId);
            // Products and variants would be cleaned up manually or via cascade if set up, 
            // but for safety in UAT we focus on Up.
        }
    }
}
