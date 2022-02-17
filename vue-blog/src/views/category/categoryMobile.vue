<template>
  <div
    class="common"
    :style="{
      background: `url(${bgImg}) 0px center no-repeat`,
      backgroundSize: 'cover',
    }"
  >
    <div class="wap-content">
      <div class="cols">

        <div v-for="category in categorys" :key="category.id" class="cols-item">
          <div class="container">
            <div class="front">
              <div class="inner">
                <p @click="toArticlePage(category.id)">{{ category.label }}</p>
                <div class="tow-category">
                  <mu-button v-for="item in category.children" :key="item.id" small :color="randomColor()" @click="toArticlePage(item.id)">{{ item.label }}</mu-button>
                </div>
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>
<script>
import { GetCategoryTree } from 'api';
export default {
  name: 'Categories',
  components: {
  },
  data() {
    return {
      categorys: [],
      bgImg: '/images/category02.png'
    };
  },
  mounted() {
    this.getCategoryTree();
  },
  methods: {
    getCategoryTree() {
      this.$progress.start();
      GetCategoryTree().then(res => {
        if (res.statusCode < 1) {
          this.$progress.done();
          return this.$toast.error(res.msg);
        }
        this.categorys = res.data;
      });
    },
    toArticlePage(id) {
      let queryData = id ? { categoryid: id } : null;
      this.$router.push({ name: 'articles', query: queryData });
    }
  }
};
</script>
<style lang="less" scoped>
  .content {
    padding-top: 64px;
    display: flex;
    justify-content: space-around;
    align-items: center;
    position: absolute;
    width: 100%;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
  }

  .wap-content {
    position: absolute;
    top: 56px;
    bottom: 0;
    overflow: auto;
    width: 100%;
    padding-top: 20px;
  }

  .cols {
    width: 100%;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-wrap: wrap;
    flex-wrap: wrap;
    -webkit-box-pack: center;
    -ms-flex-pack: center;
    justify-content: center;
  }
  .cols-item {
    width: calc(25% - 32px);
    margin: 16px;
  }
  .container {
    -webkit-transform-style: preserve-3d;
    transform-style: preserve-3d;
    -webkit-perspective: 1000px;
    perspective: 1000px;
  }
  .front,
  .back {
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    text-align: center;
    min-height: 200px;
    height: auto;
    border-radius: 10px;
    color: #fff;
    font-size: 24px;
  }
  .front:after {
    position: absolute;
    top: 0;
    left: 0;
    z-index: 1;
    width: 100%;
    height: 100%;
    content: '';
    display: block;
    opacity: 0.6;
    background-color: #000;
    -webkit-backface-visibility: hidden;
    backface-visibility: hidden;
    border-radius: 10px;
  }
  .inner {
    -webkit-transform: translateY(-50%) translateZ(60px) scale(0.94);
    transform: translateY(-50%) translateZ(60px) scale(0.94);
    top: 50%;
    position: absolute;
    left: 0;
    width: 100%;
    padding: 16px;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    outline: 1px solid transparent;
    -webkit-perspective: inherit;
    perspective: inherit;
    z-index: 2;
  }
  .container .front {
    -webkit-transform: rotateY(0deg);
    transform: rotateY(0deg);
    -webkit-transform-style: preserve-3d;
    transform-style: preserve-3d;
  }
  .front .inner p {
    font-size: 32px;
    margin-top: 0;
    margin-bottom: 32px;
    position: relative;
    cursor: pointer;
  }
  .front .inner p:after {
    content: '';
    // width: 64px;
    height: 2px;
    position: absolute;
    background: #c6d4df;
    display: block;
    left: 0;
    right: 0;
    margin: 0 auto;
    bottom: -12px;
  }
  .front .inner .tow-category {
    color: #fff;
    font-family: 'Montserrat';
    font-weight: 300;
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    /deep/ button {
      margin: 0.1rem;
    }
  }
  @media screen and (max-width: 664px) {
    .cols-item {
      width: calc(33.333333% - 32px);
    }
  }
  @media screen and (max-width: 768px) {
    .cols-item {
      width: calc(50% - 32px);
    }
  }
  @media screen and (max-width: 512px) {
    .cols-item {
      width: 80%;
      margin: 0 0 32px 0;
    }
  }
</style>
