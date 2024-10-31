using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliverySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResponsibleEmployeeOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers",
                column: "ResponsibleEmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_ResponsibleEmployeeId",
                table: "Drivers",
                column: "ResponsibleEmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
