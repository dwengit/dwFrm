<template>
  <div class="comment-list">
    <div class="comment-item">
      <mu-card v-for="item in list" :key="item.id" class="card" :class="[classStyle, isPC ? '' : 'wap-card']">
        <mu-card-header :title="item.commentNickName" :sub-title="item.createTime">
          <mu-avatar slot="avatar">
            <img :src="item.avatar">
          </mu-avatar>
        </mu-card-header>
        <mu-card-text>
          <span v-if="item.replyNickName" class="item.who">@{{ item.replyNickName }}&nbsp;&nbsp;</span>
          <span :class="{ 'text-delete': item.isDelete }">{{ item.commentContent }}</span>
        </mu-card-text>

        <mu-card-actions class="comment-card-actions">
          <mu-button small color="primary" @click="replay(item)">回复</mu-button>
          <mu-button small :color="item.isLike ? 'error' : 'success'" :class="{ 'is-link': item.isLike }" @click="onLikeComment(item.id)">
            <mu-icon left value=":iconfont icon-zantongfill"></mu-icon>{{ item.isLike ? '已赞' : '赞' }}（{{ item.commentLikeNum }}）
          </mu-button>
          <mu-button v-if="!item.isDelete && userInfo.isLoginNotExp" small color="error" @click="onDeleteComment(item.id)">删除</mu-button>
        </mu-card-actions>

        <!-- 递归组件 调用自身，必须指定name属性commentList -->
        <div v-if="item.replys && item.replys.length">
          <comment-list class-style="sub-card" v-bind="$attrs" :article-id="articleId" :list="item.replys" v-on="$listeners"></comment-list>
        </div>
      </mu-card>
    </div>

    <mu-dialog :title="modalTitle" width="600" max-width="80%" :esc-press-close="false" :overlay-close="false" :open.sync="open">
      <mu-text-field v-model="replayContent" :max-length="maxContentLen" class="comment-input" placeholder="说点什么..." multi-line :rows="4" full-width @blur="handleBlur"></mu-text-field>
      <mu-text-field v-model="nickName" :max-length="10" placeholder="输入您的昵称" prop="username"></mu-text-field>
      <emoji :content.sync="replayContent" :cursor-index.sync="cursorIndex" :max-content-len="maxContentLen" />
      <mu-button slot="actions" flat color="primary" @click="close">取消</mu-button>
      <mu-button slot="actions" flat color="primary" @click="ok">确定</mu-button>
    </mu-dialog>
  </div>
</template>
<script>
import emoji from 'components/emoji';
import { guidEmpty } from 'utils';
export default {
  name: 'CommentList',
  components: {
    emoji
  },
  props: {
    list: {
      type: Array,
      default: () => { }
    },
    articleId: {
      type: String,
      default: ''
    },
    classStyle: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      nickName: '',
      open: false,
      replayContent: '',
      modalTitle: '',
      user: JSON.parse(localStorage.getItem('user')),
      cursorIndex: 0,
      maxContentLen: 400,
      replyComment: null
    };
  },
  computed: {
    userInfo() {
      let userInfo = this.$store.getters['user/userInfo'];
      return userInfo;
    }
  },
  methods: {
    // 回复
    replay(item) {
      this.open = true;
      this.modalTitle = `回复 @${item.commentNickName}`;
      this.replyComment = item;
    },
    // 点赞{
    onLikeComment(commentId) {
      this.$emit('onLikeComment', commentId);
    },
    onDeleteComment(commentId) {
      this.$emit('onDeleteComment', commentId);
    },
    close() {
      this.open = false;
      this.replayContent = '';
      this.replyComment = null;
    },
    ok() {
      if (!this.nickName) {
        this.$toast.info('请输入您的昵称');
        return;
      }
      if (!this.replayContent) {
        this.$toast.info('请输入回复内容');
        return;
      }
      var data = { commentContent: this.replayContent, commentNickName: this.nickName, parentCommentId: guidEmpty, commentType: 0 };
      // 回复的对象不为null
      if (this.replyComment != null) {
        data.commentRootId = this.replyComment.commentType === 0 ? this.replyComment.id : this.replyComment.commentRootId;
        data.parentCommentId = this.replyComment.id;
        data.commentType = 1;
      }

      this.$emit('onSubComment', data, () => {
        this.close();
      });
    },
    handleBlur(e) {
      this.cursorIndex = e.srcElement.selectionStart;
    }
  }
};
</script>
<style lang="less" scoped>
  .comment-item {
    /deep/ .mu-card-text {
      padding-top: 0;
      .who {
        color: #e91e63;
      }
    }
  }
  .card {
    margin: 0rem 1.06667rem 0 1.1rem;
    padding-bottom: 0;
    box-shadow: none;
    border-radius: 0;
  }
  .wap-card {
    margin: 0px 10px 0 10px;
  }
  .sub-card {
    border-left: 1px solid #3c8b65;
    border-bottom: 1px solid #3c8b65;
    box-shadow: none;
    border-radius: 0;
    padding-bottom: 0.1rem;
  }
  .comment-card-actions {
    padding: 16px;
    display: flex;
    flex-wrap: wrap;
    button {
      margin-right: 0.22rem;
    }
  }
  .is-link {
    cursor: no-drop !important;
  }
</style>
