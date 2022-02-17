using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class edit_BlogComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentIp",
                table: "blog_comment",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommentRootId",
                table: "blog_comment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CommentType",
                table: "blog_comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommenterType",
                table: "blog_comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 20, 25, 0, 653, DateTimeKind.Local).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 20, 25, 0, 653, DateTimeKind.Local).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 20, 25, 0, 651, DateTimeKind.Local).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 20, 25, 0, 653, DateTimeKind.Local).AddTicks(6055));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentIp",
                table: "blog_comment");

            migrationBuilder.DropColumn(
                name: "CommentRootId",
                table: "blog_comment");

            migrationBuilder.DropColumn(
                name: "CommentType",
                table: "blog_comment");

            migrationBuilder.DropColumn(
                name: "CommenterType",
                table: "blog_comment");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 14, 33, 26, 992, DateTimeKind.Local).AddTicks(5415));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 14, 33, 26, 992, DateTimeKind.Local).AddTicks(969));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 14, 33, 26, 989, DateTimeKind.Local).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 16, 14, 33, 26, 992, DateTimeKind.Local).AddTicks(2029));
        }
    }
}
