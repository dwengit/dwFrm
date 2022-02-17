using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Admin.Api.Auth;

namespace Dw.Framework.Admin.Api.Controllers.v1.System
{
    [Route("api/v{version:apiVersion}/System/[controller]/[action]")]
    [ApiController]
    public class DeptController : ApiControllersBase
    {
        private readonly ISysDeptAppService _deptAppService;

        public DeptController(ISysDeptAppService deptAppService)
        {
            this._deptAppService = deptAppService;
        }
        /// <summary>
        /// 获取-部门页面树
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_System.DeptRead)]
        public async Task<Respbase<List<DeptPageTree>>> GetDeptPageTree(string deptName, int? status)
        {
            EnumSysDept? stateEnum = null;
            if (Enum.IsDefined(typeof(EnumSysDept), status))
            {
                stateEnum = (EnumSysDept)status;
            }
            var res = await _deptAppService.GetDeptPageTree(deptName, stateEnum);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-部门详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [PermissionFilter(_System.DeptRead)]
        public async Task<Respbase<DeptDto>> GetDeptInfo(Guid id)
        {
            var res = await _deptAppService.GetDeptInfo(id);
            return RespOk(res);
        }
        /// <summary>
        /// 新增-部门信息
        /// </summary>
        /// <param name="saveDeptInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.DeptAdd)]
        [Log(Title = "部门管理", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> AddDept(SaveDeptInput saveDeptInput)
        {
            var res = await _deptAppService.SaveDept(saveDeptInput);
            return RespOk();
        }
        /// <summary>
        /// 更新-部门信息
        /// </summary>
        /// <param name="saveDeptInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_System.DeptEdit)]
        [Log(Title = "部门管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> UpdateDept(SaveDeptInput saveDeptInput)
        {
            var res = await _deptAppService.SaveDept(saveDeptInput);
            return RespOk();
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_System.DeptDelete)]
        [Log(Title = "部门管理", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteDept(Guid Id)
        {
            var res = await _deptAppService.DeleteDept(Id);
            return RespOk();
        }
        /// <summary>
        /// 懒加载-部门树
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [HttpGet]
        public Respbase<List<DeptTreeOptions>> LazyDeptTreeOptions(Guid parentid)
        {
            var res = _deptAppService.LazyDeptTreeOptions(parentid);
            return RespOk(res);
        }
        /// <summary>
        /// 全部加载-部门树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Respbase<List<DeptTreeOptions>>> AllDeptTreeOptions()
        {
            var res = await _deptAppService.AllDeptTreeOptions();
            return RespOk(res);
        }
    }
}
