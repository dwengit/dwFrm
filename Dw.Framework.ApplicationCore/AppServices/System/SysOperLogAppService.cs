using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    public class SysOperLogAppService : ISysOperLogAppService
    {
        private readonly ISysOperLogRepository _sysOperLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SysOperLogAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysOperLogRepository sysOperLogRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysOperLogRepository = sysOperLogRepository;
        }
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="sysOperLog"></param>
        /// <returns></returns>
        public async Task InsertOperlog(SysOperLog sysOperLog)
        {
            await _sysOperLogRepository.InsertAsync(sysOperLog);
            await _unitOfWork.SaveChangesAsync();
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
        public async Task<Page<SysOperLogPage>> GetOperLogPage(string operName, string operModule, int businessType, EnumSysOperLogStatus? status,DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            var rolePage = await _sysOperLogRepository.QueryNoTracking()
                            .WhereIf(w => w.OperName.Contains(operName), !string.IsNullOrEmpty(operName))
                            .WhereIf(w => w.Title.Contains(operModule), !string.IsNullOrEmpty(operModule))
                            .WhereIf(w => w.BusinessType == businessType, businessType > -1)
                            .WhereIf(w => w.Status == status, status != null)
                            .WhereIf(w => w.CreateTime >= beginTime && w.CreateTime <= endTime, beginTime != null && endTime != null)
                            .OrderByDescending(w => w.CreateTime)
                            .ToPagedListAsync(pageIndex, pageSize);

            return _mapper.Map<Page<SysOperLogPage>>(rolePage);
        }
        /// <summary>
        /// 清空所有操作日志数据
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllData()
        {
            await _sysOperLogRepository.HardDeleteAsync(w => w.Id == w.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 删除操作日志数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteOperLog(List<Guid> ids)
        {
            await _sysOperLogRepository.HardDeleteAsync(w => ids.Contains(w.Id));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
