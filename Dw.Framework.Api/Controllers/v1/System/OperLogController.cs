using Dw.Framework.Admin.Api.Auth;
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
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure;

namespace Dw.Framework.Admin.Api.Controllers.v1.System
{
    [Route("api/v{version:apiVersion}/System/[controller]/[action]")]
    [ApiController]
    public class OperLogController : ApiControllersBase
    {
        private readonly ISysOperLogAppService _sysOperLogAppService;

        public OperLogController(ISysOperLogAppService sysOperLogAppService)
        {
            this._sysOperLogAppService = sysOperLogAppService;
        }

        /// <summary>
        /// 操作日志-分页
        /// </summary>
        /// <param name="operName"></param>
        /// <param name="operModule"></param>
        /// <param name="businessType"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.OperLogRead)]
        public async Task<Respbase<Page<SysOperLogPage>>> GetOperLogPage(string operName, string operModule, int businessType, int? status, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            EnumSysOperLogStatus? statusEnum = null;
            if (Enum.IsDefined(typeof(EnumSysOperLogStatus), status))
            {
                statusEnum = (EnumSysOperLogStatus)status;
            }
            var res = await _sysOperLogAppService.GetOperLogPage(operName, operModule, businessType, statusEnum, beginTime, endTime, pageIndex, pageSize);
            return RespOk(res);
        }
        /// <summary>
        /// 清空所有操作日志数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.OperLogClear)]
        [Log(Title = "操作日志", BusinessType = BusinessType.CLEAN)]
        public async Task<Respbase> ClearOperLogAllData()
        {
            await _sysOperLogAppService.ClearAllData();
            return RespOk();
        }
        /// <summary>
        /// 删除操作日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.OperLogDelete)]
        [Log(Title = "操作日志", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteOperLog(string ids)
        {
            List<Guid> idsList = ids.Split(',').Select(s => Guid.Parse(s)).ToList();
            await _sysOperLogAppService.DeleteOperLog(idsList);
            return RespOk();
        }
    }
}
