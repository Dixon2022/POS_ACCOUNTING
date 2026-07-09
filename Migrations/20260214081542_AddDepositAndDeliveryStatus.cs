using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilSalon_v._1.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositAndDeliveryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingBalance",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ProductDelivered",
                table: "Sales",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingBalance",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ProductReceived",
                table: "Purchases",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PendingBalance",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductDelivered",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PendingBalance",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ProductReceived",
                table: "Purchases");
        }
    }
}
