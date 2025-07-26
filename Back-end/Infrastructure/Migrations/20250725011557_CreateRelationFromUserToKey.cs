using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationFromUserToKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Key_CreatedBy",
                table: "Key",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Key_User_CreatedBy",
                table: "Key",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Key_User_CreatedBy",
                table: "Key");

            migrationBuilder.DropIndex(
                name: "IX_Key_CreatedBy",
                table: "Key");
        }
    }
}
