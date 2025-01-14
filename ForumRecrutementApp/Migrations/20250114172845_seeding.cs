using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ForumRecrutementApp.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "62a74dbb-3528-4f1e-b203-c220d250c8cf", "john.doe@example.com", true, false, null, "JOHN.DOE@EXAMPLE.COM", "JOHN.DOE@EXAMPLE.COM", "AQAAAAIAAYagAAAAEI/1Q6/6jswimtBfO+6uOe63OVbYeto3TigzegL975V7a4w6yXUJhq+oQrK3WNe6Zw==", null, false, "8e903376-aff3-4527-9c23-2b430d76bd75", false, "john.doe@example.com" },
                    { "2", 0, "a7d4d0d2-6cdd-4ded-bedd-8022007bc1e6", "jane.smith@example.com", true, false, null, "JANE.SMITH@EXAMPLE.COM", "JANE.SMITH@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGiw64pnrpaNVpqFcs24Wh/ZL0GTGFWp6kvPjEqA6HOIvJgvuMEFngoQj6htKgMQxw==", null, false, "62bef534-130c-45bb-93a5-053f1985a91f", false, "jane.smith@example.com" },
                    { "3", 0, "45922254-71c7-403d-b34d-b92a931a211b", "recruiter1@company.com", true, false, null, "RECRUITER1@COMPANY.COM", "RECRUITER1@COMPANY.COM", "AQAAAAIAAYagAAAAEDlSdO2slUEh5pTSOLFBcro/ZgoFzqqOIAecFGVA/53UQnmIbwfhO7uD+r+f3RolTg==", null, false, "904de362-05c2-4643-bd58-e09fa2aaf4a2", false, "recruiter1@company.com" },
                    { "4", 0, "030eec86-7145-40f2-bcbb-c03d098e3146", "admin@forum.com", true, false, null, "ADMIN@FORUM.COM", "ADMIN@FORUM.COM", "AQAAAAIAAYagAAAAEHtXRQgjmKcePGbku1IND1rbYS7JjeKndZW6/LbnE5DVx3PoaPjNc/8FbUnKH3l37g==", null, false, "f072d811-9739-41eb-bc74-23d8b473e2a2", false, "admin@forum.com" }
                });
        }
    }
}
