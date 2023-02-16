using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLock.Field.WebApi.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsUnderMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "Field",
                columns: new[] { "Name", "IsUnderMaintenance", "MaintenanceTime" },
                values: new object[,]
                {
                    { "Field 01", false, null },
                    { "Field 02", false, null },
                    { "Field 03", false, null },
                    { "Field 04", false, null },
                    { "Field 05", false, null },
                    { "Field 06", false, null },
                    { "Field 07", false, null },
                    { "Field 08", false, null },
                    { "Field 09", false, null },
                    { "Field 10", false, null },
                    { "Field 11", false, null },
                    { "Field 12", false, null },
                    { "Field 13", false, null },
                    { "Field 14", false, null },
                    { "Field 15", false, null },
                    { "Field 16", false, null },
                    { "Field 17", false, null },
                    { "Field 18", false, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Field");
        }
    }
}
