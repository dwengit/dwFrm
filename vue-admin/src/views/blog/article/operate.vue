<template>
  <div class="container-editor">
    <el-form ref="saveForm" :model="saveForm" :rules="saveFormRules" label-width="90px">
      <el-row>
        <el-col :span="24">
          <el-form-item label="标题" prop="articleTitle">
            <el-input v-model="saveForm.articleTitle" type="textarea" :rows="3" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="24">
          <el-form-item label="摘要" prop="introduction">
            <el-input v-model="saveForm.introduction" maxlength="256" type="textarea" :rows="3" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="24">
          <el-form-item label="内容" prop="articleTitle">
            <div v-show="!disabled" class="mavonEditor">
              <mavon-editor ref="md" v-model="saveForm.articleContent" :code-style="codeStyle" :external-link="externalLink" :ishljs="true" hightlight @imgAdd="$imgAdd" @imgDel="$imgDel" />
            </div>
            <div v-show="disabled" v-html="markdownConvertHtml" />
          </el-form-item>
        </el-col>
        <el-col :span="24">
          <el-form-item label="封面图片" prop="coverImage">
            <upload-busines v-model="saveForm.coverImage" :is-read-only="disabled" :parameter="{ businessCode: 'article.coverImage', businessId: saveForm.id }" list-type="picture" :limit="1" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="分类" prop="categoryId">
            <category-tree v-model="saveForm.categoryId" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="标签" prop="tagIds">
            <el-select
              v-model="saveForm.tagIds"
              multiple
              filterable
              placeholder="请选择文章标签"
              style="width:100%"
              :disabled="disabled"
            >
              <el-option
                v-for="item in tagOption"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="浏览量" prop="viewNum">
            <el-input-number v-model="saveForm.viewNum" :min="0" :max="9999" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="点赞数" prop="likeNum">
            <el-input-number v-model="saveForm.likeNum" :min="0" :max="9999" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="置顶" prop="top">
            <el-switch v-model="saveForm.top" active-text="是" inactive-text="否" :active-value="1" :inactive-value="0" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="权限" prop="auth">
            <el-switch v-model="saveForm.auth" active-text="VIP" inactive-text="公开" :active-value="1" :inactive-value="0" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="状态" prop="articleStatus">
            <el-switch v-model="saveForm.articleStatus" active-text="发布" inactive-text="草稿" :active-value="1" :inactive-value="0" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="开启评论" prop="isDisableComment">
            <el-switch v-model="saveForm.isDisableComment" active-text="是" inactive-text="否" :disabled="disabled" />
          </el-form-item>
        </el-col>
        <el-col :span="24">
          <div class="sub-pad">
            <el-divider />
            <el-button @click="returnPage">返回</el-button>
            <el-button type="primary" @click="submitData">提交</el-button>
          </div>
        </el-col>
      </el-row>
    </el-form>

  </div>
</template>

<script>
import { mavonEditor } from 'mavon-editor';
import categoryTree from '../category/components/categoryTree.vue';
import { GetTagOption, AddArticle, UpdateArticle, GetGetArticleInfo } from 'api/blog';
import UploadBusines from 'components/Upload/UploadBusines';
import { PostFile, RemoveUploadFile } from 'api/upload';
import { getGuid } from 'utils';
import { markdown } from 'utils/markdown';
import 'highlight.js/styles/vs2015.css';
import Clipboard from 'clipboard';

