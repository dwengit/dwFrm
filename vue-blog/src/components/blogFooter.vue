<template>
  <div class="footer" :class="{ 'fixed-footer': fixed }">
    <div class="copyright">
      <a>Copyright &copy; 2022 &middot;《阿喵丶》的博客 </a>
    </div>
  </div>
</template>
<script>
export default {
  props: {
    fixed: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {};
  },
  mounted() { },
  methods: {}
};
</script>
<style lang="less" scoped>
  .fixed-footer {
    position: fixed;
    bottom: 0;
    width: 100%;
    z-index: 1;
  }

  .footer {
    color: #eee;
    text-align: center;
    font-size: 0.2rem;
    .copyright {
      margin: 30px 0;
      a {
        color: inherit;
      }
    }
  }
</style>
