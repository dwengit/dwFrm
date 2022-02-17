<template>
  <div class="app-container">
    <el-row :gutter="15">
      <el-col :span="4">
        <global-pad-card v-loading="loadingDeptTree" border-radius="10px" padding="14px">
          <div>
            <el-input v-model="deptName" placeholder="请输入部门名称" clearable size="small" prefix-icon="el-icon-search" style="margin-bottom: 20px" />
          </div>
          <div>
            <el-tree ref="tree" :data="deptOptions" :props="defaultProps" :expand-on-click-node="false" :filter-node-method="filterNode" default-expand-all @node-click="handleNodeClick" />
          </div>
        </global-pad-card>
      </el-col>
      <el-col :span="20">
        <collapse ref="collapse" :show="showSearch">
          <el-form :inline="true" :model="queryForm" class="demo-form-inline">
            <el-form-item label="菜单名称">
              <el-input v-model="queryForm.searchContent" />
            </el-form-item>
            <el-form-item label="菜单状态">
              <el-select v-model="queryForm.status">
                <el-option label="所有" :value="-1" />
                <el-option label="隐藏" :value="0" />
                <el-option label="显示" :value="1" />
              </el-select>
            </el-form-item>
            <el-form-item label="注册日期">
              <el-date-picker v-model="datePickerVal" type="datetimerange" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="['00:00:00']" />
            </el-form-item>
            <el-form-item>
              <permission-button code="System.User.Read" @click="search" />
            </el-form-item>
          </el-form>
        </collapse>
        <table-collapse>
          <el-row>
            <el-col :span="20">
              <el-button-group>
                <permission-button code="System.User.Add" @click.native="showDialogAddOrEdit('add')" />
              </el-button-group>
            </el-col>
            <right-toolbar :show-search.sync="showSearch" :columns="tableColumn" @queryTable="getList" />
          </el-row>
          <fixed-thead-table
            ref="fixedTheadTable"
            v-loading="loading"
            :columns="tableColumn"
            :page-data="pageData"
            :is-show-operate="true"
            :is-show-select="false"
            :operate-width="150"
            @pageOrPageSizeChange="pageOrPageSizeChange"
            @selection-change="handleSelectionChange"
          >
            <template slot="status" slot-scope="scope">
              <el-tooltip :content="scope.row.status!==1 ? '点击启用' : '点击禁用'" placement="top">
                <el-switch
                  v-model="scope.row.status"
                  active-color="#13ce66"
                  inactive-color="#ff4949"
                  :active-value="1"
                  :inactive-value="0"
                  @change="handleChangeUserStatus(scope.row.id, scope.row.status)"
                />
              </el-tooltip>
            </template>
            <template slot="gender" slot-scope="scope">
              <el-tag v-if="scope.row.gender === 1" effect="plain">男</el-tag>
              <el-tag v-else-if="scope.row.gender === 2" type="success" effect="plain">女</el-tag>
              <el-tag v-else type="danger" effect="plain">未知</el-tag>
            </template>
            <template slot="roleNames" slot-scope="scope">
              <template v-for="roleName in scope.row.roleNames">
                <el-tag :key="roleName" type="success" effect="plain">{{ roleName }}</el-tag>
              </template>
            </template>
            <template slot="operateBox" slot-scope="scope">
              <permission-button code="System.User.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
              <permission-button v-if="scope.row.accountCode !== 'admin'" code="System.User.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
              <permission-button code="System.User.AssignPermission" size="mini" type="text" @click="handleAssignPermission(scope.row)" />
              <permission-button v-if="scope.row.accountCode !== 'admin'" code="System.User.ResetPwd" size="mini" type="text" @click="onShowResetPwdDig(scope.row)" />
            </template>
          </fixed-thead-table>
        </table-collapse>
      </el-col>
    </el-row>

    <!-- 新增，修改 -->
    <el-dialog ref="dialogAddOrEditForm" v-el-drag-dialog :title="dialogAddOrEditTitle+'用户'" append-to-body :visible.sync="dialogAddOrEditFormVisible" width="40%" top="6vh">
      <el-form ref="addOrEditForm" :model="saveForm" :rules="saveFormRules" label-width="90px">
        <el-row>
          <el-col v-if="dialogAddOrEditTitle === '新增'" :span="12">
            <el-form-item label="用户账号" prop="accountCode">
              <el-input v-model="saveForm.accountCode" placeholder="请输入用户编码" clearable />
            </el-form-item>
          </el-col>
          <el-col v-if="dialogAddOrEditTitle === '新增'" :span="12">
            <el-form-item label="用户密码" prop="password">
              <el-input v-model="saveForm.password" type="password" placeholder="请输入用户密码" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="昵称" prop="nickName">
              <el-input v-model="saveForm.nickName" placeholder="请输入用户昵称" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="姓名" prop="userName">
              <el-input v-model="saveForm.userName" placeholder="请输入用户姓名" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="手机号码" prop="phoneNum">
              <el-input v-model="saveForm.phoneNum" placeholder="请输入手机号码" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="邮箱账号" prop="userEmail">
              <el-input v-model="saveForm.userEmail" placeholder="请输入邮箱账号" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="性别" prop="gender">
              <el-radio-group v-model="saveForm.gender">
                <el-radio :label="1">男</el-radio>
                <el-radio :label="2">女</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="账号状态" prop="status">
              <el-radio-group v-model="saveForm.status">
                <el-radio :label="1">可用</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="所属部门" prop="deptId">
              <dept-tree v-model="saveForm.deptId" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="角色分配" prop="roleIds">
              <el-select v-model="saveForm.roleIds" style="width:100%" multiple filterable allow-create clearable placeholder="请分配用户角色">
                <el-option v-for="item in roleOptions" :key="item.id" :label="item.roleName" :value="item.id" />
              </el-select>
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
    <assign-menu-functional-tree
      :assign-menu-functional-tree-visible.sync="assignMenuFunctionalTreeVisible"
      :enum-owner-identity-type="1"
      :assign-owner-id="assignOwnerId"
      @onAssignMenuFunctional="onAssignMenuFunctional"
    />
    <!-- 重置密码 -->
    <el-dialog ref="dialogResetPwd" title="重置密码" append-to-body :visible.sync="dialogResetPwdVisible" width="30%">
      <p>请输入用户"{{ selectRow.accountCode }}"的新密码</p>
      <el-input v-model="resetPwd" placeholder="请输入内容" clearable />
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogResetPwdVisible = false">取 消</el-button>
        <el-button type="primary" @click="handleChangeUserPwd()">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import elDragDialog from '@/directives/el-drag-dialog';
