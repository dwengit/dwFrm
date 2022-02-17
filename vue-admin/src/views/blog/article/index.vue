<template>
  <div class="app-container">
    <collapse ref="collapse" :show="showSearch">
      <el-form :inline="true" :model="queryForm" class="demo-form-inline">
        <el-form-item label="博文标题">
          <el-input v-model="queryForm.articleTitle" clearable />
        </el-form-item>
        <el-form-item label="博文状态">
          <el-select v-model="queryForm.articleStatus">
            <el-option label="所有" :value="-1" />
            <el-option label="草稿" :value="0" />
            <el-option label="已发布" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item label="标签">
          <el-select
            v-model="queryForm.tagIds"
            v-loading="loadingTag"
            multiple
            filterable
            placeholder="请选择文章标签"
          >
            <el-option
              v-for="item in tagOption"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="分类">
          <category-tree v-model="queryForm.categoryId" style="width:300px;" />
        </el-form-item>
        <el-form-item label="注册日期">
          <el-date-picker v-model="datePickerVal" type="datetimerange" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="['00:00:00']" />
        </el-form-item>
        <el-form-item>
          <permission-button code="Blog.Article.Read" @click="search" />
        </el-form-item>
      </el-form>
    </collapse>
    <table-collapse>
      <el-row>
        <el-col :span="20">
          <el-button-group>
            <permission-button code="Blog.Article.Add" @click.native="toOperate('add')" />
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
        :operate-width="185"
        @pageOrPageSizeChange="pageOrPageSizeChange"
        @selection-change="handleSelectionChange"
      >
        <template slot="articleTitle" slot-scope="scope">
          {{ scope.row.articleTitle }}
          <el-tag v-if="scope.row.top === 1" type="danger" effect="plain">置顶</el-tag>
        </template>
        <template slot="coverImage" slot-scope="scope">
          <el-image style="width: 100px; height: 100px" :src="scope.row.coverImage" :preview-src-list="[scope.row.coverImage]" />
        </template>
        <template slot="auth" slot-scope="scope">
          <el-tag v-if="scope.row.auth === 0" effect="plain">公开</el-tag>
          <el-tag v-else type="danger" effect="plain">VIP</el-tag>
        </template>
        <template slot="articleStatus" slot-scope="scope">
          <el-tag v-if="scope.row.articleStatus === 0" effect="plain">草稿</el-tag>
          <el-tag v-else type="success" effect="plain">已发布</el-tag>
        </template>
        <template slot="operateBox" slot-scope="scope">
          <permission-button code="Blog.Comment.Read" size="mini" type="text" @click="toOperate('comment', scope.row.id)" /><span style="color: #ff595e;margin-right: 5px;">({{ scope.row.commentNum }})</span>
          <permission-button code="Blog.Article.Read" size="mini" icon="eye-open" title="详情" type="text" @click="toOperate('info', scope.row.id)" />
          <permission-button code="Blog.Article.Edit" size="mini" type="text" @click="toOperate('edit', scope.row.id)" />
          <permission-button v-if="scope.row.roleCode !== 'admin'" code="Blog.Article.Delete" size="mini" type="text" @click="handleDelete(scope.row)" />
          <permission-button v-if="scope.row.roleCode !== 'admin'" code="Blog.Article.AssignPermission" size="mini" type="text" @click="handleAssignPermission(scope.row)" />
        </template>
      </fixed-thead-table>
    </table-collapse>

  </div>
</template>

<script>
import { GetGetArticlePage, GetTagOption, DeleteArticle } from 'api/blog';
import categoryTree from '../category/components/categoryTree.vue';

export default {
  name: 'Article',
  directives: { },
  components: { categoryTree },
  data() {
    return {
      showSearch: true,
      loading: false,
      loadingTag: false,
      pageData: {},
      queryForm: {
        articleTitle: '',
        categoryId: null,
        tagIds: [],
        status: -1,
        beginTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10
      },
      tagOption: [],
      datePickerVal: [],
      dialogAddOrEditTitle: '新增',
      tableColumn: [
        { label: '博文标题', prop: 'articleTitle', isShow: true },
        { label: '摘要', prop: 'introduction', isShow: false },
        { label: '编辑人', prop: 'userName', width: 120, align: 'center', isShow: true },
        { label: '封面图片', prop: 'coverImage', width: 200, isShow: true },
        { label: '分类名称', prop: 'categoryTitle', width: 100, align: 'center', isShow: true },
        { label: '云标签', prop: 'tagTitles', width: 150, align: 'center', isShow: true },
        { label: '浏览量', prop: 'viewNum', width: 80, align: 'center', isShow: false },
        { label: '点赞数', prop: 'likeNum', width: 80, align: 'center', isShow: false },
        { label: '权限', prop: 'auth', width: 80, align: 'center', isShow: true },
        { label: '博文状态', prop: 'articleStatus', width: 80, align: 'center', isShow: true },
        { label: '创建时间', prop: 'createTime', align: 'center', width: 125, isShow: true }
      ],
      assignMenuFunctionalTreeVisible: false
    };
  },
  created() {
    this.search();
  },
  methods: {
    search() {
      this.getTagOption();
      this.getList();
    },
    getTagOption() {
      this.loadingTag = true;
      GetTagOption().then(res => {
        this.loadingTag = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.tagOption = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
        this.loadingTag = false;
      });
    },
    getList() {
      this.queryForm.beginTime = '';
      this.queryForm.endTime = '';
      if (this.datePickerVal && this.datePickerVal.length === 2) {
        this.queryForm.beginTime = this.datePickerVal[0];
        this.queryForm.endTime = this.datePickerVal[1];
      }
      this.loading = true;
      const reqData = JSON.parse(JSON.stringify(this.queryForm));
      reqData.tagIds = reqData.tagIds.toString();
      GetGetArticlePage(reqData).then(res => {
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
    handleDelete(row) {
      this.$confirm(`是否确认删除名称为"${row.articleTitle}"的数据项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteArticle({ id: row.id });
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('删除成功!');
        this.getList();
      });
    },
    toOperate(optType, id) {
      // 跳转到新增/编辑
      if (optType === 'add') { this.$router.push('/blog/article/add'); } else if (optType === 'comment') { this.$router.push(`/blog/comment/index/${id}`); } else { this.$router.push(`/blog/article/${optType}/${id}`); }
    },
    cancelForm() {
      this.dialogAddOrEditFormVisible = false;
      this.reset();
    },
    pageOrPageSizeChange(val) {
      this.queryForm.pageIndex = val.page;
      this.queryForm.pageSize = val.pageSize;
      this.getList();
    },
    handleSelectionChange(val) {
      console.log(val);
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
