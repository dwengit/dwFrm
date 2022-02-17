import collapse from './Collapse/index.vue';
import tableCollapse from './Collapse/tableCollapse.vue';
import groupCollapsebtn from './Collapse/groupCollapsebtn.vue';
import permissionButton from './PermissionButton/index.vue';
import fixedTheadTable from './Table/FixedTheadTable.vue';
import RightToolbar from './RightToolbar/index.vue';
import GlobalPadCard from './GlobalPadCard/index.vue';

export default {
  install: Vue => {
    Vue.component('collapse', collapse); // 折叠面板
    Vue.component('table-collapse', tableCollapse); // 表格折叠面板
    Vue.component('group-collapsebtn', groupCollapsebtn); // 表格折叠控制按钮
    Vue.component('permission-button', permissionButton); // 公共权限按钮
    Vue.component('fixed-thead-table', fixedTheadTable); // 表格(固定标题，按标题顺序排序)
    Vue.component('right-toolbar', RightToolbar); // 表格右菜单
    Vue.component('global-pad-card', GlobalPadCard); // 全局卡片
  }
};
