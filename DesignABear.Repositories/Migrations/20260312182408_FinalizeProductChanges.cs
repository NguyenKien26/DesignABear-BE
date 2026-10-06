using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeProductChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(999), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(999) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(757), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(761) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(764), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(765) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(766), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(766) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(925), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(925) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(935), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(935) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(938), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(938) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
