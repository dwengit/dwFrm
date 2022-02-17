import axios from 'axios';
import store from 'store';
import Toast from 'muse-ui-toast';

axios.defaults.baseURL = process.env.NODE_ENV === 'development' ? '/api' : process.env.VUE_APP_API_VERSION;
axios.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8';
axios.defaults.crossDomain = true;
axios.defaults.withCredentials = true; // 设置cross跨域 并设置访问权限 允许跨域携带cookie信息
axios.defaults.headers.common['Authorization'] = ''; // 设置请求头为 Authorization

// 请求拦截器
axios.interceptors.request.use(
  (config) => {
    // 判断是否存在token，如果存在的话，则每个http header都加上token
    const userInfo = store.getters['user/userInfo'];
    if (userInfo.isLoginNotExp) {
      config.headers.Authorization = userInfo.token;
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
        case 400:
          Toast.error('参数错误');
          break;
        case 401:
        case 403:
          Toast.error(msg);
          break;
        case 404:
          Toast.error('找不到请求路径，请联系管理员');
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
