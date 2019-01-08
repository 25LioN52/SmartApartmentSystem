using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ActualStatus = table.Column<byte>(nullable: false),
                    ExpectedStatus = table.Column<byte>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ModuleId = table.Column<byte>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Hour = table.Column<byte>(nullable: false),
                    Minutes = table.Column<byte>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => new { x.ModuleId, x.Day, x.Hour, x.Minutes });
                    table.ForeignKey(
                        name: "FK_Schedules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "ActualStatus", "DeviceId", "ExpectedStatus", "IsActive", "IsDisabled", "Name" },
                values: new object[] { (byte)1, (byte)0, 1, (byte)0, false, false, "Boiler" });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ModuleId", "Day", "Hour", "Minutes", "Status" },
                values: new object[] { (byte)1, 2, (byte)8, (byte)30, (byte)25 });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ModuleId", "Day", "Hour", "Minutes", "Status" },
                values: new object[] { (byte)1, 2, (byte)9, (byte)0, (byte)20 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
