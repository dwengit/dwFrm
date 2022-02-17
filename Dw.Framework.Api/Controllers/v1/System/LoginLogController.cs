using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Admin.Api.Auth;

namespace Dw.Framework.Admin.Api.Controllers.v1.System
{
    [Route("api/v{version:apiVersion}/System/[controller]/[action]")]
    [ApiController]
    public class LoginLogController : ApiControllersBase
    {
        private readonly ISysLoginLogAppService _sysLoginLogAppService;

        public LoginLogController(ISysLoginLogAppService sysLoginLogAppService)
        {
            this._sysLoginLogAppService = sysLoginLogAppService;
        }

        /// <summary>
        /// 登录日志-分页
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="ipAdd"></param>
        /// <param name="status"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.LoginLogRead)]
        public async Task<Respbase<Page<SysLoginLogPage>>> GetLoginLogPage(string accountCode, string ipAdd, int? status, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            EnumSysLoginLogStatus? statusEnum = null;
            if (Enum.IsDefined(typeof(EnumSysLoginLogStatus), status))
            {
                statusEnum = (EnumSysLoginLogStatus)status;
            }
            var res = await _sysLoginLogAppService.GetLoginLogPage(accountCode, ipAdd, statusEnum, beginTime, endTime, pageIndex, pageSize);
            return RespOk(res);
        }
        /// <summary>
        /// 清空所有登录日志数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.LoginLogClear)]
        [Log(Title = "操作日志", BusinessType = BusinessType.CLEAN)]
        public async Task<Respbase> ClearLoginLogAllData()
        {
            await _sysLoginLogAppService.ClearAllData();
            return RespOk();
        }
        /// <summary>
        /// 删除登录日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.LoginLogDelete)]
        [Log(Title = "操作日志", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteLoginLog(string ids)
        {
            List<Guid> idsList = ids.Split(',').Select(s => Guid.Parse(s)).ToList();
            await _sysLoginLogAppService.DeleteLoginLog(idsList);
            return RespOk();
        }
    }
}
