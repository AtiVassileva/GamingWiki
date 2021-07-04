using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingWiki.Web.Data.Migrations
{
    public partial class ImprovedGameAndCharacterEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommenterId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommenterId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Characters");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
