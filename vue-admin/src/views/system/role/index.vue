<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="菜单名称">
          <el-input v-model="queryForm.roleName" clearable />
        </el-form-item>
        <el-form-item label="菜单状态">
          <el-select v-model="queryForm.status">
            <el-option label="所有" :value="-1" />
            <el-option label="隐藏" :value="0" />
            <el-option label="显示" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <permission-button code="System.Role.Read" @click="search" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="System.Role.Add" @click.native="showDialogAddOrEdit('add')" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" :columns="tableColumn" @queryTable="search" />
      </el-row>
      <fixed-thead-table
        ref="fixedTheadTable"
        v-loading="loading"
        :columns="tableColumn"
        :page-data="pageData"
        :is-show-operate="true"
        :is-show-select="false"
        :operate-width="260"
        @pageOrPageSizeChange="pageOrPageSizeChange"
        @selection-change="handleSelectionChange"
      >
        <template slot="status" slot-scope="scope">
          <el-tag v-if="scope.row.status === 0" effect="plain">停用</el-tag>
          <el-tag v-else type="success" effect="plain">启用</el-tag>
        </template>
        <template slot="operateBox" slot-scope="scope">
          <permission-button code="System.Role.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
          <permission-button v-if="scope.row.roleCode !== 'admin'" code="System.Role.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          <permission-button v-if="scope.row.roleCode !== 'admin'" code="System.Role.AssignPermission" size="mini" type="text" @click="handleAssignPermission(scope.row)" />
        </template>
      </fixed-thead-table>
    </table-collapse>

    <!-- 新增，修改 -->
    <el-dialog ref="dialogAddOrEditForm" v-el-drag-dialog :title="dialogAddOrEditTitle+'角色'" append-to-body :visible.sync="dialogAddOrEditFormVisible" width="35%" top="6vh">
      <el-form ref="addOrEditForm" :model="saveForm" :rules="saveFormRules" label-width="90px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="角色名称" prop="roleName">
              <el-input v-model="saveForm.roleName" placeholder="请输入部门名称" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="角色编码" prop="roleCode">
              <el-input v-model="saveForm.roleCode" placeholder="请输入角色编码" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="显示排序" prop="orderSort">
              <el-input-number v-model="saveForm.orderSort" :min="1" :max="99999" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="角色状态" prop="state">
              <el-radio-group v-model="saveForm.status">
                <el-radio :label="1">可用</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="备注" prop="description">
              <el-input v-model="saveForm.phone" type="textarea" :rows="2" placeholder="请输入备注" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancelForm">取 消</el-button>
        <el-button type="primary" @click="saveAddOrEditForm()">确 定</el-button>
      </div>
    </el-dialog>
    <!-- 分配权限 -->
    <assign-menu-functional-tree :assign-menu-functional-tree-visible.sync="assignMenuFunctionalTreeVisible" :enum-owner-identity-type="2" :assign-owner-id="assignOwnerId" @onAssignMenuFunctional="onAssignMenuFunctional" />
  </div>
</template>

<script>
import elDragDialog from '@/directives/el-drag-dialog';
import { GetRolePage, GetRoleInfo, AddRole, UpdateRole, DeleteRole } from 'api/system';
import AssignMenuFunctionalTree from '../menu-functional/components/assign-tree';
export default {
  name: 'Role',
  directives: { elDragDialog },
  components: { AssignMenuFunctionalTree },
  data() {
    return {
      showSearch: true,
      loading: false,
      pageData: {},
      queryForm: {
        roleName: '',
        status: -1,
        pageIndex: 1,
        pageSize: 10
      },
      dialogAddOrEditTitle: '新增',
      dialogAddOrEditFormVisible: false,
      saveForm: {
        roleName: '',
        RoleCode: '',
        orderSort: 1,
        description: '',
        status: 1
      },
      tableColumn: [
        { label: '角色名称', prop: 'roleName', isShow: true },
        { label: '角色编码', prop: 'RoleCode', isShow: true },
        { label: '角色描述', prop: 'description', width: 200, isShow: true },
        { label: '排序', prop: 'orderSort', width: 80, isShow: true },
        { label: '状态', prop: 'status', width: 80, align: 'center', isShow: true },
        { label: '创建时间', prop: 'createTime', width: 180, isShow: true }
      ],
      saveFormRules: {
        roleName: [
          { required: true, message: '角色名称不能为空', trigger: 'blur' }
        ],
        roleCode: [
          { required: true, message: '角色编码不能为空', trigger: 'blur' }
        ],
        orderSort: [
          { required: true, message: '请输入排序', trigger: 'change' }
        ]
      },
      addOrEditForm: [],
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      assignMenuFunctionalTreeVisible: false,
      assignOwnerId: null
    };
  },
  created() {
    this.getList();
  },
  methods: {
    search() {
      this.getList();
    },
    getList() {
      this.loading = true;
      GetRolePage(this.queryForm).then(res => {
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
    handleUpdate(row) {
      const rloading = this.openLoading();
      GetRoleInfo({ id: row.id }).then(res => {
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.saveForm = res.data;
        this.showDialogAddOrEdit('edit');
      }).catch(err => {
        rloading.close();
        this.$message.error(err.msg);
      });
    },
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.roleName}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteRole({ id: row.id });
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
          const saveRole = this.dialogAddOrEditTitle === '新增' ? AddRole(this.saveForm) : UpdateRole(this.saveForm);
          saveRole.then(res => {
            rloading.close();
            if (res.success) {
              this.$message({
                message: this.dialogAddOrEditTitle + '成功',
                type: 'success'
              });
              this.getList();
              this.cancelForm();
            } else {
              this.$message.error(res.msg);
              rloading.close();
            }
          }).catch(err => {
            this.$message.error(err.msg);
            rloading.close();
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
    cancelForm() {
      this.dialogAddOrEditFormVisible = false;
      this.reset();
    },
    // 表单重置
    reset() {
      this.saveForm = {
        roleName: '',
        RoleCode: '',
        orderSort: 1,
        description: '',
        status: 1
      };
      this.resetForm('addOrEditForm');
    },
    pageOrPageSizeChange(val) {
      this.queryForm.pageIndex = val.page;
      this.queryForm.pageSize = val.pageSize;
      this.getList();
    },
    handleSelectionChange(val) {
      console.log(val);
    },
    handleAssignPermission(row) {
      this.assignOwnerId = row.id;
      this.assignMenuFunctionalTreeVisible = true;
    },
    onAssignMenuFunctional() {
      this.assignMenuFunctionalTreeVisible = false;
      this.getList();
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
