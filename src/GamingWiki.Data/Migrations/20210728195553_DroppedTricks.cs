using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingWiki.Data.Migrations
{
    public partial class DroppedTricks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tricks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tricks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameId1 = table.Column<int>(type: "int", nullable: true),
                    Heading = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tricks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tricks_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tricks_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tricks_Games_GameId1",
                        column: x => x.GameId1,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tricks_AuthorId",
                table: "Tricks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tricks_GameId",
                table: "Tricks",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Tricks_GameId1",
                table: "Tricks",
                column: "GameId1");
        }
    }
}
