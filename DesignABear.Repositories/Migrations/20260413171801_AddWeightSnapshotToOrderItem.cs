using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightSnapshotToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "weight_snapshot",
                table: "order_items",
                type: "real",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7580), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7581) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7302), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7306) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7309), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7310) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7312), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7312) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7487), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7487) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7501), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7501) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7504), new DateTime(2026, 4, 13, 17, 17, 59, 487, DateTimeKind.Utc).AddTicks(7504) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight_snapshot",
                table: "order_items");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7464), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7464) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7262), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7265) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7268), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7269) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7271), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7271) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7391), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7392) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7403), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7403) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7405), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7405) });
        }
    }
}
