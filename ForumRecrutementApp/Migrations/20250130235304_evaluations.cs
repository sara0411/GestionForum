using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumRecrutementApp.Migrations
{
    /// <inheritdoc />
    public partial class evaluations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Commentaire",
                table: "Evaluations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CandidatId1",
                table: "Evaluations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruteurId1",
                table: "Evaluations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_CandidatId1",
                table: "Evaluations",
                column: "CandidatId1");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_RecruteurId1",
                table: "Evaluations",
                column: "RecruteurId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Candidats_CandidatId1",
                table: "Evaluations",
                column: "CandidatId1",
                principalTable: "Candidats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Recruteurs_RecruteurId1",
                table: "Evaluations",
                column: "RecruteurId1",
                principalTable: "Recruteurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Candidats_CandidatId1",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Recruteurs_RecruteurId1",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_CandidatId1",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_RecruteurId1",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "CandidatId1",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "RecruteurId1",
                table: "Evaluations");

            migrationBuilder.AlterColumn<string>(
                name: "Commentaire",
                table: "Evaluations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
