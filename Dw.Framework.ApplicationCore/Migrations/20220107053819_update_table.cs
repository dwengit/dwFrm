using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class update_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagName",
                table: "blog_tag");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "blog_category");

            migrationBuilder.AddColumn<string>(
                name: "TagTitle",
                table: "blog_tag",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryTitle",
                table: "blog_category",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 7, 13, 38, 18, 967, DateTimeKind.Local).AddTicks(5510));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 7, 13, 38, 18, 967, DateTimeKind.Local).AddTicks(63));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 7, 13, 38, 18, 964, DateTimeKind.Local).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 7, 13, 38, 18, 967, DateTimeKind.Local).AddTicks(1267));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagTitle",
                table: "blog_tag");

            migrationBuilder.DropColumn(
                name: "CategoryTitle",
                table: "blog_category");

            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "blog_tag",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "blog_category",
                type: "varchar(20) CHARACTER SET utf8mb4",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

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
    }
}
