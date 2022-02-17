<!-- 折叠面板 -->
<template>
  <div style="float: right">
    <el-button-group>
      <el-button @click="onCollapse" type="primary" icon="el-icon-search"></el-button>
      <el-button @click="onSearch" type="primary" icon="el-icon-refresh"></el-button>
      <el-dropdown :hide-on-click="false" trigger="click" @command="handleCommand">
        <el-button type="primary">
          <svg-icon icon-class="list" /><i class="el-icon-arrow-down el-icon--right"></i>
        </el-button>
        <el-dropdown-menu slot="dropdown">
          <template v-for="column in columns">
            <el-dropdown-item :key="column.prop">
              <el-checkbox v-model="column.isShow">{{
                column.label
              }}</el-checkbox></el-dropdown-item
            >
          </template>
        </el-dropdown-menu>
      </el-dropdown>
    </el-button-group>
  </div>
</template>

<script>
export default {
  name: "groupCollapsebtn",
  components: {},
  props: {
    columns: {
      type: Array,
      default() {
        return [];
      },
    },
  },
  data() {
    return {};
  },
  methods: {
    handleCommand() {
      this.$parent.$parent.$refs.fixedTheadTable.$data.tableColumn = this.columns;
    },
    onCollapse() {
      this.$parent.$parent.$refs.collapse.$data.isShow = !this.$parent.$parent.$refs
        .collapse.$data.isShow;
    },

    onSearch() {
      this.$parent.$parent.onSearch();
    },
  },
  created() {},
  mounted() {},
};
</script>
<style lang="scss" scoped></style>
