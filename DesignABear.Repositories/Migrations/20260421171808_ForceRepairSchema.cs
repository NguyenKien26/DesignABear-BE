using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ForceRepairSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    -- Repair builds table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='size_description') THEN
                        ALTER TABLE builds ADD COLUMN size_description character varying(100);
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='size_tag') THEN
                        ALTER TABLE builds ADD COLUMN size_tag character varying(10);
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='base_variant_id') THEN
                        ALTER TABLE builds ADD COLUMN base_variant_id uuid;
                    END IF;

                    -- Repair cart_items table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='cart_items' AND column_name='product_id') THEN
                        ALTER TABLE cart_items ADD COLUMN product_id uuid;
                    END IF;
                    
                    -- Repair inventory table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='inventory' AND column_name='product_id') THEN
                        ALTER TABLE inventory ADD COLUMN product_id uuid;
                    END IF;

                    -- Repair order_items table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='order_items' AND column_name='product_id') THEN
                        ALTER TABLE order_items ADD COLUMN product_id uuid;
                    END IF;

                    -- Ensure foreign keys exist
                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_cart_items_products_product_id') THEN
                        ALTER TABLE cart_items ADD CONSTRAINT ""FK_cart_items_products_product_id"" 
                        FOREIGN KEY (product_id) REFERENCES products (product_id);
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4991), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4991) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(5147));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4952), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4953) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4627), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4629) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4633), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4633) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4635), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4635) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4820), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4821) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4832), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4832) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4835), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4836) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(619), new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(620) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(1178));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(490), new DateTime(2026, 4, 21, 16, 24, 31, 375, DateTimeKind.Utc).AddTicks(491) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9325), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9331) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9340), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9341) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9344), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9345) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9961), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9962) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9986), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9987) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9993), new DateTime(2026, 4, 21, 16, 24, 31, 374, DateTimeKind.Utc).AddTicks(9994) });
        }
    }
}
