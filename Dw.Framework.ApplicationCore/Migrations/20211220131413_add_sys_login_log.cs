using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_sys_login_log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_login_log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    AccountCode = table.Column<string>(nullable: false, defaultValue: "50"),
                    Status = table.Column<int>(nullable: false, defaultValue: 0),
                    Ipaddr = table.Column<string>(nullable: false, defaultValue: "200"),
                    LoginLocation = table.Column<string>(nullable: false, defaultValue: "50"),
                    Browser = table.Column<string>(nullable: true, defaultValue: "20"),
                    Os = table.Column<string>(nullable: true, defaultValue: "40"),
                    Msg = table.Column<string>(nullable: true, defaultValue: "50")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_login_log", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 759, DateTimeKind.Local).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 758, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 755, DateTimeKind.Local).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 759, DateTimeKind.Local).AddTicks(927));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_login_log");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 17, 18, 54, 11, DateTimeKind.Local).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 17, 18, 54, 11, DateTimeKind.Local).AddTicks(2578));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 17, 18, 54, 9, DateTimeKind.Local).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 17, 18, 54, 11, DateTimeKind.Local).AddTicks(3713));
        }
    }
}
