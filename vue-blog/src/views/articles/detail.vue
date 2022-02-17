<template>
  <div
    class="common"
    :style="{
      background: `url(${bgImg}) center center no-repeat`,
      backgroundSize: 'cover',
    }"
  >
    <div v-if="isPC" class="toc-fixed">
      <mu-card class="card">
        <div class="toc">
          <div class="title">文章目录</div>
          <div id="directory_tree">
          </div>
        </div>
      </mu-card>
      <div class="action" :class="toc.length > 0 ? '' : 'noMulu'">
        <mu-tooltip placement="top" content="点赞">
          <mu-button fab color="primary">
            <mu-icon value=":iconfont icon-zantongfill"></mu-icon>
          </mu-button>
        </mu-tooltip>

        <mu-tooltip placement="top" content="评论">
          <mu-button fab color="red" @click="scrollToPosition('#comment')">
            <mu-icon value=":iconfont icon-pinglun"></mu-icon>
          </mu-button>
        </mu-tooltip>
      </div>
    </div>

    <div class="content">
      <div class="left">
        <div class="left-box" :style="{ width: isPC ? '80%' : '100%' }">
          <mu-card class="card">
            <mu-card-title :title="articleDetail.articleTitle"></mu-card-title>
            <mu-card-media style="height: auto">
              <img v-lazy="articleDetail.coverImage" style="height: 6rem;object-fit: cover;">
            </mu-card-media>
            <mu-card-actions class="sub-title">
              <mu-button class="cursor-default" flat color="warning">字数({{ markdownConvertHtml.length }})</mu-button>
              <mu-button class="cursor-default" flat color="info">查看({{ articleDetail.viewNum }})</mu-button>
              <mu-button class="cursor-default" flat color="error">评论({{ articleDetail.commentNum }})</mu-button>
              <mu-button class="cursor-default" flat color="primary">点赞({{ articleDetail.likeNum }})</mu-button>
              <mu-button class="cursor-default" flat color="#9e9e9e">{{ articleDetail.createTime }}</mu-button>
            </mu-card-actions>

            <mavon-editor
              v-show="false"
              v-model="articleDetail.articleContent"
              :toolbars-flag="false"
              :subfield="false"
              default-open="preview"
              :code-style="codeStyle"
              :external-link="externalLink"
              :navigation="isPC"
            />

            <mu-paper id="blog_content" ref="blogContent" style="padding:16px;" v-html="markdownConvertHtml"></mu-paper>

            <mu-card-actions style="border-top: 1px solid #3c8b65;">
              <mu-button class="cursor-default" flat color="primary">
                <mu-icon left value=":iconfont icon-category"></mu-icon>
                {{ articleDetail.categoryTitle }}
              </mu-button>

              <mu-button v-for="tag in articleDetail.tags" :key="tag.value" class="cursor-default" flat>
                <mu-icon left value=":iconfont icon-tag"></mu-icon>
                {{ tag.label }}
              </mu-button>
              <prev-next :prev="articleDetail.prev" :next="articleDetail.next"></prev-next>

            </mu-card-actions>

          </mu-card>

          <div v-if="!isPC" class="action-list">
            <mu-tooltip placement="top" content="点赞">
              <mu-button fab color="primary">
                <mu-icon value=":iconfont icon-zantongfill"></mu-icon>
              </mu-button>
            </mu-tooltip>
          </div>

          <mu-card id="comment" class="card">
            <comment @onSubComment="onSubComment"></comment>
          </mu-card>

          <mu-card v-if="commentList.length > 0" class="card">
            <mu-card-title title="评论（3）"></mu-card-title>
            <mu-divider></mu-divider>
            <comment-list
              v-if="commentList.length > 0"
              style="padding: 0.3rem 0;"
              :article-id="articleDetail.id"
              :list="commentList"
              @onSubComment="onSubComment"
              @onLikeComment="onLikeComment"
              @onDeleteComment="onDeleteComment"
            ></comment-list>
          </mu-card>
        </div>

        <blog-footer style="padding:20px 0;"></blog-footer>

      </div>
    </div>

  </div>
