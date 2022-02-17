<template>
  <div class="pad-card" :style="{ 'border-radius': borderRadius, 'padding': padding}">
    <slot />
  </div>
</template>

<script>
export default {
  name: 'GlobalPadCard',
  props: {
    borderRadius: {
      type: String,
      default: '10px'
    },
    padding: {
      type: String,
      default: '14px'
    }
  }
};
</script>

<style lang="scss" scoped>
  .pad-card {
    border: 1px solid #e6ebf5;
    background-color: #ffffff;
    overflow: auto;
    color: #303133;
    transition: 0.3s;
    box-shadow: 0 2px 12px 0 rgb(0 0 0 / 10%);

    ::v-deep {
      .el-tree-node {
        white-space: normal;
        .el-tree-node__content {
          height: 100%;
          align-items: start;
        }
      }
    }
  }
</style>
