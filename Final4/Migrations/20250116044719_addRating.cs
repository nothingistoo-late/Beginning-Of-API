using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final4.Migrations
{
    /// <inheritdoc />
    public partial class addRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailOrderId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailFlowerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Ratings_OrderDetails_OrderDetailOrderId_OrderDetailFlowerId",
                        columns: x => new { x.OrderDetailOrderId, x.OrderDetailFlowerId },
                        principalTable: "OrderDetails",
                        principalColumns: new[] { "OrderId", "FlowerId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OrderDetailOrderId_OrderDetailFlowerId",
                table: "Ratings",
                columns: new[] { "OrderDetailOrderId", "OrderDetailFlowerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "OrderDetails");
        }
    }
}
