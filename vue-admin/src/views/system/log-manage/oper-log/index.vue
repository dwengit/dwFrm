<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="操作人员">
          <el-input v-model="queryForm.operName" clearable />
        </el-form-item>
        <el-form-item label="操作模块">
          <el-input v-model="queryForm.operModule" clearable />
        </el-form-item>
        <el-form-item label="操作类型">
          <el-select v-model="queryForm.businessType" style="width:200px">
            <el-option label="所有" :value="-1" />
            <el-option v-for="item in businessTypeArr" :key="item.val" :label="item.title" :value="item.val" />
          </el-select>
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="queryForm.status" style="width:200px">
            <el-option label="所有" :value="-1" />
            <el-option label="失败" :value="0" />
            <el-option label="成功" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item label="操作时间">
          <el-date-picker v-model="datePickerVal" type="datetimerange" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="['00:00:00']" />
        </el-form-item>
        <el-form-item>
          <permission-button code="System.SysOperLog.Read" @click="getList()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="System.SysOperLog.Delete" :disabled="multiple" @click.native="handleDelete()" />
            <permission-button code="System.SysOperLog.Clear" @click.native="handleClear()" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" :columns="tableColumn" @queryTable="getList()" />
      </el-row>
      <fixed-thead-table
        ref="fixedTheadTable"
        v-loading="loading"
        :columns="tableColumn"
        :page-data="pageData"
        :is-show-operate="true"
        :is-show-select="true"
        :operate-width="100"
        @pageOrPageSizeChange="pageOrPageSizeChange"
        @selection-change="onSelectionChange"
      >
        <template slot="elapsed" slot-scope="scope">
          {{ scope.row.elapsed }}s
        </template>
        <template slot="businessType" slot-scope="scope">
          {{ businessTypeArr.find(b => b.val === scope.row.businessType).title }}
        </template>
        <template slot="status" slot-scope="scope">
          <el-tag v-if="scope.row.status === 0" type="danger" effect="plain">失败</el-tag>
          <el-tag v-else type="success" effect="plain">成功</el-tag>
        </template>
        <template slot="operateBox" slot-scope="scope">
          <permission-button code="System.SysOperLog.Read" size="mini" icon="eye-open" title="详情" type="text" @click="onShowDialogInfo(scope.row)" />
        </template>
      </fixed-thead-table>
    </table-collapse>

    <!-- 新增，修改 -->
    <el-dialog v-if="infoForm" ref="dialogInfo" v-el-drag-dialog title="操作日志详情" append-to-body :visible.sync="dialogInfoVisible" width="40%" top="6vh">
      <el-form ref="infoForm" :model="infoForm" label-width="90px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="日志编码:"> {{ infoForm.id }}</el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="请求URL:"> {{ infoForm.operUrl }}</el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作模块:"> {{ infoForm.title }}</el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作人员:"> {{ infoForm.operName }} </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作类型:"> {{ businessTypeArr.find(b => b.val === infoForm.businessType).title }} </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="请求方式:"> {{ infoForm.requestMethod }} </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作人IP:"> {{ infoForm.operIp }} </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作地点:"> {{ infoForm.operLocation }} </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作状态:">
              <el-tag v-if="infoForm.status === 0" type="danger" effect="plain">失败</el-tag>
              <el-tag v-else type="success" effect="plain">成功</el-tag>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作用时:"> {{ infoForm.elapsed }}s </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="操作时间:"> {{ infoForm.createTime }} </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="请求参数:"> {{ infoForm.operParam }} </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="返回参数:"> {{ infoForm.jsonResult }} </el-form-item>
          </el-col>
          <el-col v-if="infoForm.errorMsg" :span="24">
            <el-form-item label="错误异常:"> {{ infoForm.errorMsg }} </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogInfoVisible = false">关 闭</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import elDragDialog from '@/directives/el-drag-dialog';
import { GetOperLogPage, ClearOperLogAllData, DeleteOperLog } from 'api/system';
export default {
  name: 'OperLog',
  directives: { elDragDialog },
  data() {
    return {
      dialogInfoVisible: false,
      infoForm: null,
      showSearch: true,
      loading: false,
      pageData: {},
      datePickerVal: [],
      multiple: true, // 非多个禁用
      ids: [], // 当前列表选中的id
      queryForm: {
        operName: '', // 操作人员
        operModule: '', // 操作模块
        businessType: -1, // 操作页面类型
        status: -1, // 操作状态
        beginTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10
      },
      businessTypeArr: [
        { title: '其它', val: 0 },
        { title: '新增', val: 1 },
        { title: '修改', val: 2 },
        { title: '删除', val: 3 },
        { title: '授权', val: 4 },
        { title: '导出', val: 5 },
        { title: '导入', val: 6 },
        { title: '强退', val: 7 },
        { title: '生成代码', val: 8 },
        { title: '清空数据', val: 9 }
      ],
      tableColumn: [
        { label: '日志编码', prop: 'id', width: 290, align: 'center', isShow: true },
        { label: '操作人员', prop: 'operName', width: 120, align: 'center', isShow: true },
        { label: '系统模块', prop: 'title', width: 120, align: 'center', isShow: true },
        { label: '请求方式', prop: 'requestMethod', width: 80, align: 'center', isShow: true },
        { label: '操作人IP', prop: 'operIp', width: 110, align: 'center', isShow: true },
        { label: '操作地点', prop: 'operLocation', width: 80, align: 'center', isShow: true },
        { label: '操作类型', prop: 'businessType', width: 120, align: 'center', isShow: true },
        { label: '操作状态', prop: 'status', width: 90, align: 'center', isShow: true },
        { label: '操作用时', prop: 'elapsed', width: 80, align: 'center', isShow: true },
        { label: '操作日期', prop: 'createTime', width: 170, align: 'center', isShow: true }
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
      GetOperLogPage(this.queryForm).then(res => {
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
        DeleteOperLog({ ids: this.ids.toString() }).then(res => {
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
        ClearOperLogAllData().then(res => {
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
    },
    onShowDialogInfo(row) {
      this.dialogInfoVisible = true;
      this.infoForm = row;
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
