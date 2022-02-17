import axios from 'utils/http-axios.js';

// 登录用户信息
export const GetServerInfo = (data) => axios.get('/Monitor/GetServerInfo', {
  ...data
});
