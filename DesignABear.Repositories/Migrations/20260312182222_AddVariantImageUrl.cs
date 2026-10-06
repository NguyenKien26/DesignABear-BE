using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "product_variants",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2375), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2375) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2130), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2133) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2136), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2136) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2138), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2138) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2294), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2294) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2304), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2304) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2307), new DateTime(2026, 3, 12, 18, 22, 21, 826, DateTimeKind.Utc).AddTicks(2307) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "product_variants");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9995), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9995) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9764), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9766) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9769), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9770), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9771) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9888), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9888) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9898), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9899) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9901), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9902) });
        }
    }
}
