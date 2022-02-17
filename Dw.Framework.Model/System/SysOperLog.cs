using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    public class SysOperLog : Entity<Guid>
    {
        /// <summary>
        /// 操作模块
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 0=其它,1=新增,2=修改,3=删除,4=授权,5=导出,6=导入,7=强退,8=生成代码,9=清空数据
        /// </summary>
        public int BusinessType { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 操作类别（0其它 1后台用户 2手机端用户）
        /// </summary>
        public int OperatorType { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string OperName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 请求url
        /// </summary>
        public string OperUrl { get; set; }
        /// <summary>
        /// 操作地址
        /// </summary>
        public string OperIp { get; set; }
        /// <summary>
        /// 操作地点
        /// </summary>
        public string OperLocation { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string OperParam { get; set; }
        /// <summary>
        /// 返回参数
        /// </summary>
        public string JsonResult { get; set; }
        /// <summary>
        /// 操作状态（0正常 1异常）
        /// </summary>
        public EnumSysOperLogStatus Status { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
        private double elapsed;
        /// <summary>
        /// 操作用时
        /// </summary>
        public double Elapsed
        {
            set
            {
                elapsed = value / 1000;
            }
            get
            {
                return Math.Truncate(elapsed * 1000) / 1000;
            }
        }
        /// <summary>
        /// 是否已保存数据, 默认未保存
        /// </summary>
        public bool IsSaveData { get; set; } = false;
    }
}
