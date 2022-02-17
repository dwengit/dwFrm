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
    public interface ISysOperLogAppService
    {
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="sysOperLog"></param>
        /// <returns></returns>
        Task InsertOperlog(SysOperLog sysOperLog);
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
        Task<Page<SysOperLogPage>> GetOperLogPage(string operName, string operModule, int businessType, EnumSysOperLogStatus? status, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize);
        /// <summary>
        /// 清空所有操作日志数据
        /// </summary>
        /// <returns></returns>
        Task ClearAllData();
        /// <summary>
        /// 删除操作日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteOperLog(List<Guid> ids);
    }
}
