using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcIdentityApp.Migrations
{
    /// <inheritdoc />
    public partial class addOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order_id",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order_id",
                table: "OrderDetails");
        }
    }
}
