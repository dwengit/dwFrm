<template>
  <el-dialog ref="assignMenuFunctionalTreeVisible" v-el-drag-dialog title="分配角色权限" append-to-body :visible.sync="assignMenuFunctionalTreeVisible" width="40%" top="6vh">
    <el-button-group>
      <el-button type="primary" @click="onCheckedTreeExpand()">展开/折叠</el-button>
      <el-button type="danger" @click="onResetChecked()">清空选项</el-button>
    </el-button-group>
    <el-card class="box-card">
      <el-tree ref="assignMenuFunctionalTree" :data="menuFunctionalTreeData" show-checkbox node-key="id" :props="defaultProps" />
    </el-card>
    <div slot="footer" class="dialog-footer">
      <el-button @click="onCancel">取 消</el-button>
      <el-button type="primary" @click="onAssignMenuFunctional()">确 定</el-button>
    </div>
  </el-dialog>
</template>

<script>
import elDragDialog from '@/directives/el-drag-dialog';
import { GetAssignMenuFunctionalTree, AssignMenuFunctional } from 'api/system';
export default {
  name: 'AssignMenuFunctionalTree',
  directives: { elDragDialog },
  props: {
    assignOwnerId: {
      type: String,
      default: null
    },
    enumOwnerIdentityType: {
      type: Number,
      default: 1
    },
    assignMenuFunctionalTreeVisible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      isExpandAllAssignMenuFunctional: false,
      menuFunctionalTreeData: []
    };
  },
  watch: {
    assignMenuFunctionalTreeVisible: function(newVal, oldVal) {
      this.loadAssignMenuFunctional();
    }
  },
  methods: {
    loadAssignMenuFunctional() {
      const rloading = this.openLoading();
      GetAssignMenuFunctionalTree({ enumOwnerIdentityType: this.enumOwnerIdentityType, ownerId: this.assignOwnerId }).then(res => {
        rloading.close();
        if (this.$refs.assignMenuFunctionalTree !== undefined) {
          this.$refs.assignMenuFunctionalTree.setCheckedKeys([]);
        }
        this.menuFunctionalTreeData = res.data.assignMenuFunctionalTreeOptions;
        const checkedKeys = res.data.checkedKeys;
        if (this.$refs.assignMenuFunctionalTree !== undefined) {
          this.$refs.assignMenuFunctionalTree.setCheckedKeys([]);
        }
        var that = this;
        this.$nextTick(() => {
          // 模拟手动选中节点
          checkedKeys.forEach((i, n) => {
            var node = that.$refs.assignMenuFunctionalTree.getNode(i);
            console.log(node.isLeaf);
            if (node.isLeaf) {
              that.$refs.assignMenuFunctionalTree.setChecked(node, true);
            }
          });
        });
      });
    },
    onAssignMenuFunctional() {
      const rloading = this.openLoading();
      const checkIds = this.getMenuAllCheckedKeys();
      AssignMenuFunctional({
        menuFunctionalIds: checkIds,
        enumOwnerIdentityType: this.enumOwnerIdentityType,
        ownerId: this.assignOwnerId
      }).then(res => {
        rloading.close();
        if (res.success) {
          this.$message({ message: '分配权限成功', type: 'success' });
          this.$emit('onAssignMenuFunctional');
        } else {
          this.$message.error(res.msg);
        }
      });
    },
    // 所有菜单节点数据
    getMenuAllCheckedKeys() {
      // 目前被选中的菜单节点
      const checkedKeys = this.$refs.assignMenuFunctionalTree.getCheckedKeys();
      // 半选中的菜单节点
      const halfCheckedKeys = this.$refs.assignMenuFunctionalTree.getHalfCheckedKeys();
      checkedKeys.unshift.apply(checkedKeys, halfCheckedKeys);
      return checkedKeys;
    },
    onCheckedTreeExpand() {
      this.isExpandAllAssignMenuFunctional = !this.isExpandAllAssignMenuFunctional;
      for (let i = 0; i < this.menuFunctionalTreeData.length; i++) {
        this.$refs.assignMenuFunctionalTree.store.nodesMap[this.menuFunctionalTreeData[i].id].expanded = this.isExpandAllAssignMenuFunctional;
      }
    },
    onResetChecked() {
      this.checkedKeys = [];
      this.$refs.assignMenuFunctionalTree.setCheckedKeys([]);
    },
    onCancel() {
      this.$emit('update:assignMenuFunctionalTreeVisible', !this.assignMenuFunctionalTreeVisible);
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
