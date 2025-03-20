using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicePhoto.DataEntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenameFiledAccoutIdToProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "PetPhotos",
                newName: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "PetPhotos",
                newName: "AccountId");
        }
    }
}
