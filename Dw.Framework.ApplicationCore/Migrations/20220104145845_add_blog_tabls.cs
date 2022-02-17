using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class add_blog_tabls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blog_article",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    ArticleTitle = table.Column<string>(maxLength: 500, nullable: false),
                    ArticleContent = table.Column<string>(nullable: false),
                    ArticleStatus = table.Column<int>(nullable: false, defaultValue: 0),
                    CoverImage = table.Column<string>(maxLength: 500, nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false),
                    TagIds = table.Column<string>(maxLength: 500, nullable: false),
                    Top = table.Column<int>(nullable: false, defaultValue: 0),
                    ViewNum = table.Column<int>(nullable: false, defaultValue: 0),
                    LikeNum = table.Column<int>(nullable: false, defaultValue: 0),
                    CommentNum = table.Column<int>(nullable: false, defaultValue: 0),
                    Auth = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_article", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_category",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    CategoryName = table.Column<string>(maxLength: 20, nullable: false),
                    ParentCategoryId = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    ArticleId = table.Column<Guid>(nullable: false),
                    CommentContent = table.Column<string>(maxLength: 500, nullable: false),
                    ParentCommentId = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    CommentLikeNum = table.Column<int>(nullable: false, defaultValue: 0),
                    CommentNickName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_link",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    LinkImage = table.Column<string>(maxLength: 500, nullable: false),
                    LinkTitle = table.Column<string>(maxLength: 50, nullable: false),
                    LinkUrl = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_link", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    TagName = table.Column<string>(maxLength: 50, nullable: false),
                    IsQuickNav = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_website_manage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    WebSitName = table.Column<string>(maxLength: 20, nullable: false),
                    BackImage = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_website_manage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_website_msg",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    MsgNickName = table.Column<string>(maxLength: 10, nullable: false),
                    MsgContent = table.Column<string>(maxLength: 500, nullable: false),
                    ParentMsgId = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_website_msg", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blog_article");

            migrationBuilder.DropTable(
                name: "blog_category");

            migrationBuilder.DropTable(
                name: "blog_comment");

            migrationBuilder.DropTable(
                name: "blog_link");

            migrationBuilder.DropTable(
                name: "blog_tag");

            migrationBuilder.DropTable(
                name: "blog_website_manage");

            migrationBuilder.DropTable(
                name: "blog_website_msg");

            migrationBuilder.UpdateData(
                table: "sys_dept",
                keyColumn: "Id",
                keyValue: new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 759, DateTimeKind.Local).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "sys_role",
                keyColumn: "Id",
                keyValue: new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 758, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "sys_user",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 755, DateTimeKind.Local).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "sys_user_role",
                keyColumn: "Id",
                keyValue: new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"),
                column: "CreateTime",
                value: new DateTime(2021, 12, 20, 21, 14, 12, 759, DateTimeKind.Local).AddTicks(927));
        }
    }
}
