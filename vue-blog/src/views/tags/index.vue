<template>
  <div
    class="common"
    :style="{
      background: `url(${bgImg}) center center no-repeat`,
      backgroundSize: 'cover',
    }"
  >
    <div class="content">
      <div class="tags-wap tagcloud" :style="{ width: isPC ? '70%' : '100%' }">
        <a v-for="(item, index) in tags" :key="index" @click="toArticlePage(item.value)">
          <mu-chip class="tag" :color="randomColor()">{{ item.label }}</mu-chip>
        </a>
      </div>
    </div>
    <blog-footer fixed />
  </div>
</template>

<script>
import blogFooter from 'components/blogFooter.vue';
import { GetTags } from 'api';
export default {
  name: 'Tags',
  components: {
    blogFooter
  },
  data() {
    return {
      tags: [],
      bgImg: '/images/tags.png'
    };
  },
  mounted() {
    window.tagcloud({
      selector: '.tagcloud', // 元素选择器
      fontsize: 16, // 基本字体大小, 单位px
      radius: 100, // 滚动半径, 单位px
      mspeed: 'normal', // 滚动最大速度, 取值: slow, normal(默认), fast
      ispeed: 'normal', // 滚动初速度, 取值: slow, normal(默认), fast
      direction: 135, // 初始滚动方向, 取值角度(顺时针360): 0对应top, 90对应left, 135对应right-bottom(默认)...
      keep: false // 鼠标移出组件后是否继续随鼠标滚动, 取值: false, true(默认) 对应 减速至初速度滚动, 随鼠标滚动
    });
  },
  methods: {
    getTags() {
      GetTags().then(res => {
        if (res.statusCode < 1) {
          return this.$toast.error(res.msg);
        }
        this.tags = res.data;
      });
    },
    toArticlePage(id) {
      this.$router.push({ name: 'articles', query: id });
    }
  }
};
</script>
<style lang="less" scoped>
  .content {
    padding-top: 64px;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    bottom: 0;
    right: 0;
  }

  .tags-wap {
    padding: 0 0.53333rem;
    width: 70%;
    .tag {
      margin-right: 0.53333rem;
      margin-bottom: 0.53333rem;
      cursor: pointer;
    }
  }
</style>