</template>
<script>
import { GetArticleDetail, GetArticleCommentList, SubComment, LikeComment, DeleteComment } from 'api';
import blogFooter from 'components/blogFooter.vue';
import Clipboard from 'clipboard';
import { mavonEditor } from 'mavon-editor';
import 'mavon-editor/dist/css/index.css';
import { markdown } from 'utils/markdown';
import $ from 'jquery';
import comment from 'components/comment';
import commentList from 'components/commentList';
import prevNext from 'components/prevNext';

export default {
  name: 'ArticlesDetails',
  components: {
    blogFooter,
    mavonEditor,
    comment,
    commentList,
    prevNext
  },
  data() {
    return {
      bgImg: '/images/about.png',
      delOpen: false,
      toc: [],
      commentSuccess: false,
      codeStyle: 'vs2015',
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
      },
      commentList: [], // 评论列表数据
      articleDetail: {
        id: '',
        userName: '',
        articleTitle: '',
        introduction: '',
        articleContent: '',
        coverImage: '',
        categoryTitle: '',
        categoryId: '',
        tags: [],
        viewNum: 0,
        likeNum: 0,
        commentNum: 0,
        auth: 0,
        isDisableComment: false,
        createTime: '',
        prev: { id: '', title: '' },
        next: { id: '', title: '' }
      },
      articleId: ''
    };
  },
  computed: {
    // 获取markdown渲染后的html
    markdownConvertHtml() {
      let res = this.articleDetail.articleContent ? markdown(
        mavonEditor,
        this.articleDetail.articleContent
      ) : '';
      this.$nextTick(() => {
        this.loadTree();
      });
      return res;
    }
  },
  created() {
    $('#blog_content').empty(); // 页面残留问题，清空之前Markdown渲染的dom
    this.articleId = this.$route.query.id;
    if (!this.articleId) {
      return this.$toast.error('页面参数错误');
    }
    this.getArticleDetail();
  },
  mounted() {
    this.getArticleCommentList();
    this.$nextTick(() => {
      let clipboard = new Clipboard('.copy-btn');
      // 复制成功失败的提示
      clipboard.on('success', () => {
        this.$toast.success('复制成功');
      });
      clipboard.on('error', () => {
        this.$toast.error('复制失败');
      });
    });
  },
  methods: {
    getArticleDetail() {
      this.$progress.start();
      GetArticleDetail({ id: this.articleId }).then(res => {
        if (res.statusCode < 1) {
          this.$progress.done();
          return this.$toast.error(res.msg);
        }
        this.articleDetail = res.data;
      });
    },
    getArticleCommentList() {
      GetArticleCommentList({ articleId: this.articleId }).then(res => {
        if (res.statusCode < 1) {
          return this.$toast.error(res.msg);
        }
        this.commentList = res.data;
        this.commentList = this.listToTree(this.commentList);
      });
    },
    onSubComment(data, onSuccess) {
      data.articleId = this.articleId;
      SubComment(data).then(res => {
        if (res.success) {
          this.$toast.success('提交成功!');
          onSuccess();
          this.getArticleCommentList();
        } else {
          this.$toast.error(res.msg);
        }
      }).catch(err => {
        this.$toast.error(err.msg);
      });
    },
    onLikeComment(commentId) {
      LikeComment({ commentId }).then(res => {
        if (res.success) {
          this.$toast.success('提交评论成功!');
          this.getArticleCommentList();
        } else {
          this.$toast.error(res.msg);
        }
      }).catch(err => {
        this.$toast.error(err.msg);
      });
    },
    onDeleteComment(commentId) {
      if (confirm('是否确认删除该评论')) {
        DeleteComment({ commentId }).then(res => {
          if (res.success) {
            this.$toast.success('删除评论成功!');
            this.getArticleCommentList();
          } else {
            this.$toast.error(res.msg);
          }
        }).catch(err => {
          this.$toast.error(err.msg);
        });
      }
    },
    listToTree(list) {
      var comments = list.filter(f => f.commentType === 0); // 评论
      comments.forEach(comment => {
        comment.replys = list.filter(f => f.commentRootId === comment.id);
        comment.replys.forEach(reply => {
          const parentReply = comment.replys.find(f => f.id === reply.parentCommentId);
          reply.replyNickName = (parentReply && parentReply.commentNickName) || comment.commentNickName;
        });
      });
      return comments;
    },
    scrollToPosition(id) {
      var position = $(id).offset();
      position.top = position.top - 80;
      $('.content').animate({ scrollTop: position.top }, 1000);
    },
    async comment(data, onSuccess) {
      console.log('评论数据', data);
      onSuccess();
    },
    loadTree() {
      let that = this;
      let headers = $('#blog_content :header');
      headers.each(function(index, el) {
        el.setAttribute('id', 'f' + index);
        that.toc.push({
          id: 'header-' + index,
          title: $(el).text(),
          href: 'f' + index,
          level: Number(el.nodeName[1]),
          nodeName: el.nodeName
        });
      });
      let ul = $('<ul></ul>');
      that.toc.forEach(function(data, index) {
        let li = $('<li></li>').css('padding-left', data.level * 22 - 22 + 'px');
        li.click(function() {
          var position = $('#' + data.href).offset();
          $('.content').animate({ scrollTop: position.top }, 1000);
        });
        let a = $('<a />').attr({ 'id': data.id, 'href': '#' + data.href }).text(data.title);
        li.append(a);
        ul.append(li);
      });
      $('#directory_tree').append(ul);
      $('.content').scroll(function() {
        // 为页面添加页面滚动监听事件
        var wst = $('.content').scrollTop(); // 滚动条距离顶端值
        for (let i = 0; i < that.toc.length; i++) { // 加循环
          if ($('#f' + i).offset().top <= wst) { // 判断滚动条位置
            $('#directory_tree a').removeClass('tree-onscroll');
            $('#header-' + i).addClass('tree-onscroll'); // 给当前导航加c类
          }
        }
      });
    }
  }
};
</script>
<style lang="less" scoped>
  .content {
    position: absolute;
    top: 64px;
    bottom: 0;
    overflow: auto;
    width: 100%;
    padding-top: 16px;
    color: #fff;
  }
  .toc-fixed {
    position: fixed;
    width: 20%;
    top: 80px;
    right: 0;
    padding-right: 16px;
    margin-right: 8px;
    z-index: 1502;
    .toc {
      width: 100%;
      max-height: 400px;
      overflow-y: auto;
      word-break: break-all;
      padding: 0.2rem 0 0.2rem 0.2rem;
      .title {
        font-size: 0.4rem;
        margin-bottom: 10px;
      }
    }
  }
  .action-list {
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 0.42667rem 0;
    & > button {
      margin: 0 20px;
    }
  }
  .action {
    margin-top: 0.42667rem;
    display: flex;
    justify-content: space-around;
  }
  .noMulu {
    flex-direction: column;
    align-items: center;
    height: 200px;
  }
  .content {
    padding-bottom: 0.53333rem;
    display: flex;
    .left {
      flex: 9;
      margin: 0;
      .left-box {
        padding: 0 16px;
      }
      .card {
        border-radius: 5px;
        margin-bottom: 0.48rem;
        .article-detail {
          width: 100%;
          padding: 0.42667rem 0.42667rem 0.42667rem 0.69333rem;
          box-sizing: border-box;
          word-break: break-all;
        }
        .sub-title {
          display: flex;
          flex-wrap: wrap;
        }
        .text {
          padding: 0 0.42667rem;
          overflow: hidden;
          text-overflow: ellipsis;
          display: -webkit-box;
          -webkit-box-orient: vertical;
          -webkit-line-clamp: 3;
        }
        .chip {
          margin-right: 0.26667rem;
        }
        .cover {
          flex: 1;
          border-radius: 0;
          padding: 0.42667rem;
          .cover-img {
            object-fit: cover;
            width: 100%;
            height: 4.26667rem;
            vertical-align: middle;
          }
        }
        .card-box {
          flex: 2;
          display: flex;
          flex-direction: column;
          justify-content: space-around;
        }
      }
    }
    .right {
      flex: 3;
      display: flex;
      justify-content: center;
    }
  }
</style>
