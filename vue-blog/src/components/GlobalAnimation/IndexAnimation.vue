<template>
  <div>
    <!-- <canvas ref="canvas" class="index-canvas">Canvas not supported.</canvas> -->
    <canvas id="canvas" />
  </div>
</template>
<script>
// import { index_fullScreenAnimation } from 'utils/index-fullScreen.js';
export default {
  async mounted() {
    window.canvas_word('canvas');
  }
};
</script>
<style lang="less" scoped>
#canvas {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  height: 100%;
  width: 100%;
}
#canvas{
  background-image: radial-gradient(circle, #73aad6, #003962, #000);
}
</style>
