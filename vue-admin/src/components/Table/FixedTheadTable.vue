<template>
  <div class="table-container">
    <el-table :key="key" :data="pageData.items || []" border fit highlight-current-row style="width: 100%" v-bind="$attrs" v-on="$listeners">
      <el-table-column v-if="isShowSelect" type="selection" width="55" />
      <template v-for="column in tableColumn">
        <el-table-column v-if="column.isShow" :key="column.prop" :label="column.label" :prop="column.prop" :width="column.width" :align="column.align">
          <template slot-scope="scope">
            <slot :name="column.prop" :row="scope.row" :column="column.prop">{{
              scope.row[column.prop]
            }}</slot>
          </template>
        </el-table-column>
      </template>
      <el-table-column v-if="isShowOperate" fixed="right" label="操作" :width="operateWidth" align="center">
        <template slot-scope="scope">
          <slot name="operateBox" :row="scope.row" />
        </template>
      </el-table-column>
    </el-table>
    <!-- 分页栏 -->
    <div class="pagination-container">
      <el-pagination
        v-if="pagination"
        style="margin: 20px 0;"
        background
        layout="total, sizes, prev, pager, next, jumper"
        :page-sizes="[5, 10, 20, 30]"
        :page-size="pagination.pageSize"
        :total="pagination.total"
        :current-page="pagination.page"
        @size-change="handleSizeChange"
        @current-change="currentChange"
      />
    </div>

  </div>
</template>

<script>
export default {
  props: {
    // 表头信息
    columns: {
      type: Array,
      default() {
        return [];
      }
    },
    // 是否显示操作
    isShowOperate: {
      type: Boolean,
      default: true
    },
    // 列表数据
    pageData: {
      type: Object,
      default: () => {
        return [];
      }
    },
    // 操作栏宽度
    operateWidth: {
      type: Number,
      default: 100
    },
    // 是否显示多选框
    isShowSelect: {
      type: Boolean,
      default: false
    },
    // 表格高度
    tableHeight: {
      type: Number,
      default: 0
    },
    // laoding
    loading: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      tableColumn: this.columns,
      key: 1 // table key
    };
  },
  computed: {
    pagination() {
      return {
        page: this.pageData.pageIndex || 1,
        pageSize: this.pageData.pageSize || 20,
        total: this.pageData.total || 1
      };
    }
  },
  watch: {
    tableColumn() {
      this.key = this.key + 1; // 为了保证table 每次都会重渲 In order to ensure the table will be re-rendered each time
    }
  },
  mounted() {
  },
  methods: {
    currentChange(val) {
      this.$emit('pageOrPageSizeChange', {
        page: val,
        pageSize: this.pagination.pageSize
      });
    },
    handleSizeChange(val) {
      this.$emit('pageOrPageSizeChange', {
        page: 1,
        pageSize: val
      });
    }
  }
};
</script>

<style lang="scss" scoped>
  .table-container {
    margin-top: 15px;
  }
</style>
