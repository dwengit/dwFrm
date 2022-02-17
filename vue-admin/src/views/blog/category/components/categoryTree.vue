<template>
  <treeselect v-model="id" v-bind="$attrs" :normalizer="normalizer" :options="TreeOptions" :show-count="true" placeholder="选择上级分类" append-to-body>
    <label slot="option-label" slot-scope="{ node, shouldShowCount, count }">
      {{ node.label }} <span v-if="shouldShowCount">({{ count }})</span>
    </label>
  </treeselect>
</template>

<script>
import { guidEmpty } from 'utils';
import Treeselect from '@riophae/vue-treeselect';
import '@riophae/vue-treeselect/dist/vue-treeselect.css';
import { GetCategoryTreeOption } from 'api/blog';

export default {
  name: 'CategoryTree',
  components: { Treeselect },
  model: {
    prop: 'value',
    event: 'input'
  },
  props: {
    value: {
      type: String,
      default: null
    },
    isCreateRootId: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      TreeOptions: []
    };
  },
  computed: {
    id: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit('input', val);
      }
    }
  },
  created() {
    this.loadTree();
  },
  methods: {
    loadTree() {
      GetCategoryTreeOption().then(res => {
        if (this.isCreateRootId) {
          const rootTree = { id: guidEmpty, label: '根分类', children: [] };
          rootTree.children = res.data;
          this.TreeOptions.push(rootTree);
        } else {
          this.TreeOptions = res.data;
        }
      });
    },
    normalizer(node) {
      if (!node.children || !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children
      };
    }
  }
};
</script>

<style lang="scss" scoped>

</style>
