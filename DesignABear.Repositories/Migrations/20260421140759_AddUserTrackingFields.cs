using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_earned",
                table: "users",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "total_jobs_completed",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "craftsman_commission",
                table: "production_jobs",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "evidence_images",
                table: "production_jobs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "qc_commission",
                table: "production_jobs",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_assembly_cost",
                table: "production_jobs",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4193), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4194) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4072), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(4072) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3180), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3184) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3190), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3191) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3194), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3195) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "total_earned", "total_jobs_completed", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3752), 0m, 0, new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3753) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "total_earned", "total_jobs_completed", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3775), 0m, 0, new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3775) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "total_earned", "total_jobs_completed", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3781), 0m, 0, new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3782) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_earned",
                table: "users");

            migrationBuilder.DropColumn(
                name: "total_jobs_completed",
                table: "users");

            migrationBuilder.DropColumn(
                name: "craftsman_commission",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "evidence_images",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "qc_commission",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "total_assembly_cost",
                table: "production_jobs");

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
        }
    }
}
