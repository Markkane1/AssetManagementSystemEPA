using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesWithNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Assets_PurchaseOrderId",
                table: "Assets",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_PurchaseOrders_PurchaseOrderId",
                table: "Assets",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_PurchaseOrders_PurchaseOrderId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_PurchaseOrderId",
                table: "Assets");
        }
    }
}
