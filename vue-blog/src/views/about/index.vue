<template>
  <div
    class="common"
    :style="{
      background: `url(${bgImg}) center center no-repeat`,
      backgroundSize: 'cover',
    }"
  >
    <div class="content">
      <mu-paper class="about" :z-depth="4">
        <mu-avatar class="avatar" :size="100">
          <img src="/images/avatar.jpg">
        </mu-avatar>
        <h4>关于我</h4>
        <p>一个95后，喜欢吃火锅，喜欢撸猫，喜欢编程，学的技术挺多却不深。</p>
        <h4>联系我</h4>
        <p>邮箱：devin@yousu.xyz</p>
        <h4>我的项目</h4>
        <a href="https://github.com/dwengit" target="_black">GitHub：https://github.com/dwengit</a><br>
        <a href="https://gitee.com/mydwen" target="_black">码云：https://gitee.com/mydwen</a>
      </mu-paper>
    </div>
    <blog-footer fixed />
  </div>
</template>

<script>
import blogFooter from 'components/blogFooter.vue';

export default {
  name: 'Tags',
  components: {
    blogFooter
  },
  data() {
    return {
      bgImg: '/images/about.png'
    };
  },
  mounted() {

  },
  methods: {
  }
};
</script>
<style lang="less" scoped>
  .content {
    padding-top: 64px;
    display: flex;
    justify-content: center;
    align-items: center;
    position: absolute;
    top: 20%;
    width: 100%;
  }
  .about {
    max-width: 10rem;
    width: 10rem;
    background-color: #3c2b2f;
    position: relative;
    padding: 50px;
    .avatar {
      width: 100px;
      height: 100px;
      font-size: 50px;
      margin: 0 auto;
      display: inherit;
      position: absolute;
      top: -50px;
      right: 0;
      left: 0;
      box-shadow: 1px 2px 20px #878a8b;
    }
    h4 {
      font-size: 0.3rem;
      color: #afff81;
    }
    a{
      font-size: 0.4rem;
    }
    p {
      color: #eee;
      font-size: 0.3rem;
      text-indent: 0.6rem;
    }
  }
</style>
