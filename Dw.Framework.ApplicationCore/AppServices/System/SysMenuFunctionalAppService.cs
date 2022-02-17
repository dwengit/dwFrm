using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Account;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Helper;
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
    /// <summary>
    /// 菜单功能权限模块
    /// </summary>
    public class SysMenuFunctionalAppService : AppServiceBase, ISysMenuFunctionalAppService
    {

        private readonly IAccountService _accountService;
        private readonly ISysResourceRepository _sysResourceRepository;
        private readonly ISysPrivilegeRepository _sysPrivilegeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _sysUserRepository;

        public SysMenuFunctionalAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysResourceRepository sysResourceRepository, ISysPrivilegeRepository sysPrivilegeRepository, IAccountService accountService, ISysUserRepository sysUserRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysResourceRepository = sysResourceRepository;
            this._sysPrivilegeRepository = sysPrivilegeRepository;
            this._accountService = accountService;
            this._sysUserRepository = sysUserRepository;
        }
        /// <summary>
        /// 菜单功能权限模块
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<MenuFunctionalPageTree>> GetMenuFunctionalList(string resourceName, EnumSysModuleResourceState? state)
        {
            var allResource = await _sysResourceRepository.GetAllResourceFromCache();
            Func<SysModuleResource, bool> func = node =>
            {
                bool r = true;
                if (!string.IsNullOrEmpty(resourceName))
                {
                    r = node.ResourceName.ToLower().Contains(resourceName.ToLower());
                }
                if (state != null)
                {
                    return r && node.State == state;
                }
                return r;
            };
            if (string.IsNullOrEmpty(resourceName) && state == null)
            {
                func = null;
            }

            var treePage = ConvertTreeMenuFunctional(allResource.OrderBy(w => w.SortNO).ToList(), Guid.Empty, func);
            var res = _mapper.Map<List<MenuFunctionalPageTree>>(treePage);
            return res;
        }
        /// <summary>
        /// 获取详情-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MenuFunctionalDto> GetMenuFunctional(Guid id)
        {
            var data = await _sysResourceRepository.GetAsync(id);
            if (data == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            return _mapper.Map<MenuFunctionalDto>(data);
        }
        /// <summary>
        /// 保存-功能模块
        /// </summary>
        /// <param name="saveInput"></param>
        /// <returns></returns>
        public async Task<bool> SaveMenuFunctional(SaveMenuFunctionalInput saveInput)
        {
            saveInput.ValidateData(out string vldMsg);
            saveInput.FilterData(); //过滤一下数据
            if (saveInput.Id != Guid.Empty)
            {
                return await this.Update(saveInput);
            }
            return await this.Add(saveInput);
        }
        /// <summary>
        /// 删除-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMenuFunctional(Guid id)
        {
            var delData = await _sysResourceRepository.GetAsync(id);
            if (delData == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            RecursionResourceIds(delData.Id);
            _sysResourceRepository.HardDelete(delData);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _sysResourceRepository.RemoveResourceToCache(id);
            return res;
        }
        /// <summary>
        /// 更改状态-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldVal"></param>
        /// <returns></returns>
        public async Task<bool> SetMenuFunctional(Guid id, string fieldVal)
        {
            var upData = await _sysResourceRepository.GetAsync(id);
            if (upData == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            var fv = fieldVal.Split(':');
            if (fv.Length != 2)
            {
                throw new ArgumentException("参数错误");
            }
            if (fv[0].Equals("IsShow", StringComparison.OrdinalIgnoreCase))
            {
                upData.IsShow = bool.Parse(fv[1]);
            }
            else if (fv[0].Equals("State", StringComparison.OrdinalIgnoreCase))
            {
                upData.State = (EnumSysModuleResourceState)int.Parse(fv[1]);
            }
            else if (fv[0].Equals("SortNO", StringComparison.OrdinalIgnoreCase))
            {
                upData.SortNO = int.Parse(fv[1]);
            }
            else
            {
                throw new ArgumentException("类型错误");
            }
            await _sysResourceRepository.UpdateAsync(upData);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 获取全部-权限树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuFunctionalTreeOptions>> AllMenuFunctionalTreeOptions()
        {
            var allResource = await _sysResourceRepository.QueryNoTracking(w => !w.IsDelete).OrderBy(w => w.SortNO).ToListAsync();
            var tree = ConvertTreeMenuFunctional(allResource, Guid.Empty, null);
            return _mapper.Map<List<MenuFunctionalTreeOptions>>(tree);
        }
        /// <summary>
        /// 获取-分配功能模块权限树
        /// </summary>
        /// <param name="enumOwnerIdentityType"></param>
        /// <returns></returns>
        public async Task<AssignMenuFunctionalTree> GetAssignMenuFunctionalTree(EnumOwnerIdentityType enumOwnerIdentityType, Guid ownerId)
        {
            AssignMenuFunctionalTree res = new AssignMenuFunctionalTree();
            res.AssignMenuFunctionalTreeOptions = await AllMenuFunctionalTreeOptions();
            res.CheckedKeys = _sysPrivilegeRepository.QueryNoTracking(w => !w.IsDelete && w.OwnerType == enumOwnerIdentityType && w.OwnerId == ownerId).Select(w => w.ResourceId).ToList();
            return res;
        }
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="enumOwnerIdentityType"></param>
        /// <param name="ownerId"></param>
        /// <param name="MenuFunctionalIds"></param>
        /// <returns></returns>
        public async Task AssignMenuFunctional(AssignMenuFunctionalInput input, string accountCode)
        {
            var privilegeList = await _sysPrivilegeRepository.GetAllAsync(w => w.OwnerId == input.OwnerId && w.OwnerType == input.EnumOwnerIdentityType && !w.IsDelete);
            privilegeList.ForEach(item =>
            {
                if (!input.MenuFunctionalIds.Any(w => w == item.Id))
                {
                    _sysPrivilegeRepository.HardDelete(item);
                }
            });
            input.MenuFunctionalIds.ForEach(id =>
            {
                if (!privilegeList.Any() || !privilegeList.Any(w => w.Id == id))
                {
                    _sysPrivilegeRepository.Insert(new SysPrivilege()
                    {
                        OwnerId = input.OwnerId,
                        ResourceId = id,
                        OwnerType = input.EnumOwnerIdentityType,
                    });
                }
            });
            await _unitOfWork.SaveChangesAsync();
            await _accountService.RefreshUserAllPermissionIdsCatch(accountCode);
        }

        #region 方法
        /// <summary>
        /// 递归删除子集
        /// </summary>
        /// <param name="parentid"></param>
        private void RecursionResourceIds(Guid parentid)
        {
            var parentSysModuleResource = _sysResourceRepository.Query(m => m.ParentResourceID == parentid).ToList();
            if (!parentSysModuleResource.Any())
            {
                return;
            }
            foreach (var item in parentSysModuleResource)
            {
                RecursionResourceIds(item.Id);
                _sysResourceRepository.HardDelete(item);
            }
        }
        /// <summary>
        /// 转换为树形结构
        /// </summary>
        /// <param name="menuFunctionalLists"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private List<SysModuleResource> ConvertTreeMenuFunctional(List<SysModuleResource> menuFunctionalLists, Guid parentid, Func<SysModuleResource, bool> filterFunc)
        {
            List<SysModuleResource> treeMenuFunctionalList = new List<SysModuleResource>();
            var parentMenuFunctiona = menuFunctionalLists.Where(m => m.ParentResourceID == parentid).ToList();
            if (!parentMenuFunctiona.Any())
            {
                return treeMenuFunctionalList;
            }
            foreach (var item in parentMenuFunctiona)
            {
                bool isAdd = false;
                treeMenuFunctionalList.Add(item);
                if (filterFunc != null)
                {
                    isAdd = filterFunc(item);
                    if (!isAdd)
                    {
                        treeMenuFunctionalList.Remove(item);
                    }
                }
                if (item.ResourceType != EnumSysModuleResourceType.BUTTON)
                {
                    item.Children = ConvertTreeMenuFunctional(menuFunctionalLists, item.Id, filterFunc);
                    //有过滤条件 并且 筛选出的下一级有数据 并且 上一级没有匹配上
                    if (filterFunc != null && item.Children.Count > 0 && !isAdd)
                    {
                        treeMenuFunctionalList.Add(item);
                    }
                }
            }
            return treeMenuFunctionalList;
        }
        private async Task<bool> Add(SaveMenuFunctionalInput saveInput)
        {
            if(saveInput.ParentResourceID != Guid.Empty)
            {
                await CheckParentMount(saveInput.ResourceType, saveInput.ParentResourceID);
            }
            var data = _mapper.Map<SysModuleResource>(saveInput);
            await _sysResourceRepository.InsertAsync(data);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _sysResourceRepository.InsertResourceToCache(data);
            return res;
        }
        private async Task<bool> Update(SaveMenuFunctionalInput saveInput)
        {
            //检查要更新的（模块）是否有下级，如果有下级就（不能更改模块类型）
            var upData = await _sysResourceRepository.GetAsync(saveInput.Id);
            if (upData == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            if (saveInput.ResourceType != EnumSysModuleResourceType.MODULE && upData.ResourceType == EnumSysModuleResourceType.MODULE)
            {
                var upTypeName = upData.ResourceType.GetDescription();
                if (await _sysResourceRepository.AnyAsync(w => w.ParentResourceID == saveInput.Id))
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, $@"{upTypeName}下包含子集，不能更改{upTypeName}
                    类型");
                }
                if(saveInput.ParentResourceID != Guid.Empty)
                {
                    await CheckParentMount(saveInput.ResourceType, saveInput.ParentResourceID);
                }
            }
            _mapper.Map(saveInput, upData);
            await _sysResourceRepository.UpdateAsync(upData);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _sysResourceRepository.InsertResourceToCache(upData);
            return res;
        }
        /// <summary>
        /// 检查-模块只能挂载到模块下面
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="parentResourceID"></param>
        /// <returns></returns>
        private async Task CheckParentMount(EnumSysModuleResourceType resourceType, Guid parentResourceID)
        {
            if (resourceType == EnumSysModuleResourceType.MODULE)
            {
                var upTypeName = resourceType.GetDescription();
                var parentResourceData = await _sysResourceRepository.GetAsync(parentResourceID);
                if (parentResourceData == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, $@"上级菜单数据不存在，或已被删除");
                }
                if (parentResourceData.ResourceType != EnumSysModuleResourceType.MODULE)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, $@"{upTypeName}只能挂载到{upTypeName}下");
                }
            }
        }
        #endregion
    }
}
