<template>
  <div>
    <index-animation />
    <div class="home">
      <p>{{ info.introduction }}</p>
    </div>
    <blog-footer fixed />
  </div>
</template>

<script>
import blogFooter from 'components/blogFooter.vue';
import indexAnimation from 'components/GlobalAnimation/IndexAnimation.vue';
let i = 0;
let arrIndex = 0;
let direction = true;
let timer = null;
export default {
  name: 'Home',
  components: {
    indexAnimation,
    blogFooter
  },
  data() {
    return {
      info: {
        introduction: '',
        introductionTarget: ['keep smiling! things will calm down. 乐观一点，事情会平息下来的。', '现实很近又很冷，梦想很远却很温暖。', '抓住今日，尽可能少的信赖明天。']
      }
    };
  },
  mounted() {
    console.log('mounted home');
    timer = setInterval(this.typing, 300);
  },
  destroyed() {
    clearInterval(timer);
  },
  methods: {
    typing() {
      if (i > this.info.introductionTarget[arrIndex].length || i < 0) {
        if (!direction) {
          if (arrIndex + 1 === this.info.introductionTarget.length) {
            arrIndex = 0;
          } else {
            arrIndex++;
          }
          i = 0;
        } else {
          i = this.info.introductionTarget[arrIndex].length;
        }
        direction = !direction;
      } this.info.introduction =
        this.info.introductionTarget[arrIndex].slice(0, direction ? i++ : i--) + (direction ? '_' : '|');
    }
  }
};
</script>

<style lang="less" scoped>
.home {
  position: absolute;
  top: 50%;
  width: 100%;
  text-align: center;
  transform: translateY(-50%);
  font-size: 0.48rem;
  font-weight: 500;
  p {
    background-color: rgba(0,0,0,0.2);
    width: auto !important;
    display: inline-block;
    padding: 0.1rem;
    font-weight: bold;
  }
}
</style>
