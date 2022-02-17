using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dw.Framework.Infrastructure.Shared.Enums
{
    /**
     * 业务操作类型
     * 0=其它,1=新增,2=修改,3=删除,4=授权,5=导出,6=导入,7=强退,8=生成代码,9=清空数据
     */
    public enum BusinessType
    {
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        OTHER = 0,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        INSERT = 1,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("新增")]
        UPDATE = 2,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        DELETE = 3,
        /// <summary>
        /// 授权
        /// </summary>
        [Description("授权")]
        GRANT = 4,
        /// <summary>
        /// 导出
        /// </summary>
        [Description("导出")]
        EXPORT = 5,
        /// <summary>
        /// 导入
        /// </summary>
        [Description("导入")]
        IMPORT = 6,
        /// <summary>
        /// 强退
        /// </summary>
        [Description("强退")]
        FORCE = 7,
        /// <summary>
        /// 生成代码
        /// </summary>
        [Description("生成代码")]
        GENCODE = 8,
        /// <summary>
        /// 清空数据
        /// </summary>
        [Description("清空数据")]
        CLEAN = 9
    }
}
