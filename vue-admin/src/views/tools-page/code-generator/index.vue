<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="数据库地址">
          <el-input v-model="queryForm.addres" clearable />
        </el-form-item>
        <el-form-item label="数据库名称">
          <el-input v-model="queryForm.dbName" clearable />
        </el-form-item>
        <el-form-item label="用户名">
          <el-input v-model="queryForm.userName" clearable />
        </el-form-item>
        <el-form-item label="密码">
          <el-input v-model="queryForm.pwd" clearable />
        </el-form-item>
        <el-form-item label="端口">
          <el-input v-model="queryForm.port" clearable />
        </el-form-item>
        <el-form-item label="数据库类型">
          <el-select v-model="queryForm.dbType">
            <el-option label="Mysql" :value="0" />
            <el-option label="SqlServer" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-tooltip effect="dark" content="链接数据为空，默认使用服务器链接" placement="top-start">
            <permission-button code="Tools.Code.Read" @click="search" />
          </el-tooltip>
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-form ref="codeGen" :model="codeGenForm" :rules="codeGenFormRules" label-width="80px">
        <el-row :gutter="10">
          <el-col :span="8">
            <el-form-item label="命名空间" prop="namespaceName">
              <el-input v-model="codeGenForm.namespaceName" placeholder="项目命名空间前缀" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="数据库" prop="dbName">
              <el-select v-model="codeGenForm.dbName" style="width:100%;" @change="handleChangeDbSel">
                <el-option v-for="db in allDbs" :key="db" :label="db" :value="db" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <permission-button code="Tools.Code.Read" size="mini" icon="refresh" title="刷新数据" type="text" @click="refresh" />
            <permission-button code="Tools.Code.Build" @click="handleGenCode" />
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-transfer v-model="selectedTables" filterable :data="tables" :titles="['数据库表', '生成代码']" />
          </el-col>
        </el-row>
      </el-form>
    </table-collapse>
  </div>
</template>

<script>
import { GetAllDataBases, GetAllTables, CodeGenerate } from 'api/tools';
export default {
  name: 'CodeGenerator',
  data() {
    return {
      codeGenForm: {
        namespaceName: 'Dw.Framework',
        dbName: ''
      },
      codeGenFormRules: {
        namespaceName: [{
          required: true, message: '请输入命名空间', trigger: 'blur'
        }],
        dbName: [{
          required: true, message: '请选择数据库', trigger: 'blur'
        }]
      },
      showSearch: true,
      queryForm: {
        addres: '',
        dbName: '',
        userName: '',
        pwd: '',
        port: '',
        dbType: 0
      },
      tables: [],
      allDbs: [],
      selectedTables: []
    };
  },
  created() {
    this.search();
  },
  methods: {
    // 生成代码
    handleGenCode() {
      this.$refs['codeGen'].validate((valid) => {
        if (valid) {
          if (!this.selectedTables || this.selectedTables.length <= 0) {
            return this.$message.error('生成代码列表为空！');
          }
          const rloading = this.openLoading();
          CodeGenerate({ dbName: this.codeGenForm.dbName, namespaceName: this.codeGenForm.namespaceName, tableNames: this.selectedTables }).then(res => {
            rloading.close();
            if (!res.success) {
              return this.$message.error(res.msg);
            }
            this.msgSuccess('生成成功');
            window.location = process.env.VUE_APP_API_HOST + '/src/' + res.data.path;
          }).catch(err => {
            rloading.close();
            this.$message.error(err.msg);
          });
        }
      });
    },
    search() {
      this.loading = true;
      GetAllDataBases(this.queryForm).then(res => {
        this.loading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('链接数据库成功');
        this.allDbs = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
        this.loading = false;
      });
    },
    // 选择数据库除非
    handleChangeDbSel() {
      this.loading = true;
      this.queryForm.dbName = this.codeGenForm.dbName;
      GetAllTables(this.queryForm).then(res => {
        this.loading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.tables = res.data.map(m => { return { key: m.name, label: m.name }; });
      }).catch(err => {
        this.$message.error(err.msg);
        this.loading = false;
      });
    },
    // 刷新
    refresh() {
      this.search();
      this.tables = [];
      this.codeGenForm.dbName = '';
      this.selectedTables = [];
    }
  }
};
</script>

<style lang="scss" scoped>
  ::v-deep .el-transfer-panel {
    width: 500px;
    height: 500px;
    .el-transfer-panel__body{
      height: 500px;
    }
    .el-checkbox-group{
      height: 500px;
    }
  }
</style>
