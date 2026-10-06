using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedingExpansion : Migration
    {
        private static readonly Guid CategoryId = new Guid("11111111-1111-1111-1111-111111111111");

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 20; i++)
            {
                var productId = new Guid($"EE000000-0000-0000-0000-{i:D12}");
                var basicVariantId = new Guid($"EE000000-0000-0000-B000-{i:D12}");
                var proVariantId = new Guid($"EE000000-0000-0000-C000-{i:D12}");
                
                var productName = $"Bear Model {i:D2}";
                var productSlug = $"bear-model-{i:D2}";

                // Insert Product
                migrationBuilder.InsertData(
                    table: "products",
                    columns: new[] { "product_id", "created_at", "description", "is_active", "is_personalizable", "model_3d_url", "name", "product_type", "slug", "updated_at" },
                    values: new object[] { productId, new DateTime(2026, 3, 12, 14, 0, 0, DateTimeKind.Utc), $"Premium bear model {i:D2} with customizable processor options.", true, true, null, productName, "BASE_BEAR", productSlug, new DateTime(2026, 3, 12, 14, 0, 0, DateTimeKind.Utc) }
                );

                // Associate with Category
                migrationBuilder.InsertData(
                    table: "product_categories",
                    columns: new[] { "category_id", "product_id" },
                    values: new object[] { CategoryId, productId }
                );

                // Insert Basic Variant
                migrationBuilder.InsertData(
                    table: "product_variants",
                    columns: new[] { "variant_id", "currency", "is_sold_out", "price", "product_id", "sku", "variant_name" },
                    values: new object[] { basicVariantId, "VND", false, 350000m, productId, $"BEAR-{i:D2}-BASIC", "Basic Budget (Shell + Basic Processor)" }
                );

                // Insert Pro Variant
                migrationBuilder.InsertData(
                    table: "product_variants",
                    columns: new[] { "variant_id", "currency", "is_sold_out", "price", "product_id", "sku", "variant_name" },
                    values: new object[] { proVariantId, "VND", false, 750000m, productId, $"BEAR-{i:D2}-PRO", "Pro Budget (Shell + Pro Processor)" }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 20; i >= 1; i--)
            {
                var productId = new Guid($"EE000000-0000-0000-0000-{i:D12}");
                
                migrationBuilder.DeleteData(table: "product_categories", keyColumns: new[] { "category_id", "product_id" }, keyValues: new object[] { CategoryId, productId });
                migrationBuilder.DeleteData(table: "product_variants", keyColumn: "product_id", keyValue: productId);
                migrationBuilder.DeleteData(table: "products", keyColumn: "product_id", keyValue: productId);
            }
        }
    }
}
