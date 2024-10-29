using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliverySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsibleEmployee",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployeeid",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ResponsibleEmployeeid",
                table: "Events",
                column: "ResponsibleEmployeeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_ResponsibleEmployeeid",
                table: "Events",
                column: "ResponsibleEmployeeid",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_ResponsibleEmployeeid",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ResponsibleEmployeeid",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeid",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleEmployee",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
