using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleEcommerce.Api.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class PictureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MimeType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AltAttribute = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    S3Key = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");
        }
    }
}
