using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixPostPaymentAndProductionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "serial_number",
                table: "production_jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6580), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6581) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5838), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5845) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5856), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5856) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5861), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5862) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6219), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6221) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6242), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6243) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6252), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6253) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "production_jobs");

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
    }
}
