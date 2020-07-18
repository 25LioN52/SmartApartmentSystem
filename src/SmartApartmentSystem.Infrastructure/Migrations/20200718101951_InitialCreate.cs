using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartApartmentSystem.Infrastructure.Migrations
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
                    ExpectedStatus = table.Column<byte>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleActuals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<byte>(nullable: false),
                    ActualStatus = table.Column<byte>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ChangeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleActuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleActuals_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sunday = table.Column<bool>(nullable: false),
                    Monday = table.Column<bool>(nullable: false),
                    Tuesday = table.Column<bool>(nullable: false),
                    Wednesday = table.Column<bool>(nullable: false),
                    Thursday = table.Column<bool>(nullable: false),
                    Friday = table.Column<bool>(nullable: false),
                    Saturday = table.Column<bool>(nullable: false),
                    Hour = table.Column<byte>(nullable: false),
                    Minutes = table.Column<byte>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    ModuleId = table.Column<byte>(nullable: false),
                    Status = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "ExpectedStatus", "IsDisabled", "Name" },
                values: new object[] { (byte)0, (byte)24, false, "Boiler" });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "ExpectedStatus", "IsDisabled", "Name" },
                values: new object[] { (byte)1, (byte)0, false, "Floor" });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "ExpectedStatus", "IsDisabled", "Name" },
                values: new object[] { (byte)2, (byte)1, false, "Water" });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleActuals_ModuleId",
                table: "ModuleActuals",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ModuleId",
                table: "Schedules",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleActuals");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
