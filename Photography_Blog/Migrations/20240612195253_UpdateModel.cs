using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Photography_Blog.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "MainPageTexts",
                newName: "VideoUrl2");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "MainPageTexts",
                newName: "VideoUrl1");

            migrationBuilder.AddColumn<string>(
                name: "ImageName1",
                table: "MainPageTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName2",
                table: "MainPageTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactInfoViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiktok = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfoViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainPageTextViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPageTextViewModel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInfoViewModel");

            migrationBuilder.DropTable(
                name: "MainPageTextViewModel");

            migrationBuilder.DropColumn(
                name: "ImageName1",
                table: "MainPageTexts");

            migrationBuilder.DropColumn(
                name: "ImageName2",
                table: "MainPageTexts");

            migrationBuilder.RenameColumn(
                name: "VideoUrl2",
                table: "MainPageTexts",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "VideoUrl1",
                table: "MainPageTexts",
                newName: "ImageName");
        }
    }
}
