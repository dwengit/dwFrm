<template>
  <div class="login-container">
    <div class="login-box">
      <div>
        <el-form ref="loginFormRef" :model="loginForm" :rules="loginFormRules" label-width="0px" class="login-form">
          <h1 class="login-title">系统登录</h1>
          <el-form-item prop="accountCode">
            <el-input v-model="loginForm.accountCode" prefix-icon="el-icon-user" />
          </el-form-item>
          <el-form-item prop="password">
            <el-input v-model="loginForm.password" type="password" prefix-icon="el-icon-lock" @keyup.enter.native="handleLogin" />
          </el-form-item>
          <el-form-item prop="validatecode">
            <el-input v-model="loginForm.validatecode" type="validatecode" prefix-icon="iconfont icon-3702mima" class="validate-code" placeholder="请输入验证码" @keyup.enter.native="handleLogin">
              <svg-icon slot="prefix" style="width: 25px" icon-class="yanzhengma" />
            </el-input>
            <img :src="validateCodeUrl" alt="验证码" @click="refreshValidateCode">
          </el-form-item>
          <el-form-item>
            <el-button :loading="loading" size="medium" type="primary" style="width:100%;" @click.native.prevent="handleLogin">
              <span v-if="!loading">登 录</span>
              <span v-else>登 录 中...</span>
            </el-button>
            <div class="login-link">
              <span>忘记密码</span>
              <span style="float: right">注册</span>
            </div>
            <div class="other-login">
              <span class="line" />
              <span class="txt">其他登陆方式</span>
              <span class="line" />
            </div>
            <div class="other-icon">
              <ul>
                <li><img src="@/assets/login/qq.png" alt="QQ"></li>
                <li><img src="@/assets/login/wx.png" alt="微信"></li>
                <li>
                  <img src="@/assets/login/wb.png" alt="微博">
                </li>
                <li>
                  <img src="@/assets/login/github.png" alt="github">
                </li>
              </ul>
            </div>
          </el-form-item>
        </el-form>
      </div>
    </div>
  </div>
</template>

<script>
import store from '@/store';
import { getGuid, VALIDATE_KEY } from 'utils';
export default {
  data() {
    return {
      validateCodeUrl: '',
      loginForm: {
        accountCode: '',
        password: ''
      },
      redirect: undefined,
      otherQuery: {},
      loading: false,
      // 表单验证
      loginFormRules: {
        accountCode: [
          {
            required: true,
            message: '请输入用户名',
            trigger: 'blur'
          },
          {
            min: 2,
            max: 10,
            message: '长度在 2 到 10 个字符',
            trigger: 'blur'
          }
        ],
        password: [
          {
            required: true,
            message: '请输入用户密码',
            trigger: 'blur'
          },
          {
            min: 6,
            max: 18,
            message: '长度在 6 到 18 个字符',
            trigger: 'blur'
          }
        ],
        validatecode: [
          {
            required: true,
            message: '请输入验证码',
            trigger: 'blur'
          },
          {
            min: 4,
            max: 4,
            message: '长度在4字符',
            trigger: 'blur'
          }
        ]
      }
    };
  },
  watch: {
    $route: {
      handler: function(route) {
        const query = route.query;
        if (query) {
          this.redirect = query.redirect;
          this.otherQuery = this.getOtherQuery(query);
        }
      },
      immediate: true
    }
  },
  created() {
    this.refreshValidateCode();
  },
  methods: {
    getValidateKey() {
      if (!localStorage.getItem(VALIDATE_KEY)) {
        localStorage.setItem(VALIDATE_KEY, getGuid());
      }
      return localStorage.getItem(VALIDATE_KEY);
    },
    refreshValidateCode() {
      this.validateCodeUrl =
        process.env.VUE_APP_API_HOST +
        process.env.VUE_APP_API_VERSION +
        'Login/ValidateCode' +
        '?validateKey=' +
        this.getValidateKey() +
        '&t=' +
        Math.random();
    },
    handleLogin() {
      // 表单预验证
      this.$refs.loginFormRef.validate(async valid => {
        if (!valid) return false;
        this.loginForm.validateKey = this.getValidateKey();
        this.loading = true;
        this.$store
          .dispatch('user/login', this.loginForm)
          .then(async() => {
            this.$message.success('登录成功');
            await store.dispatch('permission/loadPermission');
            await store.dispatch('user/loadUserInfo');
            this.$router.push({
              path: this.redirect || '/',
              query: this.otherQuery
            });
            this.loading = false;
          })
          .catch(err => {
            this.loading = false;
            this.refreshValidateCode();
            this.$message.error(err.msg);
          });
      });
    },
    getOtherQuery(query) {
      return Object.keys(query).reduce((acc, cur) => {
        if (cur !== 'redirect') {
          acc[cur] = query[cur];
        }
        return acc;
      }, {});
    }
  }
};
</script>

<style lang="less" scoped>
  .login-container {
    background-color: #f1f1f1;
    height: 100%;
  }
  .login-box {
    width: 400px;
    margin: 0 auto;
    background-color: #fff;
    position: relative;
    top: 40%;
    transform: translateY(-50%);
    padding: 20px;
    border-radius: 26px;
    box-shadow: 0px 4px 66px #999;
    box-sizing: content-box;
  }

  .login-form {
    bottom: 60px;
    width: 100%;
    padding: 0 20px;
    box-sizing: border-box;
    .login-title {
      text-align: center;
      color: #409eff;
      letter-spacing: 8px;
      margin-left: 8px;
      font-weight: 300;
    }
    .validate-code {
      width: 200px;
    }
    .validate-code + img {
      vertical-align: middle;
      float: right;
      cursor: pointer;
    }
    .login {
      width: 100%;
    }
    .login-link {
      color: #606266;
      font-size: 14px;
      span {
        cursor: pointer;
      }
    }
    .other-login {
      color: #8c92a4;
      height: 30px;
      line-height: 30px;
      font-size: 14px;
      text-align: center;
      .line {
        display: inline-block;
        width: 128px;
        border: 1px solid #e4e7ed;
        border-top: 0;
      }
      .txt {
        color: #909399;
        vertical-align: -15%;
        margin: 0 5px;
      }
    }
    .other-icon {
      ul {
        margin: 0 auto;
        height: 30px;
        flex-direction: row;
        display: flex;
        padding: 0;
        margin-top: 15px;
        li {
          list-style: none;
          margin: 0 auto;
          cursor: pointer;
        }
      }
      img {
        height: 30px;
      }
    }
  }
  .btns {
    display: flex;
    justify-content: center;
  }
  .info {
    font-size: 13px;
    margin: 10px 15px;
  }
</style>
