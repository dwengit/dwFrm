using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.System.IService;
using Dw.Framework.ApplicationCore.Repositorys.Accounts;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dw.Framework.Infrastructure;
using System.Linq;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Model.System;
using Dw.Framework.Infrastructure.Utility;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    public class SysUserAppService : ISysUserAppService
    {
        private readonly ISysDeptRepository _sysDeptRepository;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly ISysUserRepository _sysUserRepository;
        private readonly ISysUserRoleRepository _sysUserRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SysUserAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysUserRoleRepository sysUserRoleRepository, ISysRoleRepository sysRoleRepository, ISysUserRepository sysUserRepository, ISysDeptRepository sysDeptRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysUserRepository = sysUserRepository;
            this._sysUserRoleRepository = sysUserRoleRepository;
            this._sysRoleRepository = sysRoleRepository;
            this._sysDeptRepository = sysDeptRepository;
        }
        public async Task<bool> DeleteUser(Guid id)
        {
            await _sysUserRepository.HardDeleteAsync(id);
            return _unitOfWork.SaveChanges() > 0;
        }
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<UserDto> GetUserInfo(Guid id)
        {
            var user = await _sysUserRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            var res = _mapper.Map<UserDto>(user);
            res.RoleIds = _sysUserRoleRepository.QueryNoTracking(w => w.UserId == id).Select(s => s.RoleId).ToList();
            return res;
        }
        /// <summary>
        /// 获取-用户分页
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="status"></param>
        /// <param name="deptId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Page<UserPage>> GetUserPage(string searchContent, EnumSysUserStatus? status, Guid? deptId, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            string deptAncestorsCode = "0.1";
            if (deptId != null)
            {
                var deptData = _sysDeptRepository.Get(deptId.Value);
                if (deptData == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "部门发生更新，请刷新页面");
                }
                deptAncestorsCode = deptData.AncestorsCode;
            }
            var userPage = await _sysUserRepository.QueryNoTracking()
                              .WhereIf(w => w.UserName.Contains(searchContent) || w.NickName.Contains(searchContent) || w.PhoneNum.Contains(searchContent), !string.IsNullOrEmpty(searchContent))
                              .WhereIf(w => w.CreateTime >= beginTime && w.CreateTime <= endTime, beginTime != null && endTime != null)
                              .OrderBy(w => w.CreateTime)
                              .Join(_sysDeptRepository.QueryNoTracking().WhereIf(w => w.AncestorsCode.Contains(deptAncestorsCode), deptAncestorsCode != "0.1"), user => user.DeptId, dept => dept.Id, (user, dept) =>
                                  new UserPage()
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      AccountCode = user.AccountCode,
                                      Avatar = user.Avatar,
                                      Gender = user.Gender,
                                      NickName = user.NickName,
                                      PhoneNum = user.PhoneNum,
                                      DeptId = user.DeptId,
                                      UserEmail = user.UserEmail,
                                      CreateTime = user.CreateTime,
                                      Status = user.Status,
                                      DeptAncestorsFullName = dept.AncestorsFullName + "." + dept.DeptName
                                  }
                              )
                              .ToPagedListAsync(pageIndex, pageSize);

            if (userPage.Items.Any())
            {
                foreach (var item in userPage.Items)
                {
                    item.RoleNames = GetSysRoles(item.Id).Select(s => s.RoleName);
                }
            }

            return _mapper.Map<Page<UserPage>>(userPage);
        }
        /// <summary>
        /// 根据用户Id获取所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SysRole> GetSysRoles(Guid userId)
        {
            var res = from roleUser in _sysUserRoleRepository.GetAll(w => w.UserId == userId)
                      join role in _sysRoleRepository.GetAll() on roleUser.RoleId equals role.Id
                      select role;

            return res.AsEnumerable();
        }
        /// <summary>
        /// 保存-用户
        /// </summary>
        /// <param name="saveUserInput"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> SaveUser(SaveUserInput saveUserInput)
        {
            if (saveUserInput.Id != Guid.Empty)
            {
                var updateUser = _sysUserRepository.Get(saveUserInput.Id);
                if (updateUser == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
                }
                updateUser.NickName = saveUserInput.NickName;
                updateUser.UserName = saveUserInput.UserName;
                updateUser.DeptId = saveUserInput.DeptId;
                updateUser.Description = saveUserInput.Description;
                updateUser.PhoneNum = saveUserInput.PhoneNum;
                updateUser.UserEmail = saveUserInput.UserEmail;
                updateUser.Gender = saveUserInput.Gender;
                updateUser.Status = saveUserInput.Status;
                _sysUserRepository.Update(updateUser);
            }
            else
            {
                saveUserInput.Id = Guid.NewGuid();
                saveUserInput.Password = Encrypt.MD5Encrypt(saveUserInput.Password);
                var addUser = _mapper.Map<SysUser>(saveUserInput);
                _sysUserRepository.Insert(addUser);
            }
            UpdateUserRole(saveUserInput.RoleIds, saveUserInput.Id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 更改-用户状态/重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldVal"></param>
        /// <returns></returns>
        public async Task ChangeUser(Guid id, string fieldVal)
        {
            var upData = await _sysUserRepository.GetAsync(id);
            if (upData == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            var fv = fieldVal.Split(':');
            if (fv.Length != 2)
            {
                throw new ArgumentException("参数错误");
            }
            if (fv[0].Equals("Pwd", StringComparison.OrdinalIgnoreCase))
            {
                upData.Password = Encrypt.MD5Encrypt(fieldVal[1].ToString());
            }
            else if (fv[0].Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                upData.Status = (EnumSysUserStatus)int.Parse(fv[1]);
            }
            else
            {
                throw new ArgumentException("类型错误");
            }
            await _sysUserRepository.UpdateAsync(upData);
            await _unitOfWork.SaveChangesAsync();
        }
        public void UpdateUserRole(List<Guid> roleIds, Guid userId)
        {
            var userRoles = _sysUserRoleRepository.GetAll(w => w.UserId.Equals(userId));
            foreach (var role in userRoles)
            {
                if (!roleIds.Any(w => w.Equals(role.Id)))
                {
                    _sysUserRoleRepository.HardDelete(role);
                }
            }
            foreach (var item in roleIds)
            {
                if (!userRoles.Any() || !userRoles.Any(w => w.UserId.Equals(item)))
                {
                    _sysUserRoleRepository.Insert(new SysUserRole()
                    {
                        UserId = userId,
                        RoleId = item
                    });
                }
            }
        }
    }
}
