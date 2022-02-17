using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.System.IService;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    public class SysRoleAppService : ISysRoleAppService
    {
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SysRoleAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysRoleRepository sysRoleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysRoleRepository = sysRoleRepository;
        }
        /// <summary>
        /// 获取-角色分页
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Page<RolePage>> GetRolePage(string roleName, EnumSysRoleStatus? status, int pageIndex, int pageSize)
        {
            var roles = await _sysRoleRepository.GetAllRoleFromCache();
            var rolePage =  roles
                            .WhereIf(w => w.RoleName.Contains(roleName), !string.IsNullOrEmpty(roleName))
                            .WhereIf(w => w.Status == status, status != null)
                            .OrderBy(w => w.OrderSort)
                            .ThenBy(w => w.CreateTime)
                            .ToPagedList(pageIndex, pageSize);

            return _mapper.Map<Page<RolePage>>(rolePage);
        }
        /// <summary>
        /// 获取-角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleDto> GetRoleInfo(Guid id)
        {
            var role = await _sysRoleRepository.GetAsync(id);
            return _mapper.Map<RoleDto>(role);
        }
        /// <summary>
        /// 保存-角色
        /// </summary>
        /// <param name="saveRoleInput"></param>
        /// <returns></returns>
        public async Task<bool> SaveRole(SaveRoleInput saveRoleInput)
        {
            SysRole roleCatch = null;
            if (saveRoleInput.Id != Guid.Empty)
            {
                var upRole = await _sysRoleRepository.GetAsync(saveRoleInput.Id);
                if (upRole == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
                }
                _mapper.Map(saveRoleInput, upRole);
                roleCatch = await _sysRoleRepository.UpdateAsync(upRole);
            }
            else
            {
                var saveRole = _mapper.Map<SysRole>(saveRoleInput);
                roleCatch = _sysRoleRepository.Insert(saveRole);
            }
            bool res = await _unitOfWork.SaveChangesAsync() > 0;
            _sysRoleRepository.InsertRoleToCache(roleCatch);
            return res;
        }
        /// <summary>
        /// 删除-角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRole(Guid id)
        {
            await _sysRoleRepository.HardDeleteAsync(id);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _sysRoleRepository.RemoveRoleCache(id);
            return res;
        }
        /// <summary>
        /// 获取-角色下拉选项
        /// </summary>
        /// <returns></returns>
        public List<RoleOption> GetRoleOptions()
        {
            return _sysRoleRepository.QueryNoTracking(w => w.Status == EnumSysRoleStatus.ENABLE && !w.IsDelete).Select(w => new RoleOption()
            {
                Id = w.Id,
                RoleName = w.RoleName,
            }).ToList();
        }
    }
}
