using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_IsDisableComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisableComment",
                table: "blog_article",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 23, 19, 31, 644, DateTimeKind.Local).AddTicks(9603));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 23, 19, 31, 644, DateTimeKind.Local).AddTicks(3430));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 23, 19, 31, 641, DateTimeKind.Local).AddTicks(9250));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 23, 19, 31, 644, DateTimeKind.Local).AddTicks(5312));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisableComment",
                table: "blog_article");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 22, 58, 44, 360, DateTimeKind.Local).AddTicks(4342));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 22, 58, 44, 359, DateTimeKind.Local).AddTicks(6215));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 22, 58, 44, 355, DateTimeKind.Local).AddTicks(7308));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 4, 22, 58, 44, 359, DateTimeKind.Local).AddTicks(8176));
        }
    }
}
