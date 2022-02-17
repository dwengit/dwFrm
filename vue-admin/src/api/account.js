import axios from 'utils/http-axios.js';

// 登录获取Token
export const LoginApi = (data) => axios.post('/Login/Login', {
  ...data
});

// 登录用户信息
export const UserInfoApi = (data) => axios.get('/User/UserInfo', {
  ...data
});

// 获取用户权限
export const UserRoutesApi = (data) => axios.get('/Permission/UserRoutes', {
  ...data
});
