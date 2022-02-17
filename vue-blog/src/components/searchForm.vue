<template>
  <div>
    <mu-dialog :fullscreen="!isPC" width="60%" :open.sync="openModal">
      <slot name="title">
        <mu-auto-complete
          v-model="keyword"
          action-icon=":iconfont icon-search"
          label-float
          :data="keywords"
          label="文章搜索"
          :max-search-results="20"
          open-on-focus
          avatar
          full-width
          :action-click="handleSearch"
          @change="handleSearch"
        >
          <template slot-scope="scope">
            <mu-list-item-action>
              <mu-avatar color="primary">{{ scope.item.substring(0, 1) }}</mu-avatar>
            </mu-list-item-action>
            <mu-list-item-content v-html="scope.highlight"></mu-list-item-content>
          </template>
        </mu-auto-complete>
      </slot>

      <div v-if="list && list.length === 0" class="no-content">暂无内容</div>

      <mu-list v-else v-loading="loadingSearch" style="min-height: 1.5rem;" class="list" textline="two-line">
        <mu-list-item v-for="item in list" :key="item._id" button @click="goDetail(item)">
          <mu-list-item-content>
            <mu-list-item-title>{{ item.title }}</mu-list-item-title>
            <mu-list-item-sub-title>
              <span>{{ item.introduction }}</span>
            </mu-list-item-sub-title>
          </mu-list-item-content>
          <mu-list-item-action style="min-width:140px;">{{ item.createTime }}</mu-list-item-action>
        </mu-list-item>
      </mu-list>

      <mu-button v-if="!isPC" class="close" icon @click="clear(false)">
        <mu-icon value=":iconfont icon-close"></mu-icon>
      </mu-button>
    </mu-dialog>
  </div>
</template>
<script>
import { GetTags, GetArticleShowPage } from 'api';
export default {
  props: {
    toggle: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      tags: [],
      keywords: [],
      keyword: '',
      list: null,
      fullscreen: false,
      loadingSearch: false
    };
  },
  computed: {
    openModal: {
      get() {
        return this.toggle;
      },
      set(val) {
        this.clear(val);
      }
    }
  },
  mounted() {
    this.getTags();
  },
  methods: {
    clear(val) {
      this.keyword = '';
      this.$emit('update:toggle', val);
      this.list = null;
    },
    async handleSearch() {
      if (!this.keyword) return;
      this.loadingSearch = true;
      // 接口请求搜索 传递this.keyword
      let that = this;
      let reqData = { pageIndex: 1, pageSize: 10, tagId: '' };
      let tag = this.tags.find(f => f.label === this.keyword);
      reqData.tagId = tag?.value;
      reqData.articleTitle = reqData.tagId ? '' : this.keyword;
      this.$progress.start();
      GetArticleShowPage(reqData).then(res => {
        this.$progress.done();
        this.loadingSearch = false;
        if (res.statusCode < 1) {
          return that.$toast.error(res.msg);
        }
        that.list = res.data.items.map(m => { return { title: m.articleTitle, introduction: m.introduction }; });
      });
    },
    goDetail(item) {
      this.clear();
      this.$router.push({
        name: 'articlesDetails',
        query: { id: item.id }
      });
    },
    getTags() {
      GetTags().then(res => {
        if (res.statusCode < 1) {
          return this.$toast.error(res.msg);
        }
        this.tags = res.data;
        this.keywords = res.data.map(m => { return m.label; });
      });
    }
  }
};
</script>
<style lang="less" scoped>
  .list {
    overflow-y: auto;
    max-height: 450px;
  }
  .no-content {
    text-align: center;
  }
  .close {
    position: fixed;
    bottom: 20px;
    left: 50%;
    transform: translateX(-50%);
  }
</style>
