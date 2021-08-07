using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingWiki.Data.Migrations
{
    public partial class GamesCharactersApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContributorId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "IsApproved",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string>(
               name: "ContributorId",
               table: "Characters",
               type: "nvarchar(450)",
               nullable: false,
               defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "IsApproved",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AspNetUsers_ContributorId",
                table: "Characters",
                column: "ContributorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_ContributorId",
                table: "Games",
                column: "ContributorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AspNetUsers_ContributorId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_ContributorId",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "ContributorId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ContributorId",
                table: "Characters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

        }
    }
}
