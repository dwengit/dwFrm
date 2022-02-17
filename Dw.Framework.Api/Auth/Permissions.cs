using Dw.Framework.Web.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Auth
{
    public class _System
    {
        public const string User = "System.User";
        public const string UserAdd = "System.User.Add";
        public const string UserRead = "System.User.Read";
        public const string UserEdit = "System.User.Edit";
        public const string UserDelete = "System.User.Delete";
        public const string UserAssignPermission = "System.User.AssignPermission";
        public const string UserResetPwd = "System.User.ResetPwd";

        public const string Role = "System.Role";
        public const string RoleAdd = "System.Role.Add";
        public const string RoleEdit = "System.Role.Edit";
        public const string RoleDelete = "System.Role.Delete";
        public const string RoleRead = "System.Role.Read";
        public const string RoleAssignPermission = "System.Role.AssignPermission";

        public const string Dept = "System.Dept";
        public const string DeptAdd = "System.Dept.Add";
        public const string DeptEdit = "System.Dept.Edit";
        public const string DeptDelete = "System.Dept.Delete";
        public const string DeptRead = "System.Dept.Read";

        public const string MenuFunctional = "System.MenuFunctional";
        public const string MenuFunctionalRead = "System.MenuFunctional.Read";
        public const string MenuFunctionalAdd = "System.MenuFunctional.Add";
        public const string MenuFunctionalEdit = "System.MenuFunctional.Edit";
        public const string MenuFunctionalDelete = "System.MenuFunctional.Delete";

        public const string LoginLog = "System.LoginLog";
        public const string LoginLogRead = "System.LoginLog.Read";
        public const string LoginLogClear = "System.LoginLog.Clear";
        public const string LoginLogDelete = "System.LoginLog.Delete";

        public const string OperLog = "System.SysOperLog";
        public const string OperLogRead = "System.SysOperLog.Read";
        public const string OperLogClear = "System.SysOperLog.Clear";
        public const string OperLogDelete = "System.SysOperLog.Delete";
    }
    public class _Blog
    {
        /** 博文管理 **/
        public const string Article = "Blog.Article";
        public const string ArticleRead = "Blog.Article.Read";
        public const string ArticleDelete = "Blog.Article.Delete";
        public const string ArticleEdit = "Blog.Article.Edit";
        public const string ArticleAdd = "Blog.Article.Add";
        /** 博文标签 **/
        public const string Tag = "Blog.Tag";
        public const string TagAdd = "Blog.Tag.Add";
        public const string TagDelete = "Blog.Tag.Delete";
        public const string TagRead = "Blog.Tag.Read";
        public const string TagEdit = "Blog.Tag.Edit";
        /** 站点管理 **/
        public const string Manage = "Blog.Manage";
        public const string ManageRead = "Blog.Manage.Read";
        public const string ManageEdit = "Blog.Manage.Edit";
        /** 站点分类 **/
        public const string Category = "Blog.Category";
        public const string CategoryRead = "Blog.Category.Read";
        public const string CategoryAdd = "Blog.Category.Add";
        public const string CategoryEdit = "Blog.Category.Edit";
        public const string CategoryDelete = "Blog.Category.Delete";
        /** 博文评论 **/
        public const string Comment = "Blog.Comment";
        public const string CommentRead = "Blog.Comment.Read";
        public const string CommentAdd = "Blog.Comment.Add";
        public const string CommentDelete = "Blog.Comment.Delete";
        /** 友情链接 **/
        public const string Link = "Blog.Link";
        public const string LinkDelete = "Blog.Link.Delete";
        public const string LinkRead = "Blog.Link.Read";
        public const string LinkEdit = "Blog.Link.Edit";
        public const string LinkAdd = "Blog.Link.Add";
    }
    public class _Tools
    {
        /** 代码生成 **/
        public const string Code = "Tools.Code";
        public const string CodeRead = "Tools.Code.Read";
        public const string CodeBuild = "Tools.Code.Build";
    }
    public static class Operations
    {
        public static PermissionAuthorizationRequirement RoleAssignPermission =
            new PermissionAuthorizationRequirement { Name = "System.Role.AssignPermission" };
        public static PermissionAuthorizationRequirement UserAssignPermission =
            new PermissionAuthorizationRequirement { Name = "System.User.AssignPermission" };
    }
}
