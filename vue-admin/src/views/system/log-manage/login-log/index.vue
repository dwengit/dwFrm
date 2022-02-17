<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="用户名">
          <el-input v-model="queryForm.accountCode" clearable />
        </el-form-item>
        <el-form-item label="登录地址">
          <el-input v-model="queryForm.ipAdd" clearable />
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="queryForm.status" style="width:200px">
            <el-option label="所有" :value="-1" />
            <el-option label="失败" :value="0" />
            <el-option label="成功" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item label="登录时间">
          <el-date-picker v-model="datePickerVal" type="datetimerange" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="['00:00:00']" />
        </el-form-item>
        <el-form-item>
          <permission-button code="System.LoginLog.Read" @click="getList()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="System.LoginLog.Delete" :disabled="multiple" @click.native="handleDelete()" />
            <permission-button code="System.LoginLog.Clear" @click.native="handleClear()" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" :columns="tableColumn" @queryTable="getList()" />
      </el-row>
      <fixed-thead-table
        ref="fixedTheadTable"
        v-loading="loading"
        :columns="tableColumn"
        :page-data="pageData"
        :is-show-operate="false"
        :is-show-select="true"
        :operate-width="260"
        @pageOrPageSizeChange="pageOrPageSizeChange"
        @selection-change="onSelectionChange"
      >
        <template slot="status" slot-scope="scope">
          <el-tag v-if="scope.row.status === 0" type="danger" effect="plain">失败</el-tag>
          <el-tag v-else type="success" effect="plain">成功</el-tag>
        </template>
      </fixed-thead-table>
    </table-collapse>
  </div>
</template>

<script>
import { GetLoginLogPage, ClearLoginLogAllData, DeleteLoginLog } from 'api/system';
export default {
  name: 'LoginLog',
  data() {
    return {
      showSearch: true,
      loading: false,
      pageData: {},
      datePickerVal: [],
      multiple: true, // 非多个禁用
      ids: [], // 当前列表选中的id
      queryForm: {
        accountCode: '',
        ipAdd: '',
        status: -1,
        beginTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10
      },
      tableColumn: [
        { label: '访问编码', prop: 'id', width: 290, align: 'center', isShow: true },
        { label: '登录账号', prop: 'accountCode', width: 120, align: 'center', isShow: true },
        { label: '登录IP', prop: 'ipaddr', width: 80, align: 'center', isShow: true },
        { label: '登录地点', prop: 'loginLocation', width: 160, align: 'center', isShow: true },
        { label: '浏览器', prop: 'browser', width: 80, align: 'center', isShow: true },
        { label: '操作系统', prop: 'os', width: 120, align: 'center', isShow: true },
        { label: '登录状态', prop: 'status', width: 80, align: 'center', isShow: true },
        { label: '操作消息', prop: 'msg', align: 'center', isShow: true },
        { label: '登录日期', prop: 'createTime', width: 180, align: 'center', isShow: true }
      ]
    };
  },
  created() {
    this.getList();
  },
  methods: {
    getList() {
      this.queryForm.beginTime = '';
      this.queryForm.endTime = '';
      if (this.datePickerVal && this.datePickerVal.length === 2) {
        this.queryForm.beginTime = this.datePickerVal[0];
        this.queryForm.endTime = this.datePickerVal[1];
      }
      this.loading = true;
      GetLoginLogPage(this.queryForm).then(res => {
        this.loading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.pageData = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
        this.loading = false;
      });
    },
    pageOrPageSizeChange(val) {
      this.queryForm.pageIndex = val.page;
      this.queryForm.pageSize = val.pageSize;
      this.getList();
    },
    onSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
      this.multiple = !selection.length;
    },
    handleDelete() {
      this.$confirm(`是否确认删除已选中的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const rloading = this.openLoading();
        DeleteLoginLog({ ids: this.ids.toString() }).then(res => {
          rloading.close();
          if (!res.success) {
            this.$message.error(res.msg);
            return;
          }
          this.msgSuccess('删除成功!');
          this.getList();
        }).catch(err => {
          rloading.close();
          this.$message.error(err.msg);
        });
      });
    },
    handleClear() {
      this.$confirm(`是否确认清空所有数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const rloading = this.openLoading();
        ClearLoginLogAllData().then(res => {
          rloading.close();
          if (!res.success) {
            this.$message.error(res.msg);
            return;
          }
          this.msgSuccess('清空成功!');
          this.getList();
        }).catch(err => {
          rloading.close();
          this.$message.error(err.msg);
        });
      });
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
