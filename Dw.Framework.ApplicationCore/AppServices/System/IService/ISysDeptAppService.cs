using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Accounts
{
    public interface ISysDeptAppService
    {
        /// <summary>
        /// 获取-部门页面树
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<List<DeptPageTree>> GetDeptPageTree(string deptName, EnumSysDept? status);
        /// <summary>
        /// 获取详情-部门详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeptDto> GetDeptInfo(Guid id);
        /// <summary>
        /// 保存-部门
        /// </summary>
        /// <param name="saveDeptInput"></param>
        /// <returns></returns>
        Task<bool> SaveDept(SaveDeptInput saveDeptInput);
        /// <summary>
        /// 删除-部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> DeleteDept(Guid Id);
        /// <summary>
        /// 懒加载-部门树
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        List<DeptTreeOptions> LazyDeptTreeOptions(Guid parentid);
        /// <summary>
        /// 全部加载-部门树
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        Task<List<DeptTreeOptions>> AllDeptTreeOptions();
    }
}
