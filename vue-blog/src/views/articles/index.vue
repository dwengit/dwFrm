<template>
  <div
    class="common"
    :style="{
      background: `url(${bgImg}) center center no-repeat`,
      backgroundSize: 'cover',
    }"
  >
    <div class="content">
      <div :class="[{ 'wap-left': !isPC }, 'article-card']">
        <mu-card v-for="item in articlePage.items" :key="item.id" class="card" raised @click="toDetail(item.id)">
          <mu-card-media v-if="isPC" class="cover">
            <img v-lazy="item.coverImage" class="cover-img">
          </mu-card-media>
          <div class="card-box">
            <div class="title">{{ item.articleTitle }}</div>
            <mu-card-actions class="sub-title">
              <mu-button class="cursor-default" flat color="info">浏览({{ item.viewNum }})</mu-button>
              <mu-button class="cursor-default" flat color="error">评论({{ item.commentNum }})</mu-button>
              <mu-button class="cursor-default" flat color="primary">点赞({{ item.likeNum }})</mu-button>
              <mu-button class="cursor-default" flat color="#9e9e9e">2021-05-08 19:56</mu-button>
            </mu-card-actions>
            <mu-card-text class="text">{{ item.introduction }}</mu-card-text>
            <mu-card-actions class="card-bottom">
              <mu-button flat class="chip cursor-default" color="primary">
                <mu-icon left value=":iconfont icon-category"></mu-icon>
                <span class="icon-title">{{ item.categoryTitle }}</span>
              </mu-button>

              <mu-button v-for="tag in item.tags" :key="tag.value" flat class="chip cursor-default" :color="randomColor()">
                <mu-icon left value=":iconfont icon-tag"></mu-icon>
                <span class="icon-title">{{ tag.label }}</span>
              </mu-button>
            </mu-card-actions>
          </div>
        </mu-card>
      </div>
      <div v-if="articlePage.pageTotal > 1" class="pagination">
        <mu-pagination raised circle :total="articlePage.total" :current.sync="articlePage.pageIndex" :page-size.sync="articlePage.pageSize" :page-count="articlePage.pageTotal"></mu-pagination>
      </div>

      <blog-footer />
    </div>
  </div>
</template>
<script>
import blogFooter from 'components/blogFooter.vue';
import { GetArticleShowPage } from 'api';
export default {
  name: 'Articles',
  components: {
    blogFooter
  },
  data() {
    return {
      bgImg: '/images/article.png',
      articlePage: {
        pageIndex: 1,
        pageSize: 10,
        total: 0,
        pageTotal: 0,
        Items: []
      },
      reqData: {
        pageIndex: 1,
        pageSize: 10,
        tagId: ''
      }
    };
  },
  created() {
    this.loadList();
    this.$watch(
      () => this.$route.query,
      (toParams, previousParams) => {
        this.loadList(this);
      }
    );
  },
  mounted() { },
  methods: {
    loadList() {
      let that = this;
      this.reqData.tagId = this.$route.query.tagid;
      this.reqData.categoryid = this.$route.query.categoryid;
      this.$progress.start();
      GetArticleShowPage(this.reqData).then(res => {
        this.$progress.done();
        if (res.statusCode < 1) {
          return that.$toast.error(res.msg);
        }
        that.articlePage = res.data;
      });
    },
    toDetail(id) {
      this.$router.push({ name: 'articlesDetail', query: { id }});
    }
  }
};
</script>

<style lang="less" scoped>
  .content {
    position: absolute;
    top: 64px;
    bottom: 0;
    overflow: auto;
    width: 100%;
    padding-top: 20px;
    .article-card {
      display: flex;
      flex-wrap: wrap;
      &.wap-left {
        .card {
          float: none;
          width: 90%;
        }
      }
      .card {
        width: 80%;
        float: left;
        margin: 0.42667rem auto 0;
        display: flex;
        flex-wrap: wrap;
        &:hover {
          animation: pulse 1s;
        }
        .title {
          font-weight: 600;
          // color: inherit;
          padding: 0 0.42667rem;
          font-size: 0.4rem;
          overflow: hidden;
          text-overflow: ellipsis;
          display: -webkit-box;
          -webkit-box-orient: vertical;
          -webkit-line-clamp: 1;
          cursor: pointer;
        }
        .sub-title {
          display: flex;
          flex-wrap: wrap;
        }
        .text {
          // color: inherit;
          font-weight: 500;
          padding: 0 0.42667rem;
          overflow: hidden;
          text-overflow: ellipsis;
          display: -webkit-box;
          -webkit-box-orient: vertical;
          -webkit-line-clamp: 3;
        }
        .chip {
          margin-right: 0.26667rem;
        }
        .cover {
          flex: 1;
          border-radius: 0;
          .cover-img {
            object-fit: cover;
            width: 100%;
            height: 4.26667rem;
            vertical-align: middle;
          }
        }
        .card-box {
          padding-top: 0.4rem;
          display: flex;
          flex-direction: column;
          justify-content: space-around;
          flex: 2;
        }
        .icon-title {
          position: relative;
          top: 0.04rem;
        }
        .card-bottom {
          /deep/ .mu-button-wrapper {
            padding: 0;
          }
        }
      }
    }
    .right {
      flex: 3;
      display: flex;
      justify-content: center;
    }
  }

  .box {
    justify-content: center !important;
    padding-bottom: 0.53333rem;
  }

  .pagination {
    margin: 1rem 0;
    display: flex;
    justify-content: center;
  }
</style>
