using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RenameViewCountToViewCountIn10Min : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "product_dynamic_stats",
                newName: "view_count_in_10min");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4576), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4576) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4290), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4295) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4298), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4298) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4300), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4300) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4447), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4447) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4460), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4460) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4496), new DateTime(2026, 3, 15, 4, 9, 20, 947, DateTimeKind.Utc).AddTicks(4496) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "view_count_in_10min",
                table: "product_dynamic_stats",
                newName: "view_count");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7611), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7611) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7430), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7432) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7437), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7439), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7540), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7550), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7550) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7553), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7553) });
        }
    }
}
