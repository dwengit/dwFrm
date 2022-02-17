using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Infrastructure
{
    public class Respbase
    {
        public Respbase()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public Respbase(string msg, int statusCode)
        {
            Msg = msg;
            StatusCode = statusCode;
        }
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回的状态
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get { return StatusCode > 0; } }
    }
    public class Respbase<T> : Respbase
    {
        public Respbase()
        {
        }
        public Respbase(string msg, int status, T data)
            : base(msg, status)
        {
            Data = data;
        }
        /// <summary>
        /// 响应的数据实体
        /// </summary>
        public T Data { get; set; }
    }
    public class RespbaseFac
    {
        public static Respbase RespOk(string msg = "ok", int code = 1000)
        {
            return new Respbase(msg, code);
        }
        public static Respbase<T> RespOk<T>(T t = default, string msg = "ok", int code = 1000)
        {
            return new Respbase<T>(msg, code, t);
        }
        public static Respbase<T> RespFail<T>(string msg = "fail", T t = default, int code = -1)
        {
            return new Respbase<T>(msg, code, t);
        }
        public static Respbase RespCustom(string msg, int code)
        {
            return new Respbase(msg, code);
        }
        public static Respbase<T> RespCustom<T>(string msg, int code, T t = default)
        {
            return new Respbase<T>(msg, code, t);
        }
    }
}
