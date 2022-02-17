using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System.IService
{
    public interface ISysUserAppService
    {
        /// <summary>
        /// 获取-用户分页
        /// </summary>
        /// <param name="searchContent">用户名/昵称/电话号码</param>
        /// <param name="status"></param>
        /// <param name="deptId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Page<UserPage>> GetUserPage(string searchContent, EnumSysUserStatus? status, Guid? deptId, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize);
        /// <summary>
        /// 获取-用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto> GetUserInfo(Guid id);
        /// <summary>
        /// 保存-用户
        /// </summary>
        /// <param name="saveUserInput"></param>
        /// <returns></returns>
        Task<bool> SaveUser(SaveUserInput saveUserInput);
        /// <summary>
        /// 删除-用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid id);
        /// <summary>
        /// 更改-用户状态/重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldVal"></param>
        /// <returns></returns>
        Task ChangeUser(Guid id, string fieldVal);
    }
}
