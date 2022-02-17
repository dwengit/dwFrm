using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_sys_oper_log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_oper_log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    title = table.Column<string>(nullable: false, defaultValue: "100"),
                    businessType = table.Column<int>(nullable: false),
                    method = table.Column<string>(nullable: false, defaultValue: "100"),
                    requestMethod = table.Column<string>(nullable: false, defaultValue: "10"),
                    operatorType = table.Column<int>(nullable: false),
                    operName = table.Column<string>(nullable: false, defaultValue: "20"),
                    deptName = table.Column<string>(nullable: true, defaultValue: "50"),
                    operUrl = table.Column<string>(nullable: false, defaultValue: "200"),
                    operIp = table.Column<string>(nullable: false, defaultValue: "50"),
                    operLocation = table.Column<string>(nullable: true, defaultValue: "200"),
                    operParam = table.Column<string>(nullable: true),
                    jsonResult = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    errorMsg = table.Column<string>(nullable: true),
                    Elapsed = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_oper_log", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 16, 4, 50, 488, DateTimeKind.Local).AddTicks(3465));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 16, 4, 50, 487, DateTimeKind.Local).AddTicks(9439));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 16, 4, 50, 486, DateTimeKind.Local).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 16, 4, 50, 488, DateTimeKind.Local).AddTicks(480));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_oper_log");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 21, 37, 13, 163, DateTimeKind.Local).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 21, 37, 13, 163, DateTimeKind.Local).AddTicks(4151));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 21, 37, 13, 161, DateTimeKind.Local).AddTicks(3198));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 21, 37, 13, 163, DateTimeKind.Local).AddTicks(5238));
        }
    }
}
