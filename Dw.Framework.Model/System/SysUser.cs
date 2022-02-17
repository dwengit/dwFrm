using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 用户表
    /// </summary>
    public partial class SysUser : Entity<Guid>
    {
        /// <summary>
        /// 账号Code
        /// </summary>
        public string AccountCode { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
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
        /// 最后登录IP
        /// </summary>
        public string LoginIP { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LoginDate { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// </summary>
        public EnumSysUserStatus Status { get; set; }
        /// <summary>
        ///描述/备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 校验登录用户账号密码
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool ValidateLoginUser(string accountCode, string pwd)
        {
            return this.AccountCode.Equals(accountCode, StringComparison.OrdinalIgnoreCase) && Encrypt.MD5Encrypt(pwd).Equals(this.Password, StringComparison.OrdinalIgnoreCase);
        }
    }
}
