using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicePhoto.DataEntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnPathToFilePathInTablePetPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "PetPhotos",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "PetPhotos",
                newName: "Path");
        }
    }
}
