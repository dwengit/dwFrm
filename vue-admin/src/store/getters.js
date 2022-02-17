const getters = {
  errorLogs: state => state.errorLog.logs,
  roles: state => state.user.roles,
  permissions: state => state.user.permissions,
  avatar: state => state.user.userAvatar,
  sidebar: state => state.app.sidebar,
  size: state => state.app.size,
  device: state => state.app.device,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  permission_routes: state => state.permission.permissionRoutes
};
export default getters;
