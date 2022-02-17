using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class remove_test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "sys_role");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "CreateTime", "CurrentLevelCode" },
                values: new object[] { new DateTime(2021, 12, 12, 11, 42, 51, 89, DateTimeKind.Local).AddTicks(8422), 1 });

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                columns: new[] { "CreateTime", "Status" },
                values: new object[] { new DateTime(2021, 12, 12, 11, 42, 51, 89, DateTimeKind.Local).AddTicks(3849), 1 });

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 12, 11, 42, 51, 87, DateTimeKind.Local).AddTicks(2729));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 12, 11, 42, 51, 89, DateTimeKind.Local).AddTicks(5139));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "sys_role",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "CreateTime", "CurrentLevelCode" },
                values: new object[] { new DateTime(2021, 12, 12, 11, 41, 38, 122, DateTimeKind.Local).AddTicks(4532), 1 });

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                columns: new[] { "CreateTime", "Status" },
                values: new object[] { new DateTime(2021, 12, 12, 11, 41, 38, 122, DateTimeKind.Local).AddTicks(42), 1 });

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 12, 11, 41, 38, 119, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 12, 11, 41, 38, 122, DateTimeKind.Local).AddTicks(1150));
        }
    }
}
