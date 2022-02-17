using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class update_sysuser_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "sys_user",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "CreateTime", "CurrentLevelCode" },
                values: new object[] { new DateTime(2021, 12, 13, 21, 37, 13, 163, DateTimeKind.Local).AddTicks(8660), 1 });

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                columns: new[] { "CreateTime", "Status" },
                values: new object[] { new DateTime(2021, 12, 13, 21, 37, 13, 163, DateTimeKind.Local).AddTicks(4151), 1 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "sys_user");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "CreateTime", "CurrentLevelCode" },
                values: new object[] { new DateTime(2021, 12, 13, 16, 46, 15, 454, DateTimeKind.Local).AddTicks(3496), 1 });

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                columns: new[] { "CreateTime", "Status" },
                values: new object[] { new DateTime(2021, 12, 13, 16, 46, 15, 453, DateTimeKind.Local).AddTicks(9187), 1 });

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 16, 46, 15, 452, DateTimeKind.Local).AddTicks(178));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 16, 46, 15, 454, DateTimeKind.Local).AddTicks(228));
        }
    }
}
