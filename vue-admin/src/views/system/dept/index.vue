<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="部门名称">
          <el-input v-model="queryForm.deptName" clearable />
        </el-form-item>
        <el-form-item label="部门状态">
          <el-select v-model="queryForm.status">
            <el-option label="所有" :value="-1" />
            <el-option label="停用" :value="0" />
            <el-option label="启用" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <permission-button code="System.Dept.Read" @click="search()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="System.Dept.Add" @click.native="showDialogAddOrEdit('add')" />
            <permission-button code="System.Dept.Edit" title="修改排序" @click="showEditSort = !showEditSort" />
            <permission-button code="System.Dept.Read" type="info" plain icon="tree" title="展开/折叠" @click="toggleExpandAll(!isExpandAll)" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" @queryTable="getList" />
      </el-row>

      <el-table v-if="refreshTable" v-loading="loading" :data="pageTreeData" :default-expand-all="isExpandAll" row-key="id" border :tree-props="{ children: 'children', hasChildren: 'hasChildren' }">
        <el-table-column prop="deptName" label="部门名称" show-overflow-tooltip width="260" />
        <el-table-column prop="leader" label="负责人" show-overflow-tooltip />
        <el-table-column prop="phone" label="联系电话" show-overflow-tooltip />
        <el-table-column prop="createTime" label="创建日期" width="160" />
        <el-table-column prop="sortNO" label="排序" align="center" width="100" />
        <el-table-column prop="status" label="状态" align="center" width="100">
          <template slot-scope="scope">
            <el-tag v-if="scope.row.status === 0" effect="plain">停用</el-tag>
            <el-tag v-else type="success" effect="plain">启用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" align="center" show-overflow-tooltip width="260">
          <template slot-scope="scope">
            <permission-button code="System.Dept.Add" size="mini" type="text" @click="handleAdd(scope.row)" />
            <permission-button code="System.Dept.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
            <permission-button v-if="scope.row.level !== 1" code="System.Dept.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </table-collapse>

    <!-- 新增，修改 -->
    <el-dialog ref="dialogAddOrEditForm" v-el-drag-dialog :title="dialogAddOrEditTitle+'部门'" append-to-body :visible.sync="dialogAddOrEditFormVisible" width="700px" top="6vh">
      <el-form ref="addOrEditForm" :model="saveForm" :rules="saveFormRules" label-width="90px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="上级部门" prop="parentId">
              <dept-tree v-model="saveForm.parentId" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="部门名称" prop="deptName">
              <el-input v-model="saveForm.deptName" placeholder="请输入部门名称" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="显示排序" prop="sortNO">
              <el-input-number v-model="saveForm.sortNO" :min="1" :max="99999" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="联系人" prop="leader">
              <el-input v-model="saveForm.leader" type="textarea" :rows="1" placeholder="请输入联系人" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="联系电话" prop="phone">
              <el-input v-model="saveForm.phone" type="textarea" :rows="1" placeholder="请输入联系电话" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="部门状态" prop="state">
              <el-radio-group v-model="saveForm.status">
                <el-radio :label="1">可用</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancelForm">取 消</el-button>
        <el-button type="primary" @click="saveAddOrEditForm()">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import elDragDialog from '@/directives/el-drag-dialog';
import deptTree from './components/deptTree';
import { GetDeptPageTree, GetDeptInfo, AllDeptTreeOptions, AddDept, UpdateDept, DeleteDept } from 'api/system';
export default {
  name: 'Dept',
  directives: { elDragDialog },
  components: { deptTree },
  data() {
    return {
      refreshTable: true,
      isExpandAll: false,
      showSearch: true,
      loading: false,
      queryForm: {
        deptName: '',
        status: -1
      },
      pageTreeData: [],
      dialogAddOrEditTitle: '新增',
      dialogAddOrEditFormVisible: false,
      saveForm: {
        parentId: null,
        deptName: '',
        sortNO: 1,
        leader: '',
        status: 1
      },
      saveFormRules: {
        deptName: [
          { required: true, message: '部门名称不能为空', trigger: 'blur' }
        ],
        parentId: [
          { required: true, message: '请选择上级部门', trigger: 'change' }
        ],
        sortNO: [
          { required: true, message: '请输入排序', trigger: 'change' }
        ]
      },
      deptTreeOptions: [],
      addOrEditForm: []
    };
  },
  created() {
    this.getList();
  },
  methods: {
    search() {
      this.getList();
      this.toggleExpandAll(true);
    },
    getList() {
      this.loading = true;
      GetDeptPageTree(this.queryForm).then(res => {
        this.loading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.pageTreeData = res.data;
        this.loadDeptTree(); // 初始化下拉菜单
      }).catch(err => {
        this.$message.error(err.msg);
        this.loading = false;
      });
    },
    loadDeptTree() {
      AllDeptTreeOptions().then(res => {
        this.deptTreeOptions = res.data;
      });
    },
    normalizer(node) {
      if (node.children === null || !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children
      };
    },
    handleAdd(row) {
      this.saveForm.parentId = row.id;
      this.showDialogAddOrEdit('add');
    },
    handleUpdate(row) {
      GetDeptInfo({ id: row.id }).then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.saveForm = res.data;
        this.showDialogAddOrEdit('edit');
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.deptName}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteDept({ id: row.id });
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('删除成功!');
        this.getList();
      });
    },
    saveAddOrEditForm() {
      this.$refs['addOrEditForm'].validate((valid) => {
        if (valid) {
          const rloading = this.openLoading();
          const saveDept = this.dialogAddOrEditTitle === '新增' ? AddDept(this.saveForm) : UpdateDept(this.saveForm);
          saveDept.then(res => {
            if (res.success) {
              rloading.close();
              this.$message({
                message: this.dialogAddOrEditTitle + '成功',
                type: 'success'
              });
              this.getList();
              this.cancelForm();
            } else {
              rloading.close();
              this.$message.error(res.msg);
            }
          }).catch(err => {
            rloading.close();
            this.$message.error(err.msg);
          });
        }
      });
    },
    showDialogAddOrEdit(optType) {
      if (optType === 'add') {
        this.dialogAddOrEditTitle = '新增';
      } else {
        this.dialogAddOrEditTitle = '修改';
      }
      this.dialogAddOrEditFormVisible = true;
    },
    toggleExpandAll(isExpandAll) {
      this.refreshTable = false;
      this.isExpandAll = isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    cancelForm() {
      this.dialogAddOrEditFormVisible = false;
      this.reset();
    },
    // 表单重置
    reset() {
      this.saveForm = {
        parentId: null,
        deptName: '',
        sortNO: 1,
        leader: '',
        status: 1
      };
      this.resetForm('addOrEditForm');
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
