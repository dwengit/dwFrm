<template>
  <treeselect v-model="deptId" :normalizer="normalizer" :options="deptTreeOptions" :show-count="true" placeholder="选择上级部门">
    <label slot="option-label" slot-scope="{ node, shouldShowCount, count }">
      {{ node.label }} <span v-if="shouldShowCount">({{ count }})</span>
    </label>
  </treeselect>
</template>

<script>
import Treeselect from '@riophae/vue-treeselect';
import '@riophae/vue-treeselect/dist/vue-treeselect.css';
import { AllDeptTreeOptions } from 'api/system';
export default {
  name: 'DeptTree',
  components: { Treeselect },
  model: {
    prop: 'value',
    event: 'input'
  },
  props: {
    value: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      deptTreeOptions: []
    };
  },
  computed: {
    deptId: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit('input', val);
      }
    }
  },
  created() {
    this.loadDeptTree();
  },
  methods: {
    loadDeptTree() {
      AllDeptTreeOptions().then(res => {
        this.deptTreeOptions = res.data;
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
