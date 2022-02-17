<template>
  <div>
    <div class="header">
      <mu-appbar :color="background">
        <!-- title -->
        <span style="cursor: pointer">阿喵丶</span>

        <!-- 菜单 -->
        <template v-for="(item, index) in info.menu">
          <mu-menu v-if="item.router === 'hot'" slot="right" :key="item.name" placement="bottom" open-on-hover>
            <mu-button v-show="isPC" color="#f44336" flat>
              <mu-icon size="16" :value="item.icon" />
              {{ item.name }}
            </mu-button>
            <mu-list slot="content">
              <mu-list-item v-for="tag in hotTags" :key="tag.value" button @click="toArticlePageByTagId(tag.value)">
                <mu-list-item-title>{{ tag.label }}</mu-list-item-title>
              </mu-list-item>
            </mu-list>
          </mu-menu>
          <mu-button v-else v-show="isPC" slot="right" :key="item.name" :color="routeName === item.router ? '#00e676' : ''" flat @click="go(item, index)">
            <mu-icon size="16" :value="item.icon" />
            {{ item.name }}
          </mu-button>
        </template>

        <!-- 菜单图标 -->
        <mu-button v-show="!isPC" slot="left" icon @click="toggleWapMenu(true)">
          <mu-icon value=":iconfont icon-zhedie" />
        </mu-button>

        <!-- wap-菜单 -->
        <mu-bottom-sheet :open.sync="openWapMenu">
          <mu-list @item-click="toggleWapMenu(false)">
            <mu-list-item v-for="(item, index) in info.menu" :key="item.name" button @click="go(item, index)">
              <mu-list-item-action>
                <mu-icon :color="routeName === item.router ? '#00e676' : ''" :value="item.icon" />
              </mu-list-item-action>
              <mu-list-item-title :style="{ color: routeName === item.router ? '#00e676' : '' }">{{ item.name }}</mu-list-item-title>
            </mu-list-item>
          </mu-list>
        </mu-bottom-sheet>

        <!-- 主题切换 -->
        <mu-button slot="right" ref="theme" flat @click="openTheme = !openTheme">
          <mu-icon value=":iconfont icon-three" />
        </mu-button>
        <mu-popover :open.sync="openTheme" :trigger="triggerTheme">
          <mu-list>
            <mu-list-item button @click="switchTheme('light')">
              <mu-list-item-title><i class="three-light" />Light</mu-list-item-title>
            </mu-list-item>
            <mu-list-item button @click="switchTheme('dark')">
              <mu-list-item-title><i class="three-dark" />Dark</mu-list-item-title>
            </mu-list-item>
          </mu-list>
        </mu-popover>
        <!-- 用户 -->
        <mu-button v-show="userInfo.isLoginNotExp" slot="right" ref="button" flat @click="openUser = !openUser">
          <div class="user">
            <mu-avatar :size="36" style="margin-right: 10px;">
              <img :src="userInfo.avatar">
            </mu-avatar>
            <mu-icon value=":iconfont icon-dow" />
          </div>
        </mu-button>
        <mu-popover v-if="userInfo.isLoginNotExp" :open.sync="openUser" :trigger="trigger">
          <mu-list>
            <mu-list-item button>
              <mu-list-item-title style="color:inherit">阿喵丶</mu-list-item-title>
            </mu-list-item>
            <mu-list-item button>
              <mu-list-item-title>个人中心</mu-list-item-title>
            </mu-list-item>
            <mu-list-item button @click="loginOut">
              <mu-list-item-title>退出登录</mu-list-item-title>
            </mu-list-item>
          </mu-list>
        </mu-popover>
      </mu-appbar>

    </div>

    <!-- 侧边栏工具按钮 -->
    <div v-if="isShowAction" class="tool">

      <!-- 如果用户已经登录了那就不展示登录和注册按钮 !user 控制 -->
      <div v-if="!userInfo.isLoginNotExp" class="tool-row">
        <mu-slide-left-transition>
          <mu-button v-show="showToolBtn" fab color="primary" @click="openLoginModal = true;showToolBtn = false;">登录</mu-button>
        </mu-slide-left-transition>
      </div>
      <div class="tool-row">
        <mu-tooltip placement="right-start" content="登录/注册/搜索">
          <mu-button fab color="info" class="search-fab" @click="showToolBtn = !showToolBtn">
            <mu-icon value=":iconfont icon-anzhuo"></mu-icon>
          </mu-button>
        </mu-tooltip>

        <mu-slide-left-transition>
          <mu-button v-show="showToolBtn" fab color="error" @click="openSearchModal = true;">搜索</mu-button>
        </mu-slide-left-transition>
      </div>

      <!-- 如果用户已经登录了那就不展示登录和注册按钮  -->
      <div v-if="!userInfo.isLoginNotExp" class="tool-row">
        <mu-slide-left-transition>
          <mu-button v-show="showToolBtn" fab color="success" @click="openRegisterModal = true;showToolBtn = false;">注册</mu-button>
        </mu-slide-left-transition>
      </div>
    </div>

    <!-- 注册弹出框 -->
    <mu-dialog title="提示！" width="360" :open.sync="openRegisterModal">
      注册功能暂未开放
      <mu-button slot="actions" flat color="primary" @click="openRegisterModal = false">关闭</mu-button>
    </mu-dialog>

    <!-- 登录弹出框组件 -->
    <login-form :toggle.sync="openLoginModal && !userInfo.isLoginNotExp"></login-form>

    <!-- 文章搜索弹出框组件 -->
    <search-form :toggle.sync="openSearchModal"></search-form>

  </div>
