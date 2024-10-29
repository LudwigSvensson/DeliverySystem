using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliverySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class responsibleEmployeeconnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployeeId",
                table: "Drivers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ResponsibleEmployeeId",
                table: "Drivers",
                column: "ResponsibleEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers",
                column: "ResponsibleEmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ResponsibleEmployeeId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeId",
                table: "Drivers");
        }
    }
}
