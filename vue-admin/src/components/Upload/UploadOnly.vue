<template>
  <div>
    <el-upload
      v-loading="uploadLoading"
      class="upload-demo"
      action="string"
      :http-request="uploadFile"
      :on-preview="handlePreview"
      :on-remove="handleRemove"
      :file-list="fileList"
      :disabled="isReadOnly"
      v-bind="$attrs"
    >
      <el-button v-if="!isReadOnly" size="small" type="primary">点击上传</el-button>
      <div v-if="!isReadOnly" slot="tip" class="el-upload__tip">文件大小不超过{{ fileMaxSize }}MB</div>
    </el-upload>
  </div>
</template>

<script>
import { PostOnlyFile, RemoveUploadOnlyFile } from 'api/upload';
export default {
  model: {
    prop: 'value',
    event: 'onUpload'
  },
  props: {
    value: {
      type: Array,
      default: function() {
        return [];
      }
    },
    fileMaxSize: {
      type: Number,
      default: 5
    },
    isReadOnly: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      id: '',
      uploadLoading: false
    };
  },
  computed: {
    fileList: {
      get() {
        return this.value || [];
      },
      set(val) {
        this.$emit('onUpload', val);
      }
    }
  },
  methods: {
    uploadFile(item) {
      const isMax = item.file.size / 1024 / 1024 < this.fileMaxSize;
      if (!isMax) {
        this.$message.error(`上传文件大小不能超过 ${this.fileMaxSize}MB!`);
        return;
      }
      this.uploadLoading = true;
      const formData = new FormData();
      formData.append('UpFile', item.file);

      PostOnlyFile(formData).then(res => {
        this.uploadLoading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('上传成功');
        this.fileList.push(res.data);
        // this.$emit('onUpload', res.data.relativePath);
      }).catch(err => {
        this.uploadLoading = false;
        this.$message.error(err.msg);
      });
    },
    handleRemove(file, fileList) {
      this.$confirm(`是否确认删除文件名称为"${file.name}"的文件项?`, '警告', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(async() => {
        this.uploadLoading = true;
        const res = await RemoveUploadOnlyFile({ objectName: file.objectName });
        this.uploadLoading = false;
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.fileList = this.fileList.filter(f => f.objectName !== file.objectName);
        this.msgSuccess('删除成功!');
      });
    },
    handlePreview(file) {
      console.log(file);
    }
  }
};
</script>

<style lang="scss" scoped>

</style>
