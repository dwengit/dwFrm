using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class UserPage
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 账号Code
        /// </summary>
        public string AccountCode { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public EnumGender Gender { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        public EnumSysUserStatus Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        private string deptAncestorsFullName;
        /// <summary>
        /// 部门全路径
        /// </summary>
        public string DeptAncestorsFullName
        {
            get { return deptAncestorsFullName; }
            set { deptAncestorsFullName = value.Replace('.', '/').Trim('/'); }
        }
        /// <summary>
        /// 用户拥有的角色名称
        /// </summary>
        public IEnumerable<string> RoleNames { get; set; }
    }
}
