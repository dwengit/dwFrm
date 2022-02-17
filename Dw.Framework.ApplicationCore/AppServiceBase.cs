using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore
{
    public class AppServiceBase : IAppServiceBase
    {
        public ServiceRunStatusInfo StatusInfo { get; set; } = new ServiceRunStatusInfo("成功", 1000);
        public ICollection<ServiceRunStatusInfo> ServiceRuns { get; set; } = new List<ServiceRunStatusInfo>();
        /// <summary>
        /// 操作友好提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Info(string msg, int code = 1000)
        {
            AddRunStatusInfo(msg, code);
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 操作警告
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Warning(string msg, int code = -1)
        {
            AddRunStatusInfo(msg, code);
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Fail(string msg, int code = -20)
        {
            AddRunStatusInfo(msg, code);
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 数据获取失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void DataFail(string msg = "数据不存在，或已被删除")
        {
            int code = -21;
            AddRunStatusInfo(msg, code);
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 校验数据失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void ValidateDataFail(string msg)
        {
            int code = -22;
            AddRunStatusInfo(msg, code);
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 通用执行成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void Ok(string msg = "ok", int code = 1000)
        {
            StatusInfo.Msg = msg;
            StatusInfo.StatusCode = code;
        }
        /// <summary>
        /// 自定义操作消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public void AddRunStatusInfo(string msg, int code)
        {
            ServiceRuns.Add(new ServiceRunStatusInfo(msg, code));
        }
    }
    public class ServiceRunStatusInfo
    {
        public ServiceRunStatusInfo(string msg, int code)
        {
            StatusCode = code;
            Msg = msg;
        }
        public int StatusCode { get; set; }
        public string Msg { get; set; }
    }
}
