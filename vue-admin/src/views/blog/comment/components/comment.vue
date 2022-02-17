<template>
  <div style="margin-bottom: 16px;">
    <div class="comment-top">
      <el-avatar icon="el-icon-user-solid" />
      <span class="nike-name">{{ value.commentNickName }}<template v-if="value.replyNickName">&nbsp;<span
        style="color: blueviolet;
    font-size: 15px;"
      >回复</span>&nbsp;{{ value.replyNickName }}</template></span>
      <span class="comment-time">{{ value.createTime }}</span>
    </div>
    <div class="content">
      <p v-if="!value.isDelete">{{ value.commentContent }}</p>
      <s v-else style="color:#e63946">{{ value.commentContent }}</s>
    </div>
    <div v-if="!value.isDelete" class="comment-bottom">
      <span class="link" :class="{ 'is-link': value.isLike }" @click="onLikeComment">
        <svg-icon icon-class="like" />&nbsp;<span class="like-num">{{ value.commentLikeNum }}</span>
      </span>
      <span class="message" @click="onReply">
        <svg-icon icon-class="message" class-name="svg-btn" />&nbsp;回复
      </span>
      <span class="message" @click="onDelete">
        <svg-icon icon-class="delete" class-name="svg-btn" />&nbsp;删除
      </span>
    </div>
    <div class="reply">
      <div />
    </div>
  </div>
</template>
<script>
export default {
  name: 'Comment',
  props: {
    value: {
      type: Object,
      default: function() {
        return {};
      }
    }
  },
  methods: {
    onReply() {
      this.$emit('onReply', this.value);
    },
    onLikeComment() {
      if (this.value.isLike) return;
      this.$emit('onLikeComment', this.value.id);
    },
    onDelete() {
      this.$emit('onDelete', this.value.id);
    }
  }
};
</script>

<style lang="scss" scoped>
  .is-link{
    color: #e63946;
    cursor: no-drop !important;
  }
  .comment-top {
    .nike-name {
      font-size: 20px;
      font-weight: 600;
      position: relative;
      top: -11px;
      margin-left: 10px;
    }
    .comment-time {
      float: right;
      font-size: 14px;
      color: #6b6b6b;
    }
  }
  .content,
  .comment-bottom {
    padding-left: 50px;
  }
  .comment-bottom {
    color: #777;
    font-size: 15px;
    .link, .message{
      cursor: pointer;
    }
    .link:hover{
      color:#e63946;
    }
    .like-num {
      position: relative;
      top: 1px;
    }
    .message {
      vertical-align: middle;
      margin-left: 20px;
    }
  }
</style>
