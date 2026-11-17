using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Covoiturage.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrixTotalToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrixTotal",
                table: "Reservations",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrixTotal",
                table: "Reservations");
        }
    }
}
