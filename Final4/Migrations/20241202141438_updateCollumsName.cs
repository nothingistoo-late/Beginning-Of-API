using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final4.Migrations
{
    /// <inheritdoc />
    public partial class updateCollumsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_UserId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_AccountId");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "Flowers",
                newName: "FlowerImgUrl");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Accounts",
                newName: "AccountRoleID");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Accounts",
                newName: "AccountPassword");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Accounts",
                newName: "AccountName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Accounts",
                newName: "AccountEmail");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Accounts",
                newName: "AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlowerQuantity",
                table: "Flowers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FlowerPrice",
                table: "Flowers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameColumn(
                name: "FlowerImgUrl",
                table: "Flowers",
                newName: "ImgUrl");

            migrationBuilder.RenameColumn(
                name: "AccountRoleID",
                table: "Accounts",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "AccountPassword",
                table: "Accounts",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "Accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AccountEmail",
                table: "Accounts",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FlowerQuantity",
                table: "Flowers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlowerPrice",
                table: "Flowers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
