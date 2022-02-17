using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    /// <summary>
    /// 菜单功能权限模块
    /// </summary>
    public interface ISysMenuFunctionalAppService
    {
        /// <summary>
        /// 获取-分配功能模块权限树
        /// </summary>
        /// <param name="enumOwnerIdentityType"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<AssignMenuFunctionalTree> GetAssignMenuFunctionalTree(EnumOwnerIdentityType enumOwnerIdentityType, Guid ownerId);
        /// <summary>
        /// 获取-菜单功能列表
        /// </summary>
        /// <returns></returns>
        Task<List<MenuFunctionalPageTree>> GetMenuFunctionalList(string resourceName, EnumSysModuleResourceState? state);
        /// <summary>
        /// 获取详情-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuFunctionalDto> GetMenuFunctional(Guid id);
        /// <summary>
        /// 保存-模块/菜单/按钮
        /// </summary>
        /// <param name="saveInput"></param>
        /// <returns></returns>
        Task<bool> SaveMenuFunctional(SaveMenuFunctionalInput saveInput);
        /// <summary>
        /// 删除-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteMenuFunctional(Guid id);
        /// <summary>
        /// 更改状态-模块/菜单/按钮
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldVal"></param>
        /// <returns></returns>
        Task<bool> SetMenuFunctional(Guid id, string fieldVal);
        /// <summary>
        /// 获取全部-权限树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuFunctionalTreeOptions>> AllMenuFunctionalTreeOptions();
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AssignMenuFunctional(AssignMenuFunctionalInput input, string accountCode);
    }
}
