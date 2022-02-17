using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dw.Framework.ApplicationCore.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_dept",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: false),
                    CurrentLevelCode = table.Column<int>(nullable: false, defaultValue: 100),
                    AncestorsCode = table.Column<string>(maxLength: 400, nullable: false),
                    AncestorsFullName = table.Column<string>(maxLength: 400, nullable: false),
                    DeptName = table.Column<string>(maxLength: 50, nullable: false),
                    SortNO = table.Column<int>(nullable: false),
                    Leader = table.Column<string>(maxLength: 20, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_dept", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_module_resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    ResourceCode = table.Column<string>(maxLength: 2000, nullable: false),
                    ResourceName = table.Column<string>(maxLength: 200, nullable: false),
                    ResourceIcon = table.Column<string>(maxLength: 200, nullable: false),
                    Path = table.Column<string>(maxLength: 200, nullable: true),
                    PathName = table.Column<string>(maxLength: 50, nullable: true),
                    NoCache = table.Column<bool>(nullable: false, defaultValue: false),
                    ResourceType = table.Column<int>(nullable: false),
                    ParentResourceID = table.Column<Guid>(maxLength: 36, nullable: false),
                    SortNO = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    IsShow = table.Column<bool>(nullable: false, defaultValue: false),
                    State = table.Column<int>(nullable: false, defaultValue: 1),
                    IsExternalLink = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_module_resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_privilege",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: false),
                    OwnerType = table.Column<int>(nullable: false),
                    ResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_privilege", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 20, nullable: false),
                    RoleCode = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    OrderSort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_user",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    AccountCode = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 32, nullable: false),
                    UserEmail = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 20, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    ContactInformation = table.Column<string>(maxLength: 100, nullable: true),
                    PhoneNum = table.Column<string>(maxLength: 32, nullable: true),
                    Education = table.Column<string>(maxLength: 20, nullable: true),
                    Hobby = table.Column<string>(maxLength: 200, nullable: true),
                    Introduce = table.Column<string>(maxLength: 500, nullable: true),
                    Photo = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_user_role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user_role", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "sys_dept",
                columns: new[] { "Id", "AncestorsCode", "AncestorsFullName", "CreateTime", "CurrentLevelCode", "DeleteTime", "DeptName", "Email", "IsDelete", "Leader", "Level", "ParentId", "Phone", "SortNO", "UpdateTime" },
                values: new object[] { new Guid("a89f00d7-57bf-11ec-ad18-0242ac110004"), "0", "总公司", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "总公司", null, false, null, 1, new Guid("00000000-0000-0000-0000-000000000000"), null, 1, null });

            migrationBuilder.InsertData(
                table: "sys_role",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "Description", "IsDelete", "OrderSort", "RoleCode", "RoleName", "Status", "UpdateTime" },
                values: new object[] { new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "系统管理员，拥有该系统整个权限", false, 1, "admin", "系统管理员", 1, null });

            migrationBuilder.InsertData(
                table: "sys_user",
                columns: new[] { "Id", "AccountCode", "ContactInformation", "CreateTime", "DeleteTime", "Education", "Gender", "Hobby", "Introduce", "IsDelete", "Password", "PhoneNum", "Photo", "UpdateTime", "UserEmail", "UserName" },
                values: new object[] { new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"), "123456", "", new DateTime(2021, 12, 12, 11, 33, 37, 848, DateTimeKind.Local).AddTicks(2609), null, "", 1, "", "管理员", false, "e10adc3949ba59abbe56e057f20f883e", "", null, null, "dwen@outlook.com", "admin" });

            migrationBuilder.InsertData(
                table: "sys_user_role",
                columns: new[] { "Id", "CreateTime", "DeleteTime", "IsDelete", "RoleId", "UpdateTime", "UserId" },
                values: new object[] { new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, new Guid("c81edfd1-4bb2-11ec-9aa7-0242ac110003"), null, new Guid("35fe9b5d-57cc-11ec-ad18-0242ac110004") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_dept");

            migrationBuilder.DropTable(
                name: "sys_module_resource");

            migrationBuilder.DropTable(
                name: "sys_privilege");

            migrationBuilder.DropTable(
                name: "sys_role");

            migrationBuilder.DropTable(
                name: "sys_user");

            migrationBuilder.DropTable(
                name: "sys_user_role");
        }
    }
}
