import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import axios from 'axios';
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import './icons';
import '@/styles/index.scss'; // global css
import mavonEditor from 'mavon-editor';
import 'mavon-editor/dist/css/index.css';
import directives from '@/directives/index.js';
import components from '@/components/index.js';
import { resetForm, clearValidateForm, openLoading } from '@/utils/index';

components.install(Vue);

// 全局方法挂载
Vue.prototype.resetForm = resetForm;
Vue.prototype.clearValidateForm = clearValidateForm;
Vue.prototype.openLoading = openLoading;

Vue.prototype.msgSuccess = function(msg) {
  this.$message({ showClose: true, message: msg, type: 'success' });
};

// 全局注册
Vue.use(mavonEditor);
Vue.use(ElementUI, {
  size: window.sessionStorage.getItem('size') || 'medium' // set element-ui default size
});

Object.keys(directives).forEach((fncName) => {
  Vue.directive(fncName, directives[fncName]);
});

// 挂在到Vue实例，后面可通过this调用
Vue.prototype.$http = axios;

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app');
