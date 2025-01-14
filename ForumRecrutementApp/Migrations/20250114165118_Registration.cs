using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumRecrutementApp.Migrations
{
    /// <inheritdoc />
    public partial class Registration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Recruteurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Recruteurs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Administrateurs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recruteurs_IdentityUserId",
                table: "Recruteurs",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administrateurs_IdentityUserId",
                table: "Administrateurs",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrateurs_AspNetUsers_IdentityUserId",
                table: "Administrateurs",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recruteurs_AspNetUsers_IdentityUserId",
                table: "Recruteurs",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrateurs_AspNetUsers_IdentityUserId",
                table: "Administrateurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruteurs_AspNetUsers_IdentityUserId",
                table: "Recruteurs");

            migrationBuilder.DropIndex(
                name: "IX_Recruteurs_IdentityUserId",
                table: "Recruteurs");

            migrationBuilder.DropIndex(
                name: "IX_Administrateurs_IdentityUserId",
                table: "Administrateurs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Recruteurs");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Recruteurs");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Administrateurs");
        }
    }
}
