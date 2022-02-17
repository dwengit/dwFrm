using Dw.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Infrastructure
{
    public class CustomException : Exception
    {
        public CustomException(string msg, int code)
        {
            Code = code;
            Msg = msg;
        }
        public int Code { get; set; }
        public string Msg { get; set; }

        public CustomException(string msg) : base(msg)
        {
        }
        public CustomException(int code, string msg) : base(msg)
        {
            Code = code;
            Msg = msg;
        }
        public CustomException(ResultCodeEnums resultCode, string msg = "") : base(msg)
        {
            Code = (int)resultCode;
            Msg = msg;
        }
    }
}
