import axios from 'utils/http-axios.js';

// 获取-分配功能模块权限树
export const GetAssignMenuFunctionalTree = (data) => axios.get('/System/MenuFunctional/GetAssignMenuFunctionalTree', {
  params: { ...data }
});
// 菜单功能权限模块
export const GetMenuFunctionalList = (data) => axios.get('/System/MenuFunctional/GetMenuFunctionalList', {
  params: { ...data }
});
// 获取全部-权限树
export const AllMenuFunctionalTreeOptions = (data) => axios.get('/System/MenuFunctional/AllMenuFunctionalTreeOptions', {
  params: { ...data }
});
// 获取详情-模块/菜单/按钮
export const GetMenuFunctional = (data) => axios.get('/System/MenuFunctional/GetMenuFunctional', {
  params: { ...data }
});
// 新增-模块/菜单/按钮
export const AddMenuFunctional = (data) => axios.post('/System/MenuFunctional/AddMenuFunctional', {
  ...data
});
// 修改-模块/菜单/按钮
export const UpdateMenuFunctional = (data) => axios.post('/System/MenuFunctional/UpdateMenuFunctional', {
  ...data
});
// 分配权限
export const AssignMenuFunctional = (data) => axios.post('/System/MenuFunctional/AssignMenuFunctional', {
  ...data
});
// 删除-模块/菜单/按钮
export const DeleteMenuFunctional = (data) => axios.delete('/System/MenuFunctional/DeleteMenuFunctional', {
  params: { ...data }
});
// 修改单个数据-模块/菜单/按钮
export const SetMenuFunctional = (data) => axios.put('/System/MenuFunctional/SetMenuFunctional', {
  ...data
});

// 获取-页面树
export const GetDeptPageTree = (data) => axios.get('/System/Dept/GetDeptPageTree', {
  params: { ...data }
});
// 获取-部门详情
export const GetDeptInfo = (data) => axios.get('/System/Dept/GetDeptInfo', {
  params: { ...data }
});
// 新增-机构
export const AddDept = (data) => axios.post('/System/Dept/AddDept', {
  ...data
});
// 修改-机构
export const UpdateDept = (data) => axios.post('/System/Dept/UpdateDept', {
  ...data
});
// 删除-机构
export const DeleteDept = (data) => axios.delete('/System/Dept/DeleteDept', {
  params: { ...data }
});
// 懒加载-部门树
export const LazyDeptTreeOptions = (data) => axios.get('/System/Dept/LazyDeptTreeOptions', {
  params: { ...data }
});
// 全部加载-部门树
export const AllDeptTreeOptions = (data) => axios.get('/System/Dept/AllDeptTreeOptions', {
  params: { ...data }
});

// 获取-角色分页
export const GetRolePage = (data) => axios.get('/System/Role/GetRolePage', {
  params: { ...data }
});
// 获取-角色详情
export const GetRoleInfo = (data) => axios.get('/System/Role/GetRoleInfo', {
  params: { ...data }
});
// 获取-角色下拉树
export const GetRoleOptions = (data) => axios.get('/System/Role/GetRoleOptions', {
  params: { ...data }
});
// 新增-角色
export const AddRole = (data) => axios.post('/System/Role/AddRole', {
  ...data
});
// 修改-角色
export const UpdateRole = (data) => axios.post('/System/Role/UpdateRole', {
  ...data
});
// 删除-角色
export const DeleteRole = (data) => axios.delete('/System/Role/DeleteRole', {
  params: { ...data }
});

// 获取-用户分页
export const GetUserPage = (data) => axios.get('/System/User/GetUserPage', {
  params: { ...data }
});
// 获取-用户详情
export const GetUserInfo = (data) => axios.get('/System/User/GetUserInfo', {
  params: { ...data }
});
// 添加-用户
export const AddUser = (data) => axios.post('/System/User/AddUser', {
  ...data
});
// 修改-用户
export const UpdateUser = (data) => axios.post('/System/User/UpdateUser', {
  ...data
});
// 更改-重置密码
export const ChangeUserStatus = (data) => axios.put('/System/User/ChangeUserStatus', {
  ...data
});
// 重置-用户密码
export const ChangeUserPwd = (data) => axios.put('/System/User/ChangeUserPwd', {
  ...data
});
// 删除-用户
export const DeleteUser = (data) => axios.delete('/System/User/DeleteUser', {
  params: { ...data }
});

// 登录日志-分页
export const GetLoginLogPage = (data) => axios.get('/System/LoginLog/GetLoginLogPage', {
  params: { ...data }
});
// 清空所有登录日志数据
export const ClearLoginLogAllData = (data) => axios.delete('/System/LoginLog/ClearLoginLogAllData', {
  params: { ...data }
});
// 删除登录日志数据
export const DeleteLoginLog = (data) => axios.delete('/System/LoginLog/DeleteLoginLog', {
  params: { ...data }
});

// 操作日志-分页
export const GetOperLogPage = (data) => axios.get('/System/OperLog/GetOperLogPage', {
  params: { ...data }
});
// 清空所有操作日志数据
export const ClearOperLogAllData = (data) => axios.delete('/System/OperLog/ClearOperLogAllData', {
  params: { ...data }
});
// 删除操作日志数据
export const DeleteOperLog = (data) => axios.delete('/System/OperLog/DeleteOperLog', {
  params: { ...data }
});
