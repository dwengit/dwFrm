using Dw.Framework.Infrastructure.Helper;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dw.Framework.Infrastructure
{
    public class Logger
    {
        private static ILog _logger;
        private static ILoggerRepository loggerRepository;

        static Logger()
        {
            loggerRepository = LogManager.CreateRepository("NetRepository");
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));
            _logger = LogManager.GetLogger(loggerRepository.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void Info(string throwMsg, Exception ex = null)
        {
            _logger.Info(FormatException(throwMsg, ex));
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="infoTitle"></param>
        /// <param name="infoMsg"></param>
        public static void Info(string infoTitle,string infoMsg)
        {
            _logger.Info(FormatInfo(infoTitle, infoMsg));
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="infoTitle"></param>
        /// <param name="obj"></param>
        public static void Info(string infoTitle, object obj)
        {
            _logger.Info(FormatInfo(infoTitle, obj.ObjToJson()));
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void Warn(string throwMsg, Exception ex = null)
        {
            _logger.Warn(FormatException(throwMsg, ex));
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void Error(string throwMsg, Exception ex = null)
        {
            _logger.Error(FormatException(throwMsg, ex));

        }

        public static string FormatInfo(string infoTitle,string infoMsg)
        {
            string errorMsg = string.Format("【INFO标题】：{0} <br>【INFO内容】：{1} <br>", new object[] { infoTitle, infoMsg });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            return errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
        }
        public static string FormatException(string throwMsg, Exception ex)
        {
            if (null == ex)
            {
                return throwMsg;
            }
            
            string errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            return errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
        }
    }
}
