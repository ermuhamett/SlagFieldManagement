using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlagFieldManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buckets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buckets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_materials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SlagFieldPlaces",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    row = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    is_enable = table.Column<bool>(type: "boolean", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_slag_field_places", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialSettings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stage_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    event_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    visual_state_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    min_hours = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    max_hours = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_material_settings_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "Materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SlagFieldPlaceEventStore",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    event_data = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_slag_field_place_event_store", x => x.event_id);
                    table.ForeignKey(
                        name: "fk_slag_field_place_event_store_slag_field_places_aggregate_id",
                        column: x => x.aggregate_id,
                        principalTable: "SlagFieldPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlagFieldStates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    place_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bucket_id = table.Column<Guid>(type: "uuid", nullable: true),
                    material_id = table.Column<Guid>(type: "uuid", nullable: true),
                    slag_weight = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_slag_field_states", x => x.id);
                    table.ForeignKey(
                        name: "fk_slag_field_states_buckets_bucket_id",
                        column: x => x.bucket_id,
                        principalTable: "Buckets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_slag_field_states_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "Materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_slag_field_states_slag_field_places_place_id",
                        column: x => x.place_id,
                        principalTable: "SlagFieldPlaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SlagFieldStateEventStore",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    event_data = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_slag_field_state_event_store", x => x.event_id);
                    table.ForeignKey(
                        name: "fk_slag_field_state_event_store_slag_field_states_aggregate_id",
                        column: x => x.aggregate_id,
                        principalTable: "SlagFieldStates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlagFieldStocks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    slag_field_state_id = table.Column<Guid>(type: "uuid", nullable: true),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    application_id = table.Column<Guid>(type: "uuid", nullable: true),
                    transaction_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_slag_field_stocks", x => x.id);
                    table.ForeignKey(
                        name: "fk_slag_field_stocks_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "Materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_slag_field_stocks_slag_field_states_slag_field_state_id",
                        column: x => x.slag_field_state_id,
                        principalTable: "SlagFieldStates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_material_settings_material_id",
                table: "MaterialSettings",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                table: "Roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_place_event_store_aggregate_id",
                table: "SlagFieldPlaceEventStore",
                column: "aggregate_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_place_event_store_timestamp",
                table: "SlagFieldPlaceEventStore",
                column: "timestamp");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_state_event_store_aggregate_id",
                table: "SlagFieldStateEventStore",
                column: "aggregate_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_state_event_store_timestamp",
                table: "SlagFieldStateEventStore",
                column: "timestamp");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_states_bucket_id",
                table: "SlagFieldStates",
                column: "bucket_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_states_material_id",
                table: "SlagFieldStates",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_states_place_id",
                table: "SlagFieldStates",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_stocks_material_id",
                table: "SlagFieldStocks",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_slag_field_stocks_slag_field_state_id",
                table: "SlagFieldStocks",
                column: "slag_field_state_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                table: "Users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialSettings");

            migrationBuilder.DropTable(
                name: "SlagFieldPlaceEventStore");

            migrationBuilder.DropTable(
                name: "SlagFieldStateEventStore");

            migrationBuilder.DropTable(
                name: "SlagFieldStocks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SlagFieldStates");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Buckets");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "SlagFieldPlaces");
        }
    }
}
