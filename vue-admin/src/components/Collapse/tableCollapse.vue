<!-- 折叠面板 -->
<template>
  <div class="main-pannel">
    <div ref="panel" class="panel">
      <slot />
    </div>
  </div>
</template>

<script>
export default {
  name: 'TableCollapse',
  components: {},
  props: {},
  data() {
    return {};
  },
  created() {},
  mounted() {
    console.log(this.$parent.$refs.collapse);
  },
  methods: {}
};
</script>
<style lang="scss" scoped>
.main-pannel {
  height: 100%;
}
.panel {
  margin-bottom: 15px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  background-color: #fff;
  padding: 10px 15px;
  box-sizing: content-box;
  border-radius: 10px;
  min-height: 75%;
  .el-row:first-child {
    margin-bottom: 15px;
  }
}
</style>
