import { setToken, removeToken, getToken, parseToken } from '@/utils/auth';
const state = () => ({
  _userInfo: {
    avatar: '',
    nickName: '',
    exp: 0,
    isLoginNotExp: true,
    token: ''
  }
});

const mutations = {
  SET_USER_INFO(state, data) {
    setUserInfo(state, data);
  },
  CLEAAAR_USER_INFO(state) {
    state._userInfo.avatar = '';
    state._userInfo.nickName = '';
    state._userInfo.exp = '';
    state._userInfo.isLoginNotExp = false;
    state._userInfo.token = '';
  }
};
const setUserInfo = (state, data) => {
  state._userInfo.avatar = data.Avatar;
  state._userInfo.nickName = data.NickName;
  state._userInfo.exp = data.exp;
  state._userInfo.token = data.token;
  state._userInfo.isLoginNotExp = true;
};

const actions = {
  // 登录
  async login(context, loginForm) {
    return new Promise((resolve) => {
      let token = loginForm.token_type + ' ' + loginForm.token;
      setToken(token);
      let userInfo = parseToken(loginForm.token);
      userInfo.token = token;
      context.commit('SET_USER_INFO', userInfo);
      return resolve();
    });
  },
  // 登出
  async signOut({ commit }) {
    return new Promise((resolve) => {
      removeToken();
      commit('CLEAAAR_USER_INFO');
      return resolve();
    });
  }
};

const getters = {
  userInfo: (state) => {
    let res = state._userInfo;
    let token = getToken();
    if (!res.nickName && token) {
      res = parseToken(token);
      res.token = token;
      setUserInfo(state, res);
    }
    state._userInfo.isLoginNotExp = (res.exp - (Date.now() / 1000)) > 0;
    return state._userInfo;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