export default {
  name: 'ArticleOperate',
  components: { categoryTree, UploadBusines },
  data() {
    return {
      saveForm: {
        id: getGuid(),
        articleTitle: '',
        introduction: '',
        articleContent: '',
        categoryId: null,
        tagIds: [],
        articleStatus: 0,
        top: 0,
        auth: 1,
        isDisableComment: false,
        viewNum: 0,
        likeNum: 0,
        coverImage: []
      },
      saveFormRules: {
        articleTitle: [
          { required: true, message: '博文标题不能为空', trigger: 'blur' }
        ],
        articleContent: [
          { required: true, message: '博文内容不能为空', trigger: 'blur' }
        ]
      },
      tagOption: [],
      content_img: {},
      disabled: false,
      codeStyle: 'vs-2015',
      // 需要配置的内容：
      externalLink: {
        markdown_css: function() {
          // 这是你的markdown css文件路径
          return '/mavon-editor/markdown/github-markdown.min.css';
        },
        hljs_js: function() {
          // 这是你的hljs文件路径
          return '/mavon-editor/highlightjs/highlight.min.js';
        },
        hljs_css: function(css) {
          if (!css) return '';
          // 这是你的代码高亮配色文件路径
          return '/mavon-editor/highlightjs/styles/' + css + '.min.css';
        },
        hljs_lang: function(lang) {
          if (!lang) return '';
          // 这是你的代码高亮语言解析路径
          return '/mavon-editor/highlightjs/languages/' + lang + '.min.js';
        },
        katex_css: function() {
          // 这是你的katex配色方案路径路径
          return '/mavon-editor/katex/katex.min.css';
        },
        katex_js: function() {
          // 这是你的katex.js路径
          return '/mavon-editor/katex/katex.min.js';
        }
      }
    };
  },
  computed: {
    // 获取markdown渲染后的html
    markdownConvertHtml() {
      return this.saveForm.articleContent ? markdown(
        mavonEditor,
        this.saveForm.articleContent
      ) : '';
    }
  },
  mounted() {
    this.codeStyle = 'vs-2015';
    this.$nextTick(() => {
      this.clipboard = new Clipboard('.copy-btn');
      this.clipboard.on('success', () => {
        this.msgSuccess('复制成功');
        this.clipboard.destroy();
      });
      this.clipboard.on('error', () => {
        this.$message.error('复制失败');
        this.clipboard.destroy();
      });
    });
  },
  destroyed() {
    this.clipboard.destroy();
  },
  created() {
    this.getTagOption();
    this.optType = 'add';
    if (this.$route.path.indexOf('/info') > -1) {
      this.disabled = true;
      this.optType = 'info';
    } else if (this.$route.path.indexOf('/edit') > -1) {
      this.optType = 'edit';
    }
    if (this.optType !== 'add') {
      this.getInfo();
    }
  },
  methods: {
    getInfo() {
      const rloading = this.openLoading();
      GetGetArticleInfo({ id: this.$route.params.id }).then(res => {
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.saveForm = res.data;
      }).catch(err => {
        rloading.close();
        this.$message.error(err.msg);
      });
    },
    submitData() {
      this.$refs['saveForm'].validate((valid) => {
        if (valid) {
          const rloading = this.openLoading();
          const optTitle = this.optType === 'add' ? '新增' : '修改';
          const saveDept = this.optType === 'add' ? AddArticle(this.saveForm) : UpdateArticle(this.saveForm);
          saveDept.then(res => {
            if (res.success) {
              rloading.close();
              this.msgSuccess(optTitle + '成功');
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
    getTagOption() {
      GetTagOption().then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.tagOption = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    $imgAdd(pos, $file) {
      const formData = new FormData();
      formData.append('UpFile', $file);
      formData.append('BusinessCode', 'article.contentImage');
      formData.append('BusinessId', this.saveForm.id);
      PostFile(formData).then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('上传成功');
        this.$refs.md.$img2Url(pos, res.data.url);
        this.content_img[res.data.url] = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    async $imgDel(pos) {
      const file = this.content_img[pos[0]];
      const res = await RemoveUploadFile({ id: file.id });
      if (!res.success) {
        this.$message.error(res.msg);
        return;
      }
      this.$refs.md.$refs.toolbar_left.$imgDelByFilename(pos[0]);
      delete this.content_img[pos[0]];
      this.msgSuccess('删除成功!');
    },
    returnPage() {
      // 调用全局挂载的方法
      this.$store.dispatch('tagsView/delView', this.$route);
      // 返回上一步路由
      this.$router.go(-1);
    }
  }
};
</script>

<style lang="scss" scoped>
.container-editor{
  padding: 10px;
}
.mavonEditor {
  width: 100%;
  height: 100%;
}
.sub-pad{
   text-align: center;
    // position: fixed;
    bottom: 0;
    z-index: 9999;
    width: 100%;
    background-color: #f3f3f4;
    padding: 10px;
}
</style>
