<template>
  <div class="prev-next">
    <mu-button v-if="prev && prev.id" flat color="success" @click="goDetails(prev)">&gt;&gt; 上一篇： {{ prev.title }}</mu-button>
    <mu-button v-if="next && next.id" flat color="info" @click="goDetails(next)">&gt;&gt; 下一篇： {{ next.title }}</mu-button>
  </div>
</template>
<script>
export default {
  props: {
    prev: {
      type: Object,
      default: () => {
        return { title: '上一篇文章没有了', id: '' };
      }
    },
    next: {
      type: Object,
      default: () => {
        return { title: '下一篇文章没有了', id: '' };
      }
    }
  },
  methods: {
    goDetails(item) {
      if (item.id) {
        this.$router.push({
          name: 'articlesDetails',
          query: { id: item.id }
        });
        location.reload();
        document.body.scrollIntoView();
      }
    }
  }
};
</script>
<style lang="less" scoped>
  .prev-next {
    font-size: 0.24rem;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
  }
</style>
