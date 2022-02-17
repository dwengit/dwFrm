import Vue from 'vue';
import App from './App.vue';
import router from './router';
import 'muse-ui/lib/styles/base.less';
import '@/styles/global.less';
import 'lib-flexible'; // 适配移动端
import axios from 'axios';
import store from './store';
// 挂在到Vue实例，后面可通过this调用
Vue.prototype.$http = axios;

import 'muse-ui/lib/styles/base.less';
import {
  Button,
  Select,
  AppBar,
  Icon,
  Popover,
  List,
  Avatar,
  BottomSheet,
  Tooltip,
  Chip,
  Paper,
  Card,
  Pagination,
  Snackbar,
  TextField,
  Dialog,
  Divider,
  Badge,
  Form,
  AutoComplete,
  Progress,
  Menu
} from 'muse-ui';
import 'muse-ui/lib/styles/theme.less';
Vue.use(Button);
Vue.use(Select);
Vue.use(AppBar);
Vue.use(Icon);
Vue.use(Popover);
Vue.use(List);
Vue.use(Avatar);
Vue.use(BottomSheet);
Vue.use(Tooltip);
Vue.use(Chip);
Vue.use(Paper);
Vue.use(Card);
Vue.use(Pagination);
Vue.use(Snackbar);
Vue.use(TextField);
Vue.use(Dialog);
Vue.use(Divider);
Vue.use(Badge);
Vue.use(Form);
Vue.use(AutoComplete);
Vue.use(Progress);
Vue.use(Menu);
import Helpers from 'muse-ui/lib/Helpers';
Vue.use(Helpers);
import Loading from 'muse-ui-loading';
import 'muse-ui-loading/dist/muse-ui-loading.css';
Vue.use(Loading, {
  overlayColor: 'rgba(0, 0, 0, .6)', // 背景色
  size: 48,
  color: 'success', // color
  className: '' // loading class name
});

import Toast from 'muse-ui-toast';
Vue.use(Toast, {
  position: 'top', // 弹出的位置
  time: 2000, // 显示的时长
  closeIcon: ':iconfont icon-close', // 关闭的图标
  close: true, // 是否显示关闭按钮
  successIcon: ':iconfont icon-success-filling', // 成功信息图标
  infoIcon: ':iconfont icon-infofill', // 信息信息图标
  warningIcon: ':iconfont icon-warning-filling', // 提醒信息图标
  errorIcon: ':iconfont icon-error' // 错误信息图标
});

import VueLazyload from 'vue-lazyload';

Vue.use(VueLazyload, {
  preLoad: 1.3,
  error: '/images/loading.gif',
  loading: '/images/loading.gif',
  attempt: 1
});

import { isPC } from 'utils';
Vue.prototype.isPC = isPC;
import { randomColor } from '@/utils';
Vue.prototype.randomColor = randomColor;

Vue.config.productionTip = false;

import 'muse-ui-progress/dist/muse-ui-progress.css';
import NProgress from 'muse-ui-progress';
Vue.use(NProgress, {
  zIndex: 2000, // progress z-index
  top: 0, // position fixed top
  speed: 300, // progress speed
  color: 'success', // color
  size: 2, // progress size
  className: '' // progress custom class
});

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app');