</template>

<script>
import theme from 'muse-ui/lib/theme';
import loginForm from './loginForm.vue';
import searchForm from './searchForm.vue';
import { GetHotTags } from 'api';
const menus = [
  {
    name: '热门',
    router: 'hot',
    icon: ':iconfont icon-hot'
  },
  {
    name: '首页',
    router: 'home',
    icon: ':iconfont icon-shouyefill'
  },
  {
    name: '文章',
    router: 'articles',
    icon: ':iconfont icon-article'
  },
  {
    name: '归档',
    router: 'archives',
    icon: ':iconfont icon-yizhuguidang'
  },
  {
    name: '分类',
    router: 'category',
    icon: ':iconfont icon-category'
  },
  {
    name: '标签',
    router: 'tags',
    icon: ':iconfont icon-tag'
  },
  {
    name: '关于',
    router: 'about',
    icon: ':iconfont icon-guanyu'
  }
];
export default {
  name: 'Header',
  components: {
    loginForm,
    searchForm
  },
  props: {
    background: {
      type: String,
      default: 'transparent'
    }
  },
  data() {
    return {
      hotTags: [],
      lightIndex: 0,
      openUser: false,
      openTheme: false,
      openWapMenu: false,
      trigger: null,
      triggerTheme: null,
      info: {
        menu: menus
      },
      isShowAction: true, // 是否显示侧边栏工具按钮
      showToolBtn: false, // 点击切换显示操作按钮

      openSearchModal: false, // 打开搜索弹框
      openLoginModal: false, // 打开登录弹框
      openRegisterModal: false // 打开注册弹框

    };
  },
  computed: {
    routeName() {
      return this.$route.name;
    },
    userInfo() {
      let userInfo = this.$store.getters['user/userInfo'];
      return userInfo;
    }
  },
  created() {
    this.getHotTags();
  },
  mounted() {
    this.theme = theme;
    theme.use('dark');
    if (this.$refs.button) {
      this.trigger = this.$refs.button.$el;
      this.triggerTheme = this.$refs.theme.$el;
    }
  },
  methods: {
    async getHotTags() {
      this.$progress.start();
      const res = await GetHotTags();
      this.$progress.done();
      if (res.statusCode < 1) {
        return this.$toast.error(res.msg);
      }
      this.hotTags = res.data;
    },
    switchTheme(theme) {
      this.theme.use(theme);
    },
    toggleWapMenu(openWapMenu) {
      this.openWapMenu = openWapMenu;
    },
    go(item, index) {
      // 多次点击当前菜单则不跳转
      if (item.router !== 'articles' && this.$route.name === item.router) {
        return;
      }
      this.lightIndex = index;
      this.$router.push({
        name: item.router
      });
    },
    toArticlePageByTagId(tagid) {
      this.$router.push({
        name: 'articles',
        query: { tagid }
      });
    },
    async loginOut() {
      this.openLoginModal = false;
      await this.$store.dispatch('user/signOut');
      this.$toast.success('登出成功');
    }
  }
};
</script>

<style scoped lang="less">
  .header {
    position: fixed;
    z-index: 1501;
    width: 100%;
    top: 0;
  }
  .user {
    display: flex;
    justify-content: space-around;
    align-items: center;
  }
  i[class^='three-'] {
    display: inline-block;
    width: 15px;
    height: 15px;
    border-radius: 15px;
    margin-right: 15px;
    position: relative;
    top: 2px;
  }
  .three-light {
    background-color: #bbdefb;
  }
  .three-dark {
    background-color: #0d47a1;
  }
  .tool {
    left: 0;
    z-index: 99999;
    position: fixed;
    bottom: 2.62rem;
    // transform: translateX(-50%);
    .tool-row {
      margin-top: 20px;
      height: 54px;
      .search-fab {
        margin-right: 20px;
        margin-left: -28px;
      }
    }
  }
</style>
