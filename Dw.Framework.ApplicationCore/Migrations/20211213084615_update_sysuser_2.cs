using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class update_sysuser_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "sys_user",
                maxLength: 200,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "sys_user");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "CreateTime", "CurrentLevelCode" },
                values: new object[] { new DateTime(2021, 12, 13, 13, 1, 38, 280, DateTimeKind.Local).AddTicks(4896), 1 });

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                columns: new[] { "CreateTime", "Status" },
                values: new object[] { new DateTime(2021, 12, 13, 13, 1, 38, 280, DateTimeKind.Local).AddTicks(179), 1 });

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 13, 1, 38, 277, DateTimeKind.Local).AddTicks(8863));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 13, 1, 38, 280, DateTimeKind.Local).AddTicks(1311));
        }
    }
}
