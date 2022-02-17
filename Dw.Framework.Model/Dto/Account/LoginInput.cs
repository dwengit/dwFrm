using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dw.Framework.ApplicationCore.AppServices.Accounts.Dto
{
    public class LoginInput
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string AccountCode { get; set; }
        [Required(ErrorMessage = "密码不能空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "验证码不能空")]
        public string ValidateCode { get; set; }
        [Required(ErrorMessage = "请求异常，请重新刷新页面")]
        public string ValidateKey { get; set; }
    }
}
