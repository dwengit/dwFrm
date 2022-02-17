<!--  -->
<template>
  <el-button v-if="isShow" ref="refbtn" v-waves :type="btnType" :class="btnClass" v-bind="$attrs" v-on="$listeners">
    <svg-icon :icon-class="iconClass" class-name="svg-btn" />
    <span>&nbsp;{{ btnName }}</span>
  </el-button>
</template>

<script>
import { getPermission } from '@/utils/permission';
import { primary, success, danger } from '@/styles/element-variables.scss';
import { getLightColor } from '@/utils/index';

const btnTypes = new Map();
// btnTypes.set('search', success);
btnTypes.set('Read', primary);
btnTypes.set('Add', primary);
btnTypes.set('Edit', success);
btnTypes.set('Delete', danger);
btnTypes.set('Clear', danger);
btnTypes.set('AssignPermission', '#8338ec');
btnTypes.set('ResetPwd', '#ff006e');
export default {
  components: {},
  props: {
    code: {
      type: String,
      default: ''
    },
    type: {
      type: String,
      default: ''
    },
    spacing: {
      type: Boolean,
      default: false
    },
    title: {
      type: String,
      default: ''
    },
    icon: {
      type: String,
      default: ''
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      isShow: false,
      iconClass: '',
      btnName: '',
      btnType: 'success',
      btnClass: ''
    };
  },
  watch: {
    disabled() {
      this.loadBtnStyle();
    }
  },
  created() {
    if (this.spacing) {
      this.btnClass = 'spacing';
    }
    const permission = getPermission(this.code);
    if (permission) {
      this.isShow = true;
      this.iconClass = this.icon || permission.permissionIcon;
      this.btnName = this.title || permission.permissionName;
    }
  },
  mounted() {
    this.loadBtnStyle();
  },
  methods: {
    loadBtnStyle() {
      // 设置按钮颜色
      if (this.isShow) {
        if (this.type && this.type !== 'text') {
          this.btnType = this.type;
          return;
        }
        const codeSplist = this.code.split('.');
        const lastCode = codeSplist[codeSplist.length - 1];
        const el = this.$refs.refbtn.$el;
        if (btnTypes.has(lastCode)) {
          let colorValue = btnTypes.get(lastCode);
          if (this.type !== 'text') {
            let borderColor = colorValue;
            if (this.disabled) {
              colorValue = getLightColor(colorValue, 0.5);
              borderColor = getLightColor(colorValue, 0.8);
              el.style.cursor = 'no-drop';
              el.disabled = true;
            } else {
              el.style.cursor = 'pointer';
              el.disabled = false;
              borderColor = getLightColor(colorValue, 0.3);
            }
            el.style.backgroundColor = colorValue;
            el.style.border = '1px solid ' + borderColor;
          } else {
            this.btnType = this.type;
            el.style.color = colorValue;
            this.btnClass = 'table-inner-control-btn';
          }
        }
      }
    }
  }
};
</script>
<style lang="scss" scoped>
  .spacing {
    display: inline-block;
    margin-right: 6px;
  }

  // .table-inner-control-btn {
  //   ::v-deep .el-button {
  //     padding: 0px !important;
  //   }
  // }
</style>
