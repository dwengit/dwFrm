<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="分类名称">
          <el-input v-model="queryForm.categoryTitle" clearable />
        </el-form-item>
        <el-form-item>
          <permission-button code="Blog.Category.Read" @click="search()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="Blog.Category.Add" @click.native="handleAdd()" />
            <permission-button code="Blog.Category.Read" type="info" plain icon="tree" title="展开/折叠" @click="toggleExpandAll(!isExpandAll)" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" @queryTable="getList" />
      </el-row>

      <el-table v-if="refreshTable" v-loading="loading" :data="pageTreeData" :default-expand-all="isExpandAll" row-key="id" border :tree-props="{ children: 'children', hasChildren: 'hasChildren' }">
        <el-table-column prop="categoryTitle" label="分类名称" align="center" show-overflow-tooltip />
        <el-table-column prop="sortNO" label="排序" align="center" />
        <el-table-column prop="createTime" label="创建日期" align="center" />
        <el-table-column label="操作" align="center" show-overflow-tooltip width="260">
          <template slot-scope="scope">
            <permission-button code="Blog.Category.Add" size="mini" type="text" @click="handleAdd(scope.row.id)" />
            <permission-button code="Blog.Category.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
            <permission-button v-if="scope.row.level !== 1" code="Blog.Category.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </table-collapse>

    <!-- 新增，修改 -->
    <el-dialog ref="dialogAddOrEditForm" v-el-drag-dialog :title="dialogAddOrEditTitle+'分类'" append-to-body :visible.sync="dialogAddOrEditFormVisible" width="700px" top="6vh">
      <el-form ref="addOrEditForm" :model="saveForm" :rules="saveFormRules" label-width="90px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="上级分类" prop="parentId">
              <category-tree v-model="saveForm.parentId" :is-create-root-id="true" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="分类名称" prop="categoryTitle">
              <el-input v-model="saveForm.categoryTitle" placeholder="请输入分类名称" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="显示排序" prop="sortNO">
              <el-input-number v-model="saveForm.sortNO" :min="1" :max="99999" />
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
import { guidEmpty } from 'utils';
import elDragDialog from '@/directives/el-drag-dialog';
import categoryTree from '../category/components/categoryTree.vue';
import { GetCategoryPageTree, AddCategory, UpdateCategory, DeleteCategory } from 'api/blog';
export default {
  name: 'Category',
  directives: { elDragDialog },
  components: { categoryTree },
  data() {
    return {
      refreshTable: true,
      isExpandAll: false,
      showSearch: true,
      loading: false,
      queryForm: {
        categoryTitle: '',
        status: -1
      },
      pageTreeData: [],
      dialogAddOrEditTitle: '新增',
      dialogAddOrEditFormVisible: false,
      saveForm: {
        parentId: null,
        categoryTitle: '',
        sortNO: 1
      },
      saveFormRules: {
        categoryTitle: [
          { required: true, message: '分类名称不能为空', trigger: 'blur' }
        ],
        parentId: [
          { required: true, message: '请选择上级分类', trigger: 'change' }
        ],
        sortNO: [
          { required: true, message: '请输入排序', trigger: 'change' }
        ]
      },
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
      GetCategoryPageTree(this.queryForm).then(res => {
        this.loading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.pageTreeData = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
        this.loading = false;
      });
    },
    handleAdd(id) {
      this.saveForm.parentId = id || guidEmpty;
      this.showDialogAddOrEdit('add');
    },
    handleUpdate(row) {
      this.saveForm = JSON.parse(JSON.stringify(row));
      this.showDialogAddOrEdit('edit');
    },
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.categoryTitle}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteCategory({ id: row.id });
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
          const saveCategory = this.dialogAddOrEditTitle === '新增' ? AddCategory(this.saveForm) : UpdateCategory(this.saveForm);
          saveCategory.then(res => {
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
        categoryTitle: '',
        sortNO: 1
      };
      this.resetForm('addOrEditForm');
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
