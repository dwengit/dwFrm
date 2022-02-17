using Dw.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dw.Framework.Web.Infrastructure.Auth.Policys
{
    public class ApiResponse
    {
        public int StatusCode { get; set; } = 404;
        public string Msg { get; set; } = "No Found";

        public ApiResponse(ResultCodeEnums apiCode, string msg = null)
        {
            switch (apiCode)
            {
                case ResultCodeEnums.CODE401:
                    {
                        Msg = msg ?? "很抱歉，您无权访问该接口，请确保已经登录!";
                    }
                    break;
                case ResultCodeEnums.CODE403:
                    {
                        Msg = msg ?? "很抱歉，您的访问权限等级不够，联系管理员!";
                    }
                    break;
                case ResultCodeEnums.CODE500:
                    {
                        Msg = msg;
                    }
                    break;
            }
            StatusCode = (int)apiCode;
        }
    }
}
