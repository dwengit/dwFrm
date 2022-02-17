using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using IPTools.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.Web.Infrastructure.Filters
{
    /// <summary>
    /// 记录请求，输出日志
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {
        public BusinessType BusinessType { get; set; }
        /// <summary>
        /// 操作名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否保存请求数据
        /// </summary>
        public bool IsSaveRequestData { get; set; } = true;
        /// <summary>
        /// 是否保存返回数据
        /// </summary>
        public bool IsSaveResponseData { get; set; } = true;

        public LogAttribute() { }

        public LogAttribute(string name)
        {
            Title = name;
        }
        public LogAttribute(string name, BusinessType businessType, bool saveRequestData = true, bool saveResponseData = true)
        {
            Title = name;
            BusinessType = businessType;
            IsSaveRequestData = saveRequestData;
            IsSaveResponseData = saveResponseData;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor)) return;
            await base.OnActionExecutionAsync(context, next);

            // 获取当前的用户
            string userName = context.HttpContext.GetAccountCode();
            string requestBody = string.Empty;
            string jsonResult = string.Empty;
            string requestMethod = context.HttpContext.Request.Method.ToUpper();
            if (IsSaveResponseData)
            {
                if (HttpMethods.IsPost(requestMethod) || HttpMethods.IsPut(requestMethod))
                {
                    context.HttpContext.Request.Body.Position = 0;
                    using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                        //需要使用异步方式才能获取
                        requestBody = await reader.ReadToEndAsync();
                }
                else
                {
                    requestBody = context.HttpContext.Request.QueryString.Value.ToString();
                }
            }

            string controller = context.RouteData.Values["Controller"].ToString();
            string action = context.RouteData.Values["Action"].ToString();

            string ip = HttpContextExtension.GetClientUserIp(context.HttpContext);
            var ip_info = IpTool.Search(ip);

            var sysOperLog = AsyncLocalThreadFactory<SysOperLog>.GetValue();
            sysOperLog.Title = Title;
            sysOperLog.BusinessType = (int)BusinessType;
            sysOperLog.Status = EnumSysOperLogStatus.OK;
            sysOperLog.OperName = userName;
            sysOperLog.OperIp = ip;
            sysOperLog.OperUrl = HttpContextExtension.GetRequestUrl(context.HttpContext);
            sysOperLog.RequestMethod = requestMethod;
            sysOperLog.JsonResult = jsonResult;
            sysOperLog.OperLocation = ip_info.Province + " " + ip_info.City;
            sysOperLog.Method = controller + "." + action + "()";
            sysOperLog.OperParam = requestBody;
            sysOperLog.Elapsed = 0;
            sysOperLog.CreateTime = DateTime.Now;
        }

        public async override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor)) return;
            await base.OnResultExecutionAsync(context, next);
            try
            {
                var sysOperLog = AsyncLocalThreadFactory<SysOperLog>.GetValue();
                if (sysOperLog != null)
                {
                    if (IsSaveResponseData && context.Result is ObjectResult)
                    {
                        sysOperLog.JsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(((ObjectResult)context.Result).Value);
                    }
                    TimeSpan ts = DateTime.Now - sysOperLog.CreateTime;
                    sysOperLog.Elapsed = ts.TotalMilliseconds;
                    var sysOperLogService = (ISysOperLogAppService)context.HttpContext.RequestServices.GetService(typeof(ISysOperLogAppService));
                    await sysOperLogService.InsertOperlog(sysOperLog);
                    sysOperLog.IsSaveData = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"记录操作日志出错了", ex);
            }
           
        }

    }
}
