using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class update_sysuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInformation",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "Hobby",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "Introduce",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "sys_user");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "sys_user",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeptId",
                table: "sys_user",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginDate",
                table: "sys_user",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginIP",
                table: "sys_user",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "sys_user",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "AncestorsCode", "AncestorsFullName", "CreateTime", "CurrentLevelCode" },
                values: new object[] { "0.1", "", new DateTime(2021, 12, 13, 13, 1, 38, 280, DateTimeKind.Local).AddTicks(4896), 1 });

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
                columns: new[] { "AccountCode", "Avatar", "CreateTime", "DeptId", "NickName", "UserName" },
                values: new object[] { "admin", "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fup.enterdesk.com%2Fedpic%2F0e%2F17%2Fa6%2F0e17a6c3b98c9128bfe420450ac4df56.jpg&refer=http%3A%2F%2Fup.enterdesk.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1641955220&t=a69ff4e2830ba1e41a4541136091f473", new DateTime(2021, 12, 13, 13, 1, 38, 277, DateTimeKind.Local).AddTicks(8863), new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"), "超级管理员", "超级管理员" });

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 13, 13, 1, 38, 280, DateTimeKind.Local).AddTicks(1311));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "LoginDate",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "LoginIP",
                table: "sys_user");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "sys_user");

            migrationBuilder.AddColumn<string>(
                name: "ContactInformation",
                table: "sys_user",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "sys_user",
                type: "varchar(20) CHARACTER SET utf8mb4",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hobby",
                table: "sys_user",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduce",
                table: "sys_user",
                type: "varchar(500) CHARACTER SET utf8mb4",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "sys_user",
                type: "varchar(400) CHARACTER SET utf8mb4",
                maxLength: 400,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                columns: new[] { "AncestorsCode", "AncestorsFullName", "CreateTime", "CurrentLevelCode" },
                values: new object[] { "0", "总公司", new DateTime(2021, 12, 12, 11, 42, 51, 89, DateTimeKind.Local).AddTicks(8422), 1 });

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
                columns: new[] { "AccountCode", "ContactInformation", "CreateTime", "Education", "Hobby", "Introduce", "UserName" },
                values: new object[] { "123456", "", new DateTime(2021, 12, 12, 11, 42, 51, 87, DateTimeKind.Local).AddTicks(2729), "", "", "管理员", "admin" });

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 12, 11, 42, 51, 89, DateTimeKind.Local).AddTicks(5139));
        }
    }
}
