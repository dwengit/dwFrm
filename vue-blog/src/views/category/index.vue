<template>
  <div>
    <div>
      <category-pc v-if="isPC"></category-pc>
      <category-mobile v-else></category-mobile>
    </div>
    <blog-footer fixed />
  </div>

</template>

<script>
import categoryPc from './categoryPc.vue';
import categoryMobile from './categoryMobile.vue';
import blogFooter from 'components/blogFooter.vue';
import { GetCategoryTree } from 'api';
export default {
  name: 'Category',
  components: { categoryPc, categoryMobile, blogFooter },
  data() {
    return {
      categoryData: []
    };
  },
  created() {
    this.getCategoryTree();
  },
  methods: {
    getCategoryTree() {
      GetCategoryTree().then(res => {
        if (res.statusCode < 1) {
          return this.$toast.error(res.msg);
        }
        this.categoryData = { label: '博文分类', id: '', children: res.data };
      });
    }
  }
};
</script>

<style lang="less" scoped>
</style>
