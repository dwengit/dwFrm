using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_BusinessId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Sys_File_busines",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 12, 13, 58, 37, 984, DateTimeKind.Local).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 12, 13, 58, 37, 983, DateTimeKind.Local).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 12, 13, 58, 37, 981, DateTimeKind.Local).AddTicks(3295));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 12, 13, 58, 37, 983, DateTimeKind.Local).AddTicks(7359));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Sys_File_busines");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 11, 19, 57, 41, 747, DateTimeKind.Local).AddTicks(7968));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 11, 19, 57, 41, 747, DateTimeKind.Local).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 11, 19, 57, 41, 745, DateTimeKind.Local).AddTicks(650));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 11, 19, 57, 41, 747, DateTimeKind.Local).AddTicks(4346));
        }
    }
}
