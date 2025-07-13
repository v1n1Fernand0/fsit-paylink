using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSIT.PayLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_AddCpfAndCustomerMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "payer_cpf",
                table: "payments",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "customer_external_map",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    gateway = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    external_customer_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_external_map", x => new { x.tenant_id, x.gateway });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_external_map");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "payer_cpf",
                table: "payments");
        }
    }
}
