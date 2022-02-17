<template>
  <div>
    <blog-header background="transparent" />
    <keep-alive :include="['Home']">
      <router-view></router-view>
    </keep-alive>
    <!-- 滚动条回到页面顶部按钮 -->
    <mu-slide-bottom-transition>
      <mu-tooltip placement="top" content="回到顶部">
        <mu-button v-show="showBackTop" class="back-top" fab color="secondary" @click="scrollTop">
          <mu-icon value=":iconfont icon-up"></mu-icon>
        </mu-button>
      </mu-tooltip>
    </mu-slide-bottom-transition>
  </div>
</template>

<script>
import blogHeader from 'components/blogHeader.vue';
import $ from 'jquery';
export default {
  name: 'Layout',
  components: {
    blogHeader
  },
  data() {
    return {
      showBackTop: false
    };
  },
  mounted() {
    let contentDom = document.getElementsByClassName('content')[0];
    if (contentDom) {
      contentDom.onscroll = () => {
        if (document.documentElement.scrollTop + contentDom.scrollTop > 100) {
          this.showBackTop = true;
        } else {
          this.showBackTop = false;
        }
      };
    }
  },
  methods: {
    scrollTop() {
      let $content = $('.content');
      if ($content) {
        $content.animate({ scrollTop: 0 }, 1000);
      }
    }
  }
};
</script>
<style lang="less" scoped>
  .back-top {
    position: fixed;
    right: 0.56667rem;
    bottom: 0.4rem;
    background: #595959;
  }
</style>
