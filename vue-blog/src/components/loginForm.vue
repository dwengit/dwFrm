<template>
  <div>
    <mu-dialog title="登录" width="500" max-width="90%" :esc-press-close="false" :overlay-close="false" :open.sync="toggle">
      <mu-form ref="form" :model="validateForm">
        <mu-form-item label="Email" prop="accountCode" :rules="accountCodeRules">
          <mu-text-field v-model.trim="validateForm.accountCode" prop="accountCode"></mu-text-field>
        </mu-form-item>

        <mu-form-item label="密码" prop="password" :rules="passwordRules">
          <mu-text-field v-model.trim="validateForm.password" type="password" prop="password"></mu-text-field>
        </mu-form-item>

        <mu-form-item label="验证码" prop="validatecode" :rules="validatecodeRules">
          <mu-text-field v-model.trim="validateForm.validatecode" placeholder="区分大小写" prop="validatecode">
            <div class="validatecode" @click="getCaptcha" v-html="validatecode"></div>
            <img :src="validateCodeUrl" @click="getCaptcha()">
          </mu-text-field>

        </mu-form-item>
      </mu-form>

      <mu-button slot="actions" flat href="/api/v1/web/github/login">
        <mu-avatar style="margin-right:10px" size="30">
          <img src="/images/github.png" alt>
        </mu-avatar>
      </mu-button>

      <mu-button slot="actions" flat small @click="clear">取消</mu-button>
      <mu-button slot="actions" flat small color="primary" @click="submit">登录</mu-button>
    </mu-dialog>
  </div>
</template>
<script>
import { LoginApi } from 'api';
import { getGuid, VALIDATE_KEY } from 'utils';

export default {
  props: {
    toggle: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      validateCodeUrl: '',
      validatecode: '',
      accountCodeRules: [{ validate: val => !!val, message: '邮箱必填！' }],
      passwordRules: [{ validate: val => !!val, message: '密码必填！' }],
      validatecodeRules: [{ validate: val => !!val, message: '请输入验证码' }],
      validateForm: {
        accountCode: '',
        password: '',
        validatecode: ''
      }
    };
  },
  created() {
    this.getCaptcha();
  },
  methods: {
    getCaptcha() {
      this.validateCodeUrl =
        process.env.VUE_APP_API_HOST +
        process.env.VUE_APP_API_VERSION +
        'Login/ValidateCode' +
        '?validateKey=' +
        this.getValidateKey() +
        '&t=' +
        Math.random();
    },
    getValidateKey() {
      if (!localStorage.getItem(VALIDATE_KEY)) {
        localStorage.setItem(VALIDATE_KEY, getGuid());
      }
      return localStorage.getItem(VALIDATE_KEY);
    },
    submit() {
      this.$refs.form.validate().then(async result => {
        if (result) {
          this.validateForm.validateKey = this.getValidateKey();
          const res = await LoginApi(this.validateForm);
          console.log(res);
          if (res.statusCode < 1) {
            this.getCaptcha();
            return this.$toast.error(res.msg);
          }
          await this.$store.dispatch('user/login', res.data);
          this.$toast.success('登录成功');
        }
      });
    },
    clear() {
      this.$refs.form.clear();
      this.validateForm = {
        accountCode: '',
        nickName: '',
        password: '',
        confirmPassword: '',
        introduction: '',
        validatecode: ''
      };
      this.$emit('update:toggle', false);
    }
  }
};
</script>
<style lang="less" scoped>
  .validatecode {
    cursor: pointer;
    /deep/ svg {
      vertical-align: middle;
    }
  }
</style>
