<template>
  <div class="clearfix">
    <mu-card-title></mu-card-title>
    <mu-text-field v-model="content" class="comment-input" placeholder="说点什么..." multi-line :rows="4" full-width :max-length="maxContentLen" @blur="handleBlur"></mu-text-field>
    <div class="comment-btn" style="flex-wrap: wrap;" :style="{ 'padding-bottom': isPC ? '' : '0.55rem' }">
      <emoji :content.sync="content" :cursor-index.sync="cursorIndex" :max-content-len="maxContentLen" />
      <mu-text-field v-model="nikeName" :max-length="10" :style="{ width: isPC ? '' : '70%' }" style="margin-right: 16px;" placeholder="输入您的昵称" prop="username"></mu-text-field>
      <mu-button color="primary" @click="submit">评论</mu-button>
    </div>

    <mu-dialog title="提示" width="600" max-width="80%" :esc-press-close="false" :overlay-close="false" :open.sync="openAlert">
      系统将对评论内容进行自动审核，您确定内容无违规关键词吗？
      <mu-button slot="actions" flat color="primary" @click="cancel()">取消</mu-button>
      <mu-button slot="actions" flat color="primary" @click="ok()">确定</mu-button>
    </mu-dialog>
  </div>
</template>
<script>
import emoji from 'components/emoji';
import { guidEmpty } from 'utils';

export default {
  name: 'Comment',
  components: {
    emoji
  },
  data() {
    return {
      content: '',
      openAlert: false,
      nikeName: '',
      emojiList: [],
      cursorIndex: 0,
      maxContentLen: 400
    };
  },
  created() {

  },
  methods: {
    submit() {
      if (this.content) {
        this.openAlert = true;
      } else {
        this.$toast.info('请输入评论内容');
      }
    },
    ok() {
      let userInfo = this.$store.getters['user/userInfo'];
      if (!this.nikeName && !userInfo.isLoginNotExp) {
        return this.$toast.error('请输入昵称');
      }
      var data = { commentContent: this.content, commentNickName: this.nikeName, parentCommentId: guidEmpty, commentType: 0 };
      this.$emit('onSubComment', data, () => {
        this.openAlert = false;
        this.content = '';
      });
    },
    cancel() {
      this.openAlert = false;
    },
    handleBlur(e) {
      this.cursorIndex = e.srcElement.selectionStart;
    }
  }
};
</script>
<style lang="less" scoped>
  .comment-input {
    padding: 0 0.42667rem;
  }
  .comment-btn {
    float: right;
    padding: 0 0.42667rem;
  }
</style>
