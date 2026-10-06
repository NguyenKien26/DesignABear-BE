using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RepairBuildSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='size_description') THEN
                        ALTER TABLE builds ADD COLUMN size_description character varying(100);
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='size_tag') THEN
                        ALTER TABLE builds ADD COLUMN size_tag character varying(10);
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='builds' AND column_name='base_variant_id') THEN
                        ALTER TABLE builds ADD COLUMN base_variant_id uuid;
                    ELSE
                        -- Ensure it is nullable to match Guid? in Build model
                        ALTER TABLE builds ALTER COLUMN base_variant_id DROP NOT NULL;
                    END IF;
                END $$;
            ");

            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='build_components' AND column_name='option_variant_id') THEN
                        ALTER TABLE build_components ADD COLUMN option_variant_id uuid;
                    END IF;
                END $$;
            ");

            // Re-create indexes and foreign keys only if they don't exist (using SQL)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_builds_base_variant_id') THEN
                        CREATE INDEX ""IX_builds_base_variant_id"" ON builds (base_variant_id);
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_builds_product_variants_base_variant_id') THEN
                        ALTER TABLE builds ADD CONSTRAINT ""FK_builds_product_variants_base_variant_id"" 
                        FOREIGN KEY (base_variant_id) REFERENCES product_variants (variant_id);
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_build_components_option_variant_id') THEN
                        CREATE INDEX ""IX_build_components_option_variant_id"" ON build_components (option_variant_id);
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_build_components_product_variants_option_variant_id') THEN
                        ALTER TABLE build_components ADD CONSTRAINT ""FK_build_components_product_variants_option_variant_id"" 
                        FOREIGN KEY (option_variant_id) REFERENCES product_variants (variant_id);
                    END IF;

                    -- Repair inventory table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='inventory' AND column_name='product_id') THEN
                        ALTER TABLE inventory ADD COLUMN product_id uuid;
                    END IF;
                    ALTER TABLE inventory ALTER COLUMN variant_id DROP NOT NULL;
                    
                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_inventory_products_product_id') THEN
                        ALTER TABLE inventory ADD CONSTRAINT ""FK_inventory_products_product_id"" 
                        FOREIGN KEY (product_id) REFERENCES products (product_id);
                    END IF;

                    -- Repair cart_items table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='cart_items' AND column_name='product_id') THEN
                        ALTER TABLE cart_items ADD COLUMN product_id uuid;
                    END IF;
                    ALTER TABLE cart_items ALTER COLUMN variant_id DROP NOT NULL;

                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_cart_items_products_product_id') THEN
                        ALTER TABLE cart_items ADD CONSTRAINT ""FK_cart_items_products_product_id"" 
                        FOREIGN KEY (product_id) REFERENCES products (product_id);
                    END IF;

                    -- Re-create the unique composite index for CartItem to include both product_id and build_id
                    DROP INDEX IF EXISTS ""IX_cart_items_unique_composite"";
                    CREATE UNIQUE INDEX ""IX_cart_items_unique_composite"" ON cart_items (cart_id, product_id, build_id, includes_smart_chip);

                    -- Repair order_items table
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='order_items' AND column_name='product_id') THEN
                        ALTER TABLE order_items ADD COLUMN product_id uuid;
                    END IF;
                    ALTER TABLE order_items ALTER COLUMN variant_id DROP NOT NULL;

                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_order_items_products_product_id') THEN
                        ALTER TABLE order_items ADD CONSTRAINT ""FK_order_items_products_product_id"" 
                        FOREIGN KEY (product_id) REFERENCES products (product_id);
                    END IF;
                END $$;
            ");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_build_components_product_variants_option_variant_id",
                table: "build_components");

            migrationBuilder.DropIndex(
                name: "IX_build_components_option_variant_id",
                table: "build_components");

            migrationBuilder.DropColumn(
                name: "option_variant_id",
                table: "build_components");

            migrationBuilder.DropForeignKey(
                name: "FK_builds_product_variants_base_variant_id",
                table: "builds");

            migrationBuilder.DropIndex(
                name: "IX_builds_base_variant_id",
                table: "builds");

            migrationBuilder.DropColumn(
                name: "base_variant_id",
                table: "builds");

            migrationBuilder.DropColumn(
                name: "size_description",
                table: "builds");

            migrationBuilder.DropColumn(
                name: "size_tag",
                table: "builds");

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
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3752), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3753) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3775), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3775) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3781), new DateTime(2026, 4, 21, 14, 7, 56, 345, DateTimeKind.Utc).AddTicks(3782) });
        }
    }
}
