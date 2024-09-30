using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    /// <inheritdoc />
    public partial class CreatePetImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Pets_PetId",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "PetImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_PetId",
                table: "PetImages",
                newName: "IX_PetImages_PetId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetImages",
                table: "PetImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetImages_Pets_PetId",
                table: "PetImages",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetImages_Pets_PetId",
                table: "PetImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetImages",
                table: "PetImages");

            migrationBuilder.RenameTable(
                name: "PetImages",
                newName: "ProductImages");

            migrationBuilder.RenameIndex(
                name: "IX_PetImages_PetId",
                table: "ProductImages",
                newName: "IX_ProductImages_PetId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Pets_PetId",
                table: "ProductImages",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
