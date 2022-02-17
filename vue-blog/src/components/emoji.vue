<template>
  <mu-tooltip placement="top">
    <mu-button icon color="primary">
      <mu-icon value=":iconfont icon-xiaolian"></mu-icon>
    </mu-button>
    <template #content>
      <div class="emoji-list">
        <ul>
          <li v-for="(item, index) in emojiList" :key="index" @click="handleEmoji(item)">
            {{ item.text }}
          </li>
        </ul>
      </div>
    </template>
  </mu-tooltip>
</template>

<script>
import { emojis } from 'utils';
export default {
  name: 'Emoji',
  props: {
    cursorIndex: {
      type: Number,
      default: 0
    },
    content: {
      type: String,
      default: ''
    },
    maxContentLen: {
      type: Number,
      default: 400
    }
  },
  created() {
    this.emojiList = emojis.map(emoji => ({ text: emoji }));
  },
  methods: {
    handleEmoji(emoji) {
      // 判断是否继续插入emoji表情（emoji默认占用2个字符）
      let content = this.content;
      let cursorIndex = this.cursorIndex;
      if (this.maxContentLen - this.content.length >= 2) {
        if (content.length < cursorIndex) {
          content += emoji.text;
        } else {
          const s1 = content.substring(0, cursorIndex);
          const s2 = content.substring(cursorIndex, content.length);
          content = s1 + emoji.text + s2;
        }
        cursorIndex += 2;
      }
      this.$emit('update:content', content);
      this.$emit('update:cursorIndex', cursorIndex);
    }
  }
};
</script>

<style lang="less" scoped>

</style>
