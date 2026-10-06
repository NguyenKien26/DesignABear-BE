using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CleanupDeviceSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_subscriptions");

            migrationBuilder.DropTable(
                name: "bear_devices");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7741), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7742) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7490), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7492) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7496), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7496) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7498), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7499) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7639), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7639) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7650), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7651) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7654), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7654) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bear_devices",
                columns: table => new
                {
                    device_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    color = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    firmware_version = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    mac_address = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: true),
                    manufactured_date = table.Column<DateOnly>(type: "date", nullable: true),
                    model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    serial_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    warranty_expiry_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bear_devices", x => x.device_id);
                });

            migrationBuilder.CreateTable(
                name: "user_subscriptions",
                columns: table => new
                {
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_renew = table.Column<bool>(type: "boolean", nullable: false),
                    cancellation_reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_renewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    plan_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 4, nullable: false),
                    promo_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_subscriptions", x => x.subscription_id);
                    table.ForeignKey(
                        name: "FK_user_subscriptions_bear_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "bear_devices",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_bear_devices_serial_number",
                table: "bear_devices",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_device_id",
                table: "user_subscriptions",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_end_date",
                table: "user_subscriptions",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_user_status",
                table: "user_subscriptions",
                columns: new[] { "user_id", "status" });
        }
    }
}
