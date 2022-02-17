import Vue from 'vue';
import VueRouter from 'vue-router';
import Layout from '@/layout';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    component: Layout,
    redirect: '/home',
    children: [
      {
        path: '/home',
        name: 'home',
        component: () => import('@/views/home/index.vue')
      },
      {
        path: '/archives',
        name: 'archives',
        component: () => import('@/views/archives/index.vue')
      },
      {
        path: '/category',
        name: 'category',
        component: () => import('@/views/category/index.vue')
      },
      {
        path: '/tags',
        name: 'tags',
        component: () => import('@/views/tags/index.vue')
      },
      {
        path: '/about',
        name: 'about',
        component: () => import('@/views/about/index.vue')
      },
      {
        path: '/articles',
        name: 'articles',
        component: () => import('@/views/articles/index.vue')
      },
      {
        path: '/articles/detail',
        name: 'articlesDetail',
        component: () => import('@/views/articles/detail.vue')
      }
    ]
  }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.VUE_APP_ROUTER_BASE_DIR,
  routes
});

export default router;
