using AutoMapper;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Accounts
{
    public class SysLoginLogAppService : ISysLoginLogAppService
    {
        private readonly ISysLoginLogRepository _sysLoginLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SysLoginLogAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysLoginLogRepository sysLoginLogRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysLoginLogRepository = sysLoginLogRepository;
        }
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="SysLoginLog"></param>
        /// <returns></returns>
        public async Task InsertLoginLog(SysLoginLog sysLoginLog)
        {
            await _sysLoginLogRepository.InsertAsync(sysLoginLog);
            await _unitOfWork.SaveChangesAsync();
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
        public async Task<Page<SysLoginLogPage>> GetLoginLogPage(string accountCode, string ipAdd, EnumSysLoginLogStatus? status, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            var rolePage = await _sysLoginLogRepository.QueryNoTracking()
                            .WhereIf(w => w.AccountCode.Contains(accountCode), !string.IsNullOrEmpty(accountCode))
                            .WhereIf(w => w.Status == status, status != null)
                            .WhereIf(w => w.Ipaddr == ipAdd, !string.IsNullOrEmpty(ipAdd))
                            .WhereIf(w => w.CreateTime >= beginTime && w.CreateTime <= endTime, beginTime != null && endTime != null)
                            .OrderByDescending(w => w.CreateTime)
                            .ToPagedListAsync(pageIndex, pageSize);

            return _mapper.Map<Page<SysLoginLogPage>>(rolePage);
        }
        /// <summary>
        /// 清空所有登录日志数据
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllData()
        {
            await _sysLoginLogRepository.HardDeleteAsync(w => w.Id == w.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 删除登录日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteLoginLog(List<Guid> ids)
        {
            await _sysLoginLogRepository.HardDeleteAsync(w => ids.Contains(w.Id));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
