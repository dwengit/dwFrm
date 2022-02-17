<template>
  <div>
    <comment-list :value="commentList" @onSubComment="onSubComment" @onLikeComment="onLikeComment" @onDelete="onDelete" />
    <div style="text-align: center;margin-bottom: 20px;">
      <el-divider />
      <el-button @click="returnPage">返回</el-button>
    </div>
  </div>
</template>

<script>
import commentList from './components/comment-list';
import { GetArticleCommentList, SubComment, LikeComment, DeleteComment } from 'api/blog';
export default {
  name: 'Comment',
  components: { commentList },
  data() {
    return {
      commentList: []
    };
  },
  created() {
    this.articleId = this.$route.params.id;
    this.getArticleCommentList();
  },
  methods: {
    getArticleCommentList() {
      const rloading = this.openLoading();
      GetArticleCommentList({ articleId: this.articleId }).then(res => {
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        var comments = res.data.filter(f => f.commentType === 0); // 评论
        comments.forEach(comment => {
          comment.replys = res.data.filter(f => f.commentRootId === comment.id);
          comment.replys.forEach(reply => {
            const parentReply = comment.replys.find(f => f.id === reply.parentCommentId);
            reply.replyNickName = (parentReply && parentReply.commentNickName) || comment.commentNickName;
          });
        });
        this.commentList = comments;
        console.log(this.commentList);
      }).catch(err => {
        rloading.close();
        this.$message.error(err.msg || err);
      });
    },
    onSubComment(data, onSuccess) {
      const rloading = this.openLoading();
      data.articleId = this.articleId;
      SubComment(data).then(res => {
        if (res.success) {
          rloading.close();
          this.msgSuccess('提交评论成功!');
          onSuccess();
          this.getArticleCommentList();
        } else {
          rloading.close();
          this.$message.error(res.msg);
        }
      }).catch(err => {
        rloading.close();
        this.$message.error(err.msg);
      });
    },
    onLikeComment(commentId) {
      LikeComment({ commentId }).then(res => {
        if (res.success) {
          this.msgSuccess('提交评论成功!');
          this.getArticleCommentList();
        } else {
          this.$message.error(res.msg);
        }
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    onDelete(commentId) {
      this.$confirm(`是否确认删除该评论?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        const rloading = this.openLoading();
        const res = await DeleteComment({ commentId });
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.getArticleCommentList();
        this.msgSuccess('删除成功!');
      });
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

</style>
