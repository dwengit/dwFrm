using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Accounts
{
    public interface ISysLoginLogAppService
    {
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="sysOperLog"></param>
        /// <returns></returns>
        Task InsertLoginLog(SysLoginLog sysLoginLog);
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
        Task<Page<SysLoginLogPage>> GetLoginLogPage(string accountCode, string ipAdd, EnumSysLoginLogStatus? status, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize);
        /// <summary>
        /// 清空所有登录日志数据
        /// </summary>
        /// <returns></returns>
        Task ClearAllData();
        /// <summary>
        /// 删除登录日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteLoginLog(List<Guid> ids);
    }
}
