using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "network_sites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_network_sites", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("users_role_check", "role IN ('Dispatcher', 'Electrician')");
                });

            migrationBuilder.CreateTable(
                name: "cable_tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    assignee_user_id = table.Column<int>(type: "integer", nullable: true),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cable_tickets", x => x.id);
                    table.CheckConstraint("cable_tickets_description_check", "char_length(description) BETWEEN 10 AND 500");
                    table.CheckConstraint("cable_tickets_status_check", "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");
                    table.CheckConstraint("cable_tickets_title_check", "char_length(title) BETWEEN 5 AND 80");
                    table.ForeignKey(
                        name: "FK_cable_tickets_network_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "network_sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cable_tickets_users_assignee_user_id",
                        column: x => x.assignee_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cable_tickets_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "cable_tickets_site_id_idx",
                table: "cable_tickets",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "cable_tickets_status_idx",
                table: "cable_tickets",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_cable_tickets_assignee_user_id",
                table: "cable_tickets",
                column: "assignee_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cable_tickets_created_by_user_id",
                table: "cable_tickets",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cable_tickets_number",
                table: "cable_tickets",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_network_sites_name",
                table: "network_sites",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cable_tickets");

            migrationBuilder.DropTable(
                name: "network_sites");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
