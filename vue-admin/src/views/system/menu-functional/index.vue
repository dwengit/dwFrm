<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="formModule" class="demo-form-inline">
        <el-form-item label="功能名称">
          <el-input v-model="formModule.resourceName" clearable />
        </el-form-item>
        <el-form-item label="功能状态">
          <el-select v-model="formModule.state">
            <el-option label="所有" :value="-1" />
            <el-option label="停用" :value="0" />
            <el-option label="启用" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <permission-button code="System.MenuFunctional.Read" @click="search()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="System.MenuFunctional.Add" @click.native="showDialogAddOrEditMenu('add')" />
            <permission-button code="System.MenuFunctional.Edit" title="修改排序" @click="showEditSort = !showEditSort" />
            <permission-button code="System.MenuFunctional.Read" type="info" icon="module" title="展开/折叠" @click="toggleExpandAll(!isExpandAll)" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" :columns="columns" @queryTable="getList" />
      </el-row>

      <el-table
        v-if="refreshTable"
        v-loading="loading"
        :data="menuFunctionalTree"
        :default-expand-all="isExpandAll"
        row-key="id"
        border
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
        <el-table-column prop="resourceName" label="菜单/模块名称" show-overflow-tooltip width="160" />
        <el-table-column prop="icon" label="图标" align="center" width="75%">
          <template slot-scope="scope">
            <svg-icon :icon-class="scope.row.resourceIcon" class-name="svg-btn" />
          </template>
        </el-table-column>
        <el-table-column prop="icon" label="类型" align="center" width="100">
          <template slot-scope="scope">
            <el-tag v-if="scope.row.resourceType == 1" effect="plain">模块/目录</el-tag>
            <el-tag v-else-if="scope.row.resourceType == 2" type="success" effect="plain">菜单</el-tag>
            <el-tag v-else type="warning" effect="plain">功能/按钮</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="resourceCode" label="功能编码" :show-overflow-tooltip="true" />
        <el-table-column prop="pathName" label="路由名称" :show-overflow-tooltip="true">
          <template slot-scope="scope">
            {{ scope.row.pathName || '--' }}
          </template>
        </el-table-column>
        <el-table-column prop="path" label="组件地址" :show-overflow-tooltip="true">
          <template slot-scope="scope">
            {{ scope.row.path || '--' }}
          </template>
        </el-table-column>
        <el-table-column v-if="columns[0].isShow" prop="noCache" label="页面缓存" class="table-column-ok-icon" width="80">
          <template slot-scope="scope">
            <i v-if="scope.row.noCache" class="el-icon-check table-column-ok-icon" />
          </template>
        </el-table-column>
        <el-table-column prop="sortNO" label="排序" width="80">
          <template slot-scope="scope">
            <el-input v-if="showEditSort" v-model.number="scope.row.sortNO" size="mini" style="width:50px" controls-position="no" @blur="setMenuVal(scope.row.id, 'sortNO:'+scope.row.sortNO)" />
            <span v-else>{{ scope.row.sortNO }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="isShow" label="显示" width="80" align="center">
          <template slot-scope="scope">
            <el-tooltip :content="!scope.row.isShow ? '点击显示' : '点击隐藏'" placement="top">
              <el-switch v-model="scope.row.isShow" active-color="#13ce66" inactive-color="#909399" @change="setMenuVal(scope.row.id, 'isShow:'+scope.row.isShow)" />
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column prop="isShow" label="状态" width="80" align="center">
          <template slot-scope="scope">
            <el-tooltip :content="scope.row.state!==1 ? '点击启用' : '点击禁用'" placement="top">
              <el-switch v-model="scope.row.state" active-color="#13ce66" inactive-color="#ff4949" :active-value="1" :inactive-value="0" @change="setMenuVal(scope.row.id, 'state:'+scope.row.state)" />
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column label="操作" align="center">
          <template slot-scope="scope">
            <permission-button v-if="scope.row.resourceType !== 3" code="System.MenuFunctional.Add" size="mini" type="text" @click="handleAdd(scope.row)" />
            <permission-button code="System.MenuFunctional.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
            <permission-button code="System.MenuFunctional.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </table-collapse>
    <!-- 添加或修改菜单对话框 -->
    <el-dialog ref="dialogAddOrEditMenuForm" v-el-drag-dialog :title="dialogAddOrEditMenuTitle+'模块/菜单'" append-to-body :visible.sync="dialogMenuEditFormVisible" width="700px" top="6vh">
      <el-form ref="addOrEditMenuForm" :model="form" :rules="menuFormRules" label-width="90px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="上级菜单">
              <treeselect v-model="form.parentResourceID" :options="menuOptions" :normalizer="normalizer" :show-count="true" placeholder="选择上级菜单">
                <label slot="option-label" slot-scope="{ node, shouldShowCount, count }">
                  <el-tag v-if="node.raw.resourceType === 1" style="margin:1px" effect="plain">模块 =</el-tag>
                  <el-tag v-else-if="node.raw.resourceType === 2" type="success" style="margin:1px" effect="plain">菜单 =</el-tag>
                  <el-tag v-else type="warning" style="margin:1px" effect="plain">按钮 =</el-tag>
                  {{ node.label }} <span v-if="shouldShowCount">({{ count }})</span>
                </label>
              </treeselect>
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="菜单类型" prop="resourceType">
              <el-radio-group v-model="form.resourceType">
                <el-radio-button :label="1">模块</el-radio-button>
                <el-radio-button :label="2">菜单</el-radio-button>
                <el-radio-button :label="3">按钮</el-radio-button>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="resourceTypeTitle+'名称'" prop="resourceName">
              <el-input v-model="form.resourceName" :placeholder="'请输入'+resourceTypeTitle+'名称'" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="resourceTypeTitle+'编码'" prop="resourceCode">
              <el-input v-model="form.resourceCode" :placeholder="'请输入'+resourceTypeTitle+'编码'" autocomplete="off" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item :label="resourceTypeTitle+'图标'" prop="resourceIcon">
              <el-popover placement="bottom-start" width="660" trigger="click" @show="$refs['iconSelect'].reset()">
                <icon-select ref="iconSelect" @selected="selected" />
                <el-input slot="reference" v-model="form.resourceIcon" placeholder="点击选择图标" readonly>
                  <svg-icon v-if="form.resourceIcon" slot="prefix" :icon-class="form.resourceIcon" class="el-input__icon" style="height: 32px;width: 16px;" />
                  <i v-else slot="prefix" class="el-icon-search el-input__icon" />
                </el-input>
              </el-popover>
            </el-form-item>
          </el-col>
          <el-col v-if="form.resourceType != 3" :span="24">
            <el-form-item :label="form.isExternalLink ? '外部链接' : '路由地址'" prop="pathName">
              <el-input v-model="form.pathName" placeholder="请输入路由地址" autocomplete="off" clearable />
            </el-form-item>
          </el-col>
          <el-col v-if="(form.resourceType === 2 && !form.isExternalLink)" :span="12">
            <el-form-item label="组件地址" prop="path">
              <el-input v-model="form.path" placeholder="请输入组件地址" autocomplete="off" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="排序" prop="sortNO">
              <el-input-number v-model="form.sortNO" :min="1" :max="99999" />
            </el-form-item>
          </el-col>
          <el-col v-if="form.resourceType != 3" :span="12">
            <el-form-item label="是否外链" prop="isExternalLink">
              <el-radio-group v-model="form.isExternalLink">
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col v-if="form.resourceType != 3" :span="12">
            <el-form-item label="显示状态" prop="isShow">
              <el-radio-group v-model="form.isShow">
                <el-radio :label="true">显示</el-radio>
                <el-radio :label="false">隐藏</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="状态" prop="state">
              <el-radio-group v-model="form.state">
                <el-radio :label="1">可用</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col v-if="form.resourceType != 3" :span="12">
            <el-form-item label="缓存状态" prop="noCache">
              <el-radio-group v-model="form.noCache">
                <el-radio :label="true">缓存</el-radio>
                <el-radio :label="false">不缓存</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item :label="resourceTypeTitle+'描述'">
              <el-input v-model="form.description" type="textarea" :rows="1" :placeholder="'请输入'+resourceTypeTitle+'描述'" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancel">取 消</el-button>
        <el-button type="primary" @click="saveAddOrEditMenuForm()">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import Treeselect from '@riophae/vue-treeselect';
import '@riophae/vue-treeselect/dist/vue-treeselect.css';
import elDragDialog from '@/directives/el-drag-dialog';
import IconSelect from '@/components/IconSelect';
import { GetMenuFunctionalList, AllMenuFunctionalTreeOptions, GetMenuFunctional, AddMenuFunctional, UpdateMenuFunctional, DeleteMenuFunctional, SetMenuFunctional } from 'api/system';
import { guidEmpty } from 'utils';
export default {
  name: 'MenuFunctional',
  directives: { elDragDialog },
  components: { Treeselect, IconSelect },
  data() {
    return {
      formModule: {
        resourceName: '',
        state: -1
      },
      columns: [
        { key: 0, label: `页面缓存`, isShow: false }
      ],
      isExpandAll: false,
      menuFunctionalTree: [], // 菜单功能表格数据
      loading: true, // 遮罩层
      refreshTable: true, // 重新渲染表格状态
      showEditSort: false, // 表格排序编辑显示
      showSearch: true,
      dialogAddOrEditMenuTitle: '新增',
      dialogMenuEditFormVisible: false,
      addOrEditMenuForm: {},
      menuOptions: [], // 菜单树选项
      form: {
        parentResourceID: guidEmpty,
        resourceType: 1,
        resourceName: '',
        resourceCode: '',
        resourceIcon: '',
        isExternalLink: false,
        path: '',
        pathName: '',
        sortNO: 1,
        isShow: true,
        state: 1,
        noCache: false,
        description: ''
      },
      menuFormRules: {
        resourceName: [
          { required: true, message: '名称不能为空', trigger: 'blur' }
        ],
        resourceCode: [
          { required: true, message: '编码名称不能为空', trigger: 'blur' }
        ],
        path: [
          { required: true, message: '组件地址不能为空', trigger: 'blur' }
        ],
        pathName: [
          { required: true, message: '路由地址不能为空', trigger: 'blur' }
        ]
      }
    };
  },
  computed: {
    resourceTypeTitle() {
      this.clearValidateForm('addOrEditMenuForm');
      if (this.form.resourceType === 1) {
        return '模块';
      } else if (this.form.resourceType === 2) {
        return '菜单';
      }
      return '功能';
    }
  },
  created() {
    this.getList();
  },
  methods: {
    search() {
      this.getList();
      this.toggleExpandAll(true);
    },
    async getList() {
      this.loading = true;
      const res = await GetMenuFunctionalList(this.formModule);
      this.loading = false;
      if (!res.success) {
        this.$message.error(res.msg);
        return;
      }
      this.menuFunctionalTree = res.data;
      this.loadMenuTree(res.data); // 初始化下拉菜单
    },
    saveAddOrEditMenuForm() {
      this.$refs['addOrEditMenuForm'].validate((valid) => {
        if (valid) {
          const rloading = this.openLoading();
          const saveMenuFunctional = this.dialogAddOrEditTitle === '新增' ? AddMenuFunctional(this.form) : UpdateMenuFunctional(this.form);
          saveMenuFunctional.then(res => {
            if (res.success) {
              rloading.close();
              this.$message({
                message: this.dialogAddOrEditMenuTitle + '成功',
                type: 'success'
              });
              this.cancel();
              this.getList();
            } else {
              this.$message.error(res.msg);
              rloading.close();
            }
          });
        }
      });
    },
    // 取消按钮
    cancel() {
      this.dialogMenuEditFormVisible = false;
      this.reset();
    },
    // 表单重置
    reset() {
      this.form = {
        parentResourceID: guidEmpty,
        resourceType: 1,
        resourceName: '',
        resourceIcon: '',
        isExternalLink: false,
        path: '',
        pathName: '',
        sortNO: 1,
        isShow: true,
        state: 1,
        noCache: false,
        description: ''
      };
      this.resetForm('addOrEditMenuForm');
    },
    loadMenuTree() {
      AllMenuFunctionalTreeOptions().then(res => {
        this.menuOptions = [];
        const menu = { id: guidEmpty, label: '根菜单', resourceType: 1, children: [] };
        this.deptTreeOptions = res.data;
        menu.children = res.data;
        this.menuOptions.push(menu);
      });
    },
    // 选择图标
    selected(name) {
      this.form.resourceIcon = name;
    },
    normalizer(node) {
      if (node.children === null || !node.children.length) {
        delete node.children;
      }
      let isDisabled = false;
      if (node.resourceType === 3 || node.isExternalLink) {
        isDisabled = true;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children,
        isDisabled: isDisabled
      };
    },
    async setMenuVal(id, fieldVal) {
      const res = await SetMenuFunctional({ id, fieldVal });
      if (!res.success) {
        this.$message.error(res.msg);
        return;
      }
      this.getList();
      this.msgSuccess('修改成功!');
    },
    handleAdd(row) {
      if (row.resourceType !== 3) {
        this.form.resourceType = row.resourceType + 1;
      }
      this.form.parentResourceID = row.id;
      this.showDialogAddOrEditMenu('add');
    },
    async handleUpdate(row) {
      const res = await GetMenuFunctional({ id: row.id });
      if (!res.success) {
        this.$message.error(res.msg);
        return;
      }
      this.form = res.data;
      this.showDialogAddOrEditMenu('edit');
    },
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.resourceName}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteMenuFunctional({ id: row.id });
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('删除成功!');
        this.getList();
      });
    },
    showDialogAddOrEditMenu(optType) {
      if (optType === 'add') {
        this.dialogAddOrEditMenuTitle = '新增';
      } else {
        this.dialogAddOrEditMenuTitle = '修改';
      }
      this.dialogMenuEditFormVisible = true;
    },
    toggleExpandAll(isExpandAll) {
      this.refreshTable = false;
      this.isExpandAll = isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    }
  }
};
</script>

<style lang="scss" scoped></style>
