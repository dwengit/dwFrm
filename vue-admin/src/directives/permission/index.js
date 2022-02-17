import { checkPermission } from '@/utils/permission';

const btnTypes = new Map();
btnTypes.set('search', '#18a689');

function checkDisPermission(el, binding, vnode) {
  const { value, oldValue } = binding;
  if (value === oldValue) {
    return;
  }
  if (value) {
    /* 获取当前所挂载的vue上下文节点路由信息 */
    const hasPermission = checkPermission([].concat(value));
    if (!hasPermission) {
      return el.parentNode && el.parentNode.removeChild(el);
    }
  } else {
    throw new Error(`没有操作权限`);
  }
}

export default {
  inserted(el, binding, vnode) {
    checkDisPermission(el, binding, vnode);
  },
  update(el, binding, vnode) {
    checkDisPermission(el, binding, vnode);
  }
};
