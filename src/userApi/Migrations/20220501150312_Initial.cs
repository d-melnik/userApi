using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace userApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaimEntity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimEntity", x => new { x.UserId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_UserClaimEntity_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClaimEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "For Admin", "Admin Role" },
                    { 2, "For Editor", "Editor Role" },
                    { 3, "For User", "User Role" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreateDate", "Email", "FirstName", "LastName", "Password", "PasswordHash", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 5, 1, 18, 3, 12, 191, DateTimeKind.Local).AddTicks(3582), "test@test.com", "testFirst", "testLast", null, "$2a$11$0ys3d2/35/oiVnmZEoX4eeJgSUUNOcdSumko46KqGMZfxAgtIMtlO", new DateTime(2022, 5, 1, 18, 3, 12, 191, DateTimeKind.Local).AddTicks(3591) },
                    { 2, new DateTime(2022, 5, 1, 18, 3, 12, 320, DateTimeKind.Local).AddTicks(5710), "editor@test.com", "testFirst", "testLast", null, "$2a$11$BsDWuPOEEMva.zIUL2u4Ve6XeiQ3aS4DlXS2TLfeJ.2zmUiSq5.qm", new DateTime(2022, 5, 1, 18, 3, 12, 320, DateTimeKind.Local).AddTicks(5725) },
                    { 3, new DateTime(2022, 5, 1, 18, 3, 12, 449, DateTimeKind.Local).AddTicks(9000), "admin@test.com", "testFirst", "testLast", null, "$2a$11$g/ArCySEB0zcT/qWnKed4uq8AMirl/bfBHUny46d8ic28TTMK5.6e", new DateTime(2022, 5, 1, 18, 3, 12, 449, DateTimeKind.Local).AddTicks(9018) }
                });

            migrationBuilder.InsertData(
                table: "UserClaimEntity",
                columns: new[] { "ClaimId", "UserId", "Id" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "UserClaimEntity",
                columns: new[] { "ClaimId", "UserId", "Id" },
                values: new object[] { 2, 2, 2 });

            migrationBuilder.InsertData(
                table: "UserClaimEntity",
                columns: new[] { "ClaimId", "UserId", "Id" },
                values: new object[] { 3, 3, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimEntity_ClaimId",
                table: "UserClaimEntity",
                column: "ClaimId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserClaimEntity");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
