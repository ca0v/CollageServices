using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageRipper.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "collages",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    filename = table.Column<string>(type: "TEXT", nullable: true),
                    created = table.Column<string>(type: "TEXT", nullable: true),
                    width = table.Column<long>(type: "INTEGER", nullable: true),
                    height = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recordings",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recordings", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collages");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "recordings");
        }
    }
}