import deptTree from '../dept/components/deptTree.vue';
import AssignMenuFunctionalTree from '../menu-functional/components/assign-tree';
import { GetUserPage, GetUserInfo, AddUser, UpdateUser, DeleteUser, AllDeptTreeOptions, GetRoleOptions, ChangeUserStatus, ChangeUserPwd } from 'api/system';
export default {
  name: 'User',
  directives: { elDragDialog },
  components: { deptTree, AssignMenuFunctionalTree },
  data() {
    return {
      deptName: '',
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      deptOptions: [],
      roleOptions: [],
      showSearch: true,
      loading: false,
      loadingDeptTree: false,
      pageData: {},
      datePickerVal: [],
      queryForm: {
        searchContent: '',
        status: -1,
        deptId: null,
        beginTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10
      },
      dialogAddOrEditTitle: '新增',
      dialogAddOrEditFormVisible: false,
      saveForm: {
        accountCode: '',
        password: '',
        userName: '',
        nickName: '',
        gender: 1,
        phoneNum: '',
        userEmail: '',
        description: '',
        deptId: null,
        status: 1,
        roleIds: []
      },
      tableColumn: [
        { label: '账号', prop: 'accountCode', isShow: true },
        { label: '姓名', prop: 'UserName', isShow: false },
        { label: '昵称', prop: 'nickName', isShow: true },
        { label: '性别', prop: 'gender', width: 70, align: 'center', isShow: true },
        { label: '手机号码', prop: 'phoneNum', width: 110, align: 'center', isShow: true },
        { label: '所属组织', prop: 'deptAncestorsFullName', width: 180, isShow: true },
        { label: '拥有角色', prop: 'roleNames', width: 160, isShow: true },
        { label: '创建时间', prop: 'createTime', width: 160, align: 'center', isShow: true },
        { label: '状态', prop: 'status', width: 80, align: 'center', isShow: true }
      ],
      saveFormRules: {
        nickName: [
          { required: true, message: '用户昵称不能为空', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '用户密码不能为空', trigger: 'blur' }
        ],
        accountCode: [
          { required: true, message: '用户账号不能为空', trigger: 'blur' }
        ],
        deptId: [
          { required: true, message: '请选择部门', trigger: 'change' }
        ]
      },
      addOrEditForm: [],
      assignMenuFunctionalTreeVisible: false,
      assignOwnerId: null,
      dialogResetPwdVisible: false,
      selectRow: {}, // 当前选中行
      resetPwd: ''
    };
  },
  created() {
    this.getTreeselect();
    this.getList();
    this.getRoleOptions();
  },
  methods: {
    // 查询部门下拉树结构
    getTreeselect() {
      AllDeptTreeOptions().then((response) => {
        this.deptOptions = response.data;
      });
    },
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    getRoleOptions() {
      this.loadingDeptTree = true;
      GetRoleOptions().then(res => {
        this.roleOptions = res.data;
        this.loadingDeptTree = false;
      });
    },
    handleNodeClick(data) {
      this.queryForm.deptId = data.id;
      this.saveForm.deptId = data.id;
      this.getList();
    },
    search() {
      this.getList();
    },
    getList() {
      this.queryForm.beginTime = '';
      this.queryForm.endTime = '';
      if (this.datePickerVal && this.datePickerVal.length === 2) {
        this.queryForm.beginTime = this.datePickerVal[0];
        this.queryForm.endTime = this.datePickerVal[1];
      }
      this.loading = true;
      GetUserPage(this.queryForm).then(res => {
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
      GetUserInfo({ id: row.id }).then(res => {
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
      this.$confirm(`是否确认删除名称为"${row.userName}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteUser({ id: row.id });
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
          const saveUser = this.dialogAddOrEditTitle === '新增' ? AddUser(this.saveForm) : UpdateUser(this.saveForm);
          saveUser.then(res => {
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
    handleChangeUserStatus(id, value) {
      ChangeUserStatus({ id, value: String(value) }).then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.getList();
        this.msgSuccess('修改成功');
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    handleChangeUserPwd() {
      ChangeUserPwd({ id: this.selectRow.id, value: String(this.resetPwd) }).then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.dialogResetPwdVisible = false;
        this.resetPwd = '';
        this.msgSuccess('重置密码成功');
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    onShowResetPwdDig(row) {
      this.selectRow = row;
      this.dialogResetPwdVisible = true;
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
      const deptId = this.saveForm.deptId;
      this.saveForm = {
        accountCode: '',
        password: '',
        userName: '',
        nickName: '',
        gender: 1,
        phoneNum: '',
        userEmail: '',
        description: '',
        status: 1,
        roleIds: [],
        deptId
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
