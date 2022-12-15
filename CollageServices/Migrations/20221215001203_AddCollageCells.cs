using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageRipper.Migrations
{
    /// <inheritdoc />
    public partial class AddCollageCells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "collages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "collages",
                type: "TEXT",
                nullable: true);

            // perform an update operation to set Title and Note
            migrationBuilder.Sql("UPDATE collages SET Title=json_extract(data, '$.title'), Note=json_extract(data, '$.note'), data=json_extract(data, '$.data')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "collages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "collages");
        }

    }
}
