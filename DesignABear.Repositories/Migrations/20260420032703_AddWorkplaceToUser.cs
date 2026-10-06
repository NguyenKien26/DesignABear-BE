using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkplaceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cart_items_cart_id",
                table: "cart_items");

            migrationBuilder.AddColumn<Guid>(
                name: "workplace_id",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "size_tag",
                table: "products",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "esp_finished_at",
                table: "production_jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "esp_handover_note",
                table: "production_jobs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "esp_handover_photo_urls",
                table: "production_jobs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "esp_status",
                table: "production_jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "esp_technician_id",
                table: "production_jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "qc_finished_at",
                table: "production_jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "qc_status",
                table: "production_jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "qc_technician_id",
                table: "production_jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "shell_finished_at",
                table: "production_jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shell_handover_note",
                table: "production_jobs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shell_handover_photo_urls",
                table: "production_jobs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shell_status",
                table: "production_jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "shell_technician_id",
                table: "production_jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(842));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(839));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "size", "size_tag", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9875), null, null, new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9876) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "size", "size_tag", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(172), null, null, new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(175) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9095), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9100) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9107), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9107) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9110), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9111) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at", "workplace_id" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9575), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9576), null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at", "workplace_id" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9595), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9595), null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at", "workplace_id" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9599), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9600), null });

            migrationBuilder.CreateIndex(
                name: "IX_users_workplace_id",
                table: "users",
                column: "workplace_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_unique_composite",
                table: "cart_items",
                columns: new[] { "cart_id", "product_id", "build_id", "includes_smart_chip" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_locations_workplace_id",
                table: "users",
                column: "workplace_id",
                principalTable: "locations",
                principalColumn: "location_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_locations_workplace_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_workplace_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_cart_items_unique_composite",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "workplace_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "size",
                table: "products");

            migrationBuilder.DropColumn(
                name: "size_tag",
                table: "products");

            migrationBuilder.DropColumn(
                name: "esp_finished_at",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "esp_handover_note",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "esp_handover_photo_urls",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "esp_status",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "esp_technician_id",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "qc_finished_at",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "qc_status",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "qc_technician_id",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "shell_finished_at",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "shell_handover_note",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "shell_handover_photo_urls",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "shell_status",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "shell_technician_id",
                table: "production_jobs");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5892), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5893) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5944), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5945) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5207), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5213) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5221), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5221) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5225), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5226) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5709), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5710) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5723), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5724) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5727), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5727) });

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_cart_id",
                table: "cart_items",
                column: "cart_id");
        }
    }
}
