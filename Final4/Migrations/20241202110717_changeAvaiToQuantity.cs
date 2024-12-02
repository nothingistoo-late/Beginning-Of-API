using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final4.Migrations
{
    /// <inheritdoc />
    public partial class changeAvaiToQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Flowers");

            migrationBuilder.AddColumn<int>(
                name: "FlowerQuantity",
                table: "Flowers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlowerQuantity",
                table: "Flowers");

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Flowers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
