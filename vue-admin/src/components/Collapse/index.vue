<!-- 折叠面板 -->
<template>
  <div class="main-pannel">
    <transition
      name="search"
      @before-enter="beforeEnter"
      @enter="enter"
      @after-enter="afterEnter"
      @before-leave="beforeLeave"
      @leave="leave"
      @after-leave="afterLeave"
    >
      <div v-show="copShow" ref="panel" class="panel">
        <div style="padding: 10px 15px 0">
          <slot />
        </div>
      </div>
    </transition>
    <transition name="search2">
      <div v-show="copShow" ref="panel2" class="panel2" />
    </transition>
  </div>
</template>

<script>
export default {
  name: 'Collapse',
  props: {
    show: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      isShow: true,
      height: 0
    };
  },
  computed: {
    copShow: function() {
      return this.show && this.isShow;
    }
  },
  created() {},
  mounted() {
    this.height = this.$refs.panel.offsetHeight;
  },
  methods: {
    beforeEnter(el) {
      el.style.maxHeight = '0';
      el.style.opacity = 0;
    },
    enter(el) {
      el.offsetWidth;
      el.style.opacity = 1;
      el.style.maxHeight = this.height + 'px';
    },
    afterEnter(el) {
      el.style.height = null;
    },
    beforeLeave(el) {
      el.style.opacity = 1;
      el.style.maxHeight = this.height + 'px';
    },
    leave(el) {
      el.offsetWidth;
      el.style.opacity = 0;
      el.style.maxHeight = '0';
    },
    afterLeave(el) {
      el.style.height = null;
    }
  }
};
</script>
<style lang="scss" scoped>
.panel {
  overflow: hidden;
  transition: all 0.5s ease-in-out;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  background-color: #fff;
  box-sizing: content-box;
  border-radius: 10px;
}
.panel2 {
  height: 15px;
  transition: height 0.5s ease-out;
}
.search2-enter-active,
.search2-leave-active {
  height: 15px;
}
.search2-enter, .search2-leave-to /* .fade-leave-active below version 2.1.8 */ {
  height: 0;
}
.btn-fold {
  cursor: pointer;
  font-size: 20px;
  float: right;
  color: #8b8d90;
}
</style>
