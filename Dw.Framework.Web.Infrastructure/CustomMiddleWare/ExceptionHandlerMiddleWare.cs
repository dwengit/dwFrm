using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Helper;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dw.Framework.Web.Infrastructure.CustomMiddleWare
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                AsyncLocalThreadFactory<SysOperLog>.SetValue(new SysOperLog());
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            int code = (int)ResultCodeEnums.CODE500;
            string msg = "网络繁忙，请稍后重试";
            string logLevel = "info";
            string errMsg = null;

            //状态码
            if (exception is CustomException customException)
            {
                code = customException.Code;
                msg = customException.Message;
                if (code == (int)ResultCodeEnums.DATA_VALIDATE_ERR && string.IsNullOrEmpty(msg))
                {
                    msg = "操作的数据项，不存在或已被删除";
                }
                else if (code == (int)ResultCodeEnums.PERMISSION_NOT_ASSIGNED)
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
                else if (code == (int)ResultCodeEnums.CODE401 || code == (int)ResultCodeEnums.CODE403)
                {
                    response.StatusCode = code;
                    msg = ((ResultCodeEnums)code).GetDescription();
                }
            }
            else if (exception is ArgumentException)//参数异常
            {
                code = (int)ResultCodeEnums.PARAM_ERROR;
                msg = exception.Message;
            }
            else if (exception is Exception)
            {
                logLevel = "error";
                errMsg = exception.Message;
                context.Response.StatusCode = (int)ResultCodeEnums.CODE500;
            }

            //记录日志
            if (logLevel == "info")
            {
                Logger.Info("异常捕获", exception);
            }
            else
            {
                Logger.Error("异常捕获", exception);
            }

            response.ContentType = context.Request.Headers["Accept"];
            var res = new Respbase(msg, code);
            var sysOperLog = AsyncLocalThreadFactory<SysOperLog>.GetValue();

            if (sysOperLog != null && !sysOperLog.IsSaveData && !string.IsNullOrEmpty(sysOperLog.Title))
            {
                TimeSpan ts = DateTime.Now - sysOperLog.CreateTime;
                sysOperLog.Elapsed = ts.TotalMilliseconds;
                var sysOperLogService = (ISysOperLogAppService)context.RequestServices.GetService(typeof(ISysOperLogAppService));
                sysOperLog.JsonResult = res.ObjToJson();
                sysOperLog.ErrorMsg = errMsg ?? msg;
                sysOperLog.Status = EnumSysOperLogStatus.ERROR;
                await sysOperLogService.InsertOperlog(sysOperLog);
            }
            if (response.ContentType.ToLower() == "application/xml")
            {
                await response.WriteAsync(Object2XmlString(res)).ConfigureAwait(false);
            }
            else
            {
                response.ContentType = "application/json";
                await response.WriteAsync(res.ObjToJsonCamel()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 对象转为Xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string Object2XmlString(object o)
        {
            StringWriter sw = new StringWriter();
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(sw, o);
            }
            catch
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Dispose();
            }
            return sw.ToString();
        }
    }
}