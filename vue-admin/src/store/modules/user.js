import { UserInfoApi, LoginApi } from 'api/account.js';
import { setToken, removeToken } from '@/utils/auth';

const state = {
  userName: '',
  userAvatar: '',
  roles: [],
  permissions: []
};

const mutations = {
  SET_USER_INFO(state, data) {
    state.userName = data.userName;
    state.userAvatar = data.avatar;
    state.roles = data.roles;
    state.permissions = data.permissions;
  },
  CLEAAAR_USER_INFO(state) {
    state.userName = '';
    state.userAvatar = '';
    state.roles = [];
    state.permissions = [];
  }
};

const actions = {
  // 加载用户信息
  async loadUserInfo({ commit }) {
    const res = await UserInfoApi();
    commit('SET_USER_INFO', res.data);
  },
  // 登录
  async login(context, loginForm) {
    return new Promise((resolve, reject) => {
      LoginApi(loginForm).then(data => {
        const res = data;
        if (res.statusCode < 1) {
          return reject(res);
        }
        setToken(res.data.token_type + ' ' + res.data.token);
        return resolve();
      }).catch(error => {
        return reject(error);
      });
    });
  },
  // 注销
  async signOut({ commit }) {
    return new Promise((resolve) => {
      removeToken();
      commit('CLEAAAR_USER_INFO');
      return resolve();
    });
  }
};

const getters = {

};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};
