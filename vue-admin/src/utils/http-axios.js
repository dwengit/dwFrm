import axios from 'axios';
import { Message, MessageBox } from 'element-ui';
import { getToken } from 'utils/auth';
import store from 'store';
import router from 'router';

axios.defaults.baseURL = process.env.NODE_ENV === 'development' ? '/api' : process.env.VUE_APP_API_VERSION;
axios.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8';
axios.defaults.crossDomain = true;
axios.defaults.withCredentials = true; // 设置cross跨域 并设置访问权限 允许跨域携带cookie信息
axios.defaults.headers.common['Authorization'] = ''; // 设置请求头为 Authorization

// 请求拦截器
axios.interceptors.request.use(
  (config) => {
    // 判断是否存在token，如果存在的话，则每个http header都加上token
    const token = getToken();
    if (token) {
      config.headers.Authorization = token;
    }
    return config;
  },
  (error) => {
    return Promise.error(error);
  }
);

axios.interceptors.response.use(
  (response) => {
    // 如果返回的状态码为200，说明接口请求成功，可以正常拿到数据
    // 否则的话抛出错误
    if (response.status === 200) {
      return Promise.resolve(response.data);
    } else {
      return Promise.reject(response);
    }
  },
  (error) => {
    if (error.response.status) {
      var msg = error.response.data.msg;
      switch (error.response.status) {
        case 401:
        case 403:
          Message.error(msg);
          if (error.response.data.statusCode === -201) {
            // 未分配权限，登出
            store.dispatch('user/signOut').then(async() => {
              router.push('/404');
            });
            return;
          } else {
            var title = error.response.headers['token-expired'] === 'true' ? '登录状态已过期' : '无权访问';
            MessageBox.confirm(title + '，可以取消继续留在该页面，或者重新登录', '确定登出', {
              confirmButtonText: '重新登录',
              cancelButtonText: '取消',
              type: 'warning'
            }).then(async() => {
              await store.dispatch('user/signOut');
              router.push('/login');
            });
          }
          break;
        case 404:
          Message.error('找不到请求路径，请联系管理员');
          break;
        default:
          msg = msg || '网络繁忙，请稍后重试';
          break;
      }
      return Promise.reject(error.response.data);
    }
  }
);

export default axios;
