using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dw.Framework.Infrastructure
{
    public enum ResultCodeEnums
    {
        /// <summary>
        /// 操作失败
        /// </summary>
        FAIL = -1,
        [Description("参数错误")]
        PARAM_ERROR = -101,
        [Description("数据验证错误")]
        DATA_VALIDATE_ERR = -102,
        [Description("权限未分配")]
        PERMISSION_NOT_ASSIGNED = -201,
        [Description("演示模式，不能操作")]
        ISDEMOMODE = -202,
        [Description("错误的请求")]
        CODE400 = 400,
        [Description("未授权")]
        CODE401 = 401,
        [Description("权限不足")]
        CODE403 = 403,
        [Description("找不到资源")]
        CODE404 = 404,
        [Description("服务端异常")]
        CODE500 = 500,
        /// <summary>
        /// 操作/访问-成功
        /// </summary>
        [Description("成功")]
        Ok = 1000,
    }
}
