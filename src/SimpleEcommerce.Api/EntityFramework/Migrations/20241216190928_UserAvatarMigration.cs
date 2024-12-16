using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleEcommerce.Api.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UserAvatarMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AvatarId",
                table: "User",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Picture_AvatarId",
                table: "User",
                column: "AvatarId",
                principalTable: "Picture",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Picture_AvatarId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AvatarId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "User");
        }
    }
}
