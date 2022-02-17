import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '@/store';
import Layout from '@/layout';
import { getToken } from '@/utils/auth';
import NProgress from 'nprogress'; // progress bar
import 'nprogress/nprogress.css'; // progress bar style

// 路由懒加载
const login = () => import('@/views/login/index.vue');
const home = () => import('@/views/home/index.vue');
const articleOperate = () => import('@/views/blog/article/operate.vue');

Vue.use(VueRouter);
NProgress.configure({ showSpinner: false }); // NProgress Configuration

const constantRoutes = [
  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path(.*)',
        component: () => import('@/views/redirect/index')
      }
    ]
  },
  {
    path: '/',
    component: Layout,
    redirect: '/home',
    children: [
      {
        path: 'home',
        component: home,
        name: 'home',
        // hidden: true,
        meta: { title: '主页', icon: 'home', affix: true }
      },
      {
        path: '/blog/article/add',
        component: articleOperate,
        name: 'rticle-add',
        hidden: true,
        meta: { title: '新增博文' }
      },
      {
        path: '/blog/article/edit/:id',
        component: articleOperate,
        name: 'rticle-edit',
        hidden: true,
        meta: { title: '编辑博文' }
      },
      {
        path: '/blog/article/info/:id',
        component: articleOperate,
        name: 'rticle-edit',
        hidden: true,
        meta: { title: '博文详情' }
      }
    ]
  },
  { path: '/login', name: 'login', component: login, hidden: true },
  {
    path: '/401',
    component: () => import('@/views/error-page/401'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/error-page/404'),
    hidden: true
  },
  {
    path: '/500',
    component: () => import('@/views/error-page/500'),
    hidden: true
  }
  // { path: '*', redirect: '/404', hidden: true }
];

const createRouter = () => new VueRouter({
  mode: 'history',
  base: process.env.VUE_APP_ROUTER_BASE_DIR,
  routes: constantRoutes,
  scrollBehavior: () => ({ y: 0 })
});

const router = createRouter();

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
function resetRouter() {
  const newRouter = createRouter();
  router.matcher = newRouter.matcher; // reset router
}

const whiteList = ['/login', '/auth-redirect', '/401', '/404', '/500']; // 白名单列表
/* 挂载路由导航守卫,to表示将要访问的路径，from表示从哪里来，next是下一个要做的操作 next('/login')强制跳转login */
router.beforeEach(async(to, from, next) => {
  NProgress.start();
  /* 获取token */
  if (getToken()) {
    console.log(to.name);
    /* 访问登录页，放行 */
    if (to.path === '/login') {
      next({ path: '/' });
      NProgress.done();
    } else {
      /* 判断是否有角色 */
      const hasRoles = store.getters.roles && store.getters.roles.length > 0;
      if (hasRoles) {
        next();
      } else {
        try {
          /* 重新获取用户信息 */
          await store.dispatch('user/loadUserInfo');
          /* 重新获取权限并加载路由 */
          await store.dispatch('permission/loadPermission');

          next({ ...to, replace: true });
        } catch (error) {
          console.log(error);
        }
      }
    }
  } else {
    if (whiteList.indexOf(to.path) !== -1) {
      /* 白名单直接跳转 */
      next();
    } else {
      next(`/login?redirect=${to.path}`); // 否则全部重定向到登录页
    }
  }
});

router.afterEach(() => {
  // finish progress bar
  NProgress.done();
});

export { router as default, constantRoutes, resetRouter };
