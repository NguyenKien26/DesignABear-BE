using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseVariantIdAndOptionVariantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "assembly_cost",
                table: "products");

            migrationBuilder.DropColumn(
                name: "base_cost",
                table: "products");

            migrationBuilder.DropColumn(
                name: "weight_gram",
                table: "products");

            migrationBuilder.AddColumn<Guid>(
                name: "base_variant_id",
                table: "builds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "option_variant_id",
                table: "build_components",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1371));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1253), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1253) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1260), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1263), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1263) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1221), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1221) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1229), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1229) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(963), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(967) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(970), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(970) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(973), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(973) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(976), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(976) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(977), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(977) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1106), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1106) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1154), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1155) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1157), new DateTime(2026, 4, 20, 15, 27, 41, 717, DateTimeKind.Utc).AddTicks(1157) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base_variant_id",
                table: "builds");

            migrationBuilder.DropColumn(
                name: "option_variant_id",
                table: "build_components");

            migrationBuilder.AddColumn<decimal>(
                name: "assembly_cost",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "base_cost",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "weight_gram",
                table: "products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5565));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5279), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5280) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5295), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5296) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5302), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5303) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "assembly_cost", "base_cost", "created_at", "updated_at", "weight_gram" },
                values: new object[] { 0m, 0m, new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4916), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4917), 500f });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "assembly_cost", "base_cost", "created_at", "updated_at", "weight_gram" },
                values: new object[] { 0m, 0m, new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4934), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4934), 10f });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4252), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4256) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4262), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4263) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4267), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4267) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4270), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4271) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4274), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4275) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4619), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4620) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4638), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4638) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4645), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4645) });
        }
    }
}
