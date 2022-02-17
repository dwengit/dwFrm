<template>
  <div>
    <div class="list">
      <ul v-if="value.length">
        <li v-for="item in value" :key="item.id">
          <comment :value="item" @onReply="onReply" @onLikeComment="onLikeComment" @onDelete="onDelete" />
          <ul v-if="item.replys && item.replys.length" class="reply">
            <li v-for="reply in item.replys" :key="reply.id"><hr><comment :value="reply" @onReply="onReply" @onLikeComment="onLikeComment" @onDelete="onDelete" /></li>
          </ul>
        </li>
      </ul>
      <p v-else style="text-align: center;font-size: 24px;">暂无评论</p>
    </div>
    <div style="margin-top: 20px;">
      <div class="box">
        <el-input ref="content" v-model="content" :max="maxContentLen" type="textarea" :rows="5" :placeholder="replysNikeName" @blur="handleBlur" />
        <div class="box-sub">
          <el-popover width="300" trigger="click">
            <div class="emoji-list">
              <ul>
                <li v-for="(item, index) in emojis" :key="index" @click="handleEmoji(item)">
                  {{ item.text }}
                </li>
              </ul>
            </div>
            <svg-icon slot="reference" icon-class="emoji" class-name="emoji-btn" />
          </el-popover>
          <el-input v-model="nikeName" style="width:30%" placeholder="请输入昵称" />
          <el-button type="primary" @click="onSubComment">提交</el-button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import comment from './comment.vue';
import { guidEmpty } from 'utils';
const emojis = [
  '😀', '😄', '😅', '🤣', '😂', '😉', '😊', '😍', '😘', '😜',
  '😝', '😏', '😒', '🙄', '😔', '😴', '😷', '🤮', '🥵', '😎',
  '😮', '😰', '😭', '😱', '😩', '😡', '💀', '👽', '🤓', '🥳',
  '😺', '😹', '😻', '🤚', '💩', '👍', '👎', '👏', '🙏', '💪'
];
export default {
  name: 'CommentList',
  components: { comment },
  props: {
    value: {
      type: Array,
      default: function() {
        return [];
      }
    }
  },
  data() {
    return {
      replyComment: null,
      replysNikeName: '',
      content: '',
      nikeName: '',
      cursorIndex: 0,
      maxContentLen: 400
    };
  },
  created() {
    this.emojis = emojis.map(emoji => ({ text: emoji }));
  },
  methods: {
    onSubComment() {
      var data = { commentContent: this.content, commentNickName: this.nikeName, parentCommentId: guidEmpty, commentType: 0 };
      // 回复的对象不为null
      if (this.replyComment != null) {
        data.commentRootId = this.replyComment.commentType === 0 ? this.replyComment.id : this.replyComment.commentRootId;
        data.parentCommentId = this.replyComment.id;
        data.commentType = 1;
      }
      const that = this;
      this.$emit('onSubComment', data, function() {
        that.content = '';
        that.replyComment = null;
        that.replysNikeName = '';
      });
    },
    onReply(replyComment) {
      this.scrollToBottom();
      this.replysNikeName = '@' + replyComment.commentNickName;
      this.replyComment = replyComment;
    },
    onLikeComment(commentId) {
      this.$emit('onLikeComment', commentId);
    },
    onDelete(commentId) {
      this.$emit('onDelete', commentId);
    },
    scrollToBottom() {
      window.scrollTo(0, document.documentElement.clientHeight);
      this.$refs.content.focus();
    },
    handleBlur(e) {
      this.cursorIndex = e.srcElement.selectionStart;
    },
    handleEmoji(emoji) {
      // 判断是否继续插入emoji表情（emoji默认占用2个字符）
      if (this.maxContentLen - this.content.length >= 2) {
        if (this.content.length < this.cursorIndex) {
          this.content += emoji.text;
        } else {
          const s1 = this.content.substring(0, this.cursorIndex);
          const s2 = this.content.substring(this.cursorIndex, this.content.length);
          this.content = s1 + emoji.text + s2;
        }
        this.cursorIndex += 2;
      }
    }
  }
};
</script>

<style lang="scss" scoped>
  ul li {
    list-style-type: none;
  }
  .list{
    width: 80%;
    margin: 0 auto;
    border-radius: 15px;
    padding: 20px;
    margin-top: 10px;
    background-color: #bcd4e6;
    .reply {
      padding-left: 50px;
    }
  }
  .box{
    border-radius: 15px;
    width:80%;
    margin: 0 auto;
    padding: 20px;
    background-color: #bcd4e6;
    ::v-deep .el-textarea textarea {
      border-radius: 15px;
      font-size: 18px;
    }

}
.box-sub{
  padding: 10px 0 0;
  text-align: right;
  button {
    margin-left: 10px;
  }
}
.emoji-list{
  background-color: #fff;
  ul {
    padding: 5px 0;
    margin: 0;
  }
  ul li {
    float: left;
    cursor: pointer;
    font-size: 24px;
  }
}
.emoji-btn {
    font-size: 34px;
    color: #fcbf49;
    position: relative;
    top: 8px;
    margin-right: 8px;
    cursor: pointer;
  }
</style>
