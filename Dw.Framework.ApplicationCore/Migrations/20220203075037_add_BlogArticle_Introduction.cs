using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_BlogArticle_Introduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "blog_article",
                maxLength: 256,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 2, 3, 15, 50, 37, 352, DateTimeKind.Local).AddTicks(509));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 2, 3, 15, 50, 37, 351, DateTimeKind.Local).AddTicks(5785));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 2, 3, 15, 50, 37, 349, DateTimeKind.Local).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 2, 3, 15, 50, 37, 351, DateTimeKind.Local).AddTicks(6959));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "blog_article");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 18, 15, 30, 9, 768, DateTimeKind.Local).AddTicks(8325));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 18, 15, 30, 9, 768, DateTimeKind.Local).AddTicks(3871));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 18, 15, 30, 9, 766, DateTimeKind.Local).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2022, 1, 18, 15, 30, 9, 768, DateTimeKind.Local).AddTicks(4942));
        }
    }
}
