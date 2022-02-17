import store from '@/store';

const getPermission = value => {
  if (value) {
    const permissions = store.getters && store.getters.permissions;
    if (permissions && permissions.length > 0) {
      return permissions.find(pms => {
        return pms.permissionCode === value;
      });
    }
  }
  return null;
};

const checkPermission = value => {
  if (value && value instanceof Array && value.length > 0) {
    const permissions = store.getters && store.getters.permissions;
    if (permissions && permissions.length > 0) {
      return permissions.some(pms => {
        return value.includes(pms.permissionCode);
      });
    } else {
      return false;
    }
  } else {
    console.error('没有操作权限');
    return false;
  }
};

export { checkPermission, getPermission };
