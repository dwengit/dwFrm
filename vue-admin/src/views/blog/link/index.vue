<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="友情链接名称">
          <el-input v-model="queryForm.linkTitle" clearable />
        </el-form-item>
        <el-form-item>
          <permission-button code="Blog.Link.Read" @click="search()" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="Blog.Link.Add" @click.native="showDialogAddOrEdit('add')" />
          </el-button-group>
        </el-col>
        <right-toolbar :show-search.sync="showSearch" @queryTable="getList" />
      </el-row>

      <el-table v-if="refreshTable" v-loading="loading" :data="pageData" row-key="id" border>
        <el-table-column prop="linkTitle" label="友情链接名称" align="center" show-overflow-tooltip />
        <el-table-column label="背景图" align="center">
          <template slot-scope="scope">
            <el-image style="width: 100px; height: 100px" :src="scope.row.linkImage.length ? scope.row.linkImage[0].url : ''" />
          </template>
        </el-table-column>
        <el-table-column prop="linkUrl" label="链接地址" align="center" />
        <el-table-column prop="sortNO" label="排序" align="center" />
        <el-table-column prop="createTime" label="创建日期" align="center" />
        <el-table-column label="操作" align="center" show-overflow-tooltip width="260">
          <template slot-scope="scope">
            <permission-button code="Blog.Link.Edit" size="mini" type="text" @click="handleUpdate(scope.row)" />
            <permission-button v-if="scope.row.level !== 1" code="Blog.Link.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </table-collapse>

    <!-- 新增，修改 -->
    <el-dialog ref="dialogAddOrEditForm" v-el-drag-dialog :title="dialogAddOrEditTitle+'友情链接'" append-to-body :visible.sync="dialogAddOrEditFormVisible" width="700px" top="6vh" center>
      <el-form ref="addOrEditForm" :model="saveForm" :rules="saveFormRules" label-width="120px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="友情链接名称" prop="linkTitle">
              <el-input v-model="saveForm.linkTitle" placeholder="请输入友情链接名称" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="友情链接地址" prop="linkUrl">
              <el-input v-model="saveForm.linkUrl" placeholder="请输入友情链接地址" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="显示排序" prop="sortNO">
              <el-input-number v-model="saveForm.sortNO" :min="1" :max="99999" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="封面图片" prop="coverImage">
              <upload-busines v-model="saveForm.coverImage" :file-max-size="1" :parameter="{ businessCode: 'blog.link.backImg', businessId: saveForm.id }" list-type="picture" :limit="1" />
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
import { GetLinkPage, AddLink, UpdateLink, DeleteLink } from 'api/blog';
import UploadBusines from 'components/Upload/UploadBusines';
import { getGuid } from 'utils';
export default {
  name: 'Link',
  directives: { elDragDialog },
  components: { UploadBusines },
  data() {
    return {
      refreshTable: true,
      showSearch: true,
      loading: false,
      queryForm: {
        linkTitle: '',
        status: -1
      },
      pageData: [],
      dialogAddOrEditTitle: '新增',
      dialogAddOrEditFormVisible: false,
      saveForm: {
        linkImage: [],
        linkTitle: '',
        linkUrl: '',
        sortNO: 1,
        id: ''
      },
      saveFormRules: {
        linkTitle: [
          { required: true, message: '友情链接名称不能为空', trigger: 'blur' }
        ],
        linkUrl: [
          { required: true, message: '友情链接名称不能为空', trigger: 'blur' }
        ],
        sortNO: [
          { required: true, message: '请输入排序', trigger: 'change' }
        ]
      },
      linkTreeOptions: [],
      addOrEditForm: []
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
      GetLinkPage(this.queryForm).then(res => {
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
    handleAdd() {
      this.showDialogAddOrEdit('add');
    },
    handleUpdate(row) {
      this.saveForm = JSON.parse(JSON.stringify(row));
      this.showDialogAddOrEdit('edit');
    },
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.linkTitle}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteLink({ id: row.id });
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
          const saveLink = this.dialogAddOrEditTitle === '新增' ? AddLink(this.saveForm) : UpdateLink(this.saveForm);
          saveLink.then(res => {
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
        this.saveForm.id = getGuid();
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
        linkImage: [],
        linkTitle: '',
        linkUrl: '',
        sortNO: 1
      };
      this.resetForm('addOrEditForm');
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
