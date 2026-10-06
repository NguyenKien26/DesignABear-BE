using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges_BuildVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Các cột này đã tồn tại trong DB thực tế nhưng bị thiếu trong Snapshot.
            // Chúng ta vô hiệu hóa lệnh Add để tránh lỗi "column already exists", 
            // nhưng vẫn giữ Migration này để EF cập nhật Snapshot cho các lần sau.
            /*
            migrationBuilder.AddColumn<Guid>(
                name: "base_variant_id",
                table: "builds",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "option_variant_id",
                table: "build_components",
                type: "uuid",
                nullable: true);
            */

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7373), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7374) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7549));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7551));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7344), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7344) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7160), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7162) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7165), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7165) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7166), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7166) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7274), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7275) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7283), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7284) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7286), new DateTime(2026, 4, 21, 9, 40, 59, 423, DateTimeKind.Utc).AddTicks(7286) });

            /*
            migrationBuilder.CreateIndex(
                name: "IX_builds_base_variant_id",
                table: "builds",
                column: "base_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_build_components_option_variant_id",
                table: "build_components",
                column: "option_variant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_build_components_product_variants_option_variant_id",
                table: "build_components",
                column: "option_variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_builds_product_variants_base_variant_id",
                table: "builds",
                column: "base_variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_build_components_product_variants_option_variant_id",
                table: "build_components");

            migrationBuilder.DropForeignKey(
                name: "FK_builds_product_variants_base_variant_id",
                table: "builds");

            migrationBuilder.DropIndex(
                name: "IX_builds_base_variant_id",
                table: "builds");

            migrationBuilder.DropIndex(
                name: "IX_build_components_option_variant_id",
                table: "build_components");

            migrationBuilder.DropColumn(
                name: "base_variant_id",
                table: "builds");

            migrationBuilder.DropColumn(
                name: "option_variant_id",
                table: "build_components");

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(768), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(769) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(983));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(986));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(729), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(730) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(310), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(314) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(318), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(320), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(320) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(554), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(555) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(566), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(567) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(570), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(570) });
        }
    }
}
