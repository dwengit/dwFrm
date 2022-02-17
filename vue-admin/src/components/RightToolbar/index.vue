<!-- @author Shiyn/   huangmx 20200807优化-->
<template>
  <div class="top-right-btn">
    <el-button-group>
      <el-tooltip
        class="item"
        effect="dark"
        :content="showSearch ? '隐藏搜索' : '显示搜索'"
        placement="top"
      >
        <el-button
          @click="toggleSearch()"
          type="primary"
          icon="el-icon-search"
        ></el-button>
      </el-tooltip>
      <el-tooltip class="item" effect="dark" content="刷新" placement="top">
        <el-button
          @click="refresh()"
          type="primary"
          icon="el-icon-refresh"
        ></el-button>
      </el-tooltip>
      <el-tooltip
        class="item"
        effect="dark"
        content="显隐列"
        placement="top"
        v-if="columns"
      >
        <el-dropdown :hide-on-click="false" trigger="click">
          <el-button type="primary">
            <svg-icon icon-class="list" /><i
              class="el-icon-arrow-down el-icon--right"
            ></i>
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
      </el-tooltip>
    </el-button-group>
  </div>
</template>
<script>
export default {
  name: 'RightToolbar',
  data() {
    return {};
  },
  props: {
    showSearch: {
      type: Boolean,
      default: true
    },
    columns: {
      type: Array
    }
  },

  methods: {
    //搜索
    toggleSearch() {
      this.$emit('update:showSearch', !this.showSearch);
    },
    //刷新
    refresh() {
      this.$emit('queryTable');
    },
    // 打开显隐列dialog
    showColumn() {
      this.open = true;
    }
  }
};
</script>
<style lang="scss" scoped>
</style>
