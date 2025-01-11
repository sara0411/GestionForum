using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumRecrutementApp.Migrations
{
    /// <inheritdoc />
    public partial class testingAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidats_Forums_ForumId",
                table: "Candidats");

            migrationBuilder.AlterColumn<int>(
                name: "ForumId",
                table: "Candidats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidats_Forums_ForumId",
                table: "Candidats",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidats_Forums_ForumId",
                table: "Candidats");

            migrationBuilder.AlterColumn<int>(
                name: "ForumId",
                table: "Candidats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidats_Forums_ForumId",
                table: "Candidats",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
