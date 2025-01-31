using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ForumRecrutementApp.Migrations
{
    /// <inheritdoc />
    public partial class testing1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recruteurs_IdentityUserId",
                table: "Recruteurs");

            migrationBuilder.DropIndex(
                name: "IX_Administrateurs_IdentityUserId",
                table: "Administrateurs");

            migrationBuilder.DropColumn(
                name: "CVContentType",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "CVData",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "CVFileName",
                table: "Candidats");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEvaluation",
                table: "Evaluations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Forums",
                columns: new[] { "Id", "Date", "Nom" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forum 1" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forum 2" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forum 3" }
                });

            migrationBuilder.InsertData(
                table: "Candidats",
                columns: new[] { "Id", "CV", "Competences", "Email", "ForumId", "Nom", "Prenom" },
                values: new object[,]
                {
                    { 1, "john_doe_cv.pdf", "C#, .NET, SQL", "john.doe@example.com", 1, "Doe", "John" },
                    { 2, "jane_smith_cv.pdf", "Java, Spring Boot, Hibernate", "jane.smith@example.com", 2, "Smith", "Jane" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recruteurs_IdentityUserId",
                table: "Recruteurs",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrateurs_IdentityUserId",
                table: "Administrateurs",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recruteurs_IdentityUserId",
                table: "Recruteurs");

            migrationBuilder.DropIndex(
                name: "IX_Administrateurs_IdentityUserId",
                table: "Administrateurs");

            migrationBuilder.DeleteData(
                table: "Candidats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Candidats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Forums",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Forums",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Forums",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "DateEvaluation",
                table: "Evaluations");

            migrationBuilder.AddColumn<string>(
                name: "CVContentType",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "CVData",
                table: "Candidats",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "CVFileName",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
