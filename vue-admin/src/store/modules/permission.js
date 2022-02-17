import router from '@/router';
import { UserRoutesApi } from 'api/account.js';
import Layout from '@/layout';
import ParentView from '@/components/ParentView';

const state = {
  permissionRoutes: [], /* 用户路由权限 */
  sidebarMenu: [] /* 导航菜单 */
};

const mutations = {
  SET_PERMISSIONS(state, routes) {
    state.permissionRoutes = routes;
  },
  CLEAR_PERMISSION(state) {
    state.permissionList = [];
  }
};

const actions = {
  async loadPermission({ commit }) {
    /* 根据用户权限设置对应路由 */
    const res = await UserRoutesApi();
    const UserRoutes = res.data || [];
    if (UserRoutes.length > 0) {
      /* 递归权限集合 */
      const routes = recursionRouter(UserRoutes);
      routes.push({ path: '*', redirect: '/404', hidden: true });

      /*  初始路由 */
      const initialRoutes = router.options.routes;

      /*  动态添加路由 */
      router.addRoutes(routes);
      const constRoutes = [...initialRoutes, ...routes];
      console.log('constRoutes ' + constRoutes);

      /* 完整的路由表 */
      commit('SET_PERMISSIONS', constRoutes);
    }
  }
};

// 遍历后台返回的路由字符串，转换为组件对象
function recursionRouter(asyncRouterMap) {
  return asyncRouterMap.filter(route => {
    if (route.component) {
      if (route.component === 'Layout') {
        route.component = Layout;
      } else if (route.component === 'ParentView') {
        route.component = ParentView;
      } else {
        route.component = loadView(route.component);
      }
    }
    if (route.children && route.children.length) {
      route.children = recursionRouter(route.children);
    }
    return true;
  });
}

export const loadView = (view) => { // 路由懒加载
  return (resolve) => require([`@/views/${view}`], resolve);
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
