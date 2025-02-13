using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicePhoto.DataEntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldAccountIdInProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "PersonalPhotos",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "PersonalPhotos",
                newName: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "PersonalPhotos",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "PersonalPhotos",
                newName: "Path");
        }
    }
}
