<template>
  <div style="padding: 50px 0;">
    <el-form ref="addOrEditForm" :model="saveForm" label-width="120px">
      <el-row>
        <el-col :span="12">
          <el-form-item label="站点名称" prop="linkTitle">
            <el-input v-model="saveForm.webSitName" placeholder="请输入站点名称" clearable />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item label="封面图片" prop="backImage">
            <upload-only v-model="saveForm.backImage" :file-max-size="1" list-type="picture" :limit="1" />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <div slot="footer" class="dialog-footer" style="text-align: center;">
      <el-button type="primary" @click="saveAddOrEditForm()">确 定</el-button>
    </div>
  </div>
</template>

<script>
import UploadOnly from 'components/Upload/UploadOnly';
import { GetBlogWebSiteManageInfo, UpdateWebSiteManageInfo } from 'api/blog';
export default {
  name: 'Manage',
  components: { UploadOnly },
  data() {
    return {
      saveForm: {
        backImage: [],
        webSitName: '',
        id: ''
      }
    };
  },
  created() {
    this.getInfo();
  },
  methods: {
    getInfo() {
      GetBlogWebSiteManageInfo().then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.saveForm = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    saveAddOrEditForm() {
      if (!this.saveForm.backImage.length) {
        this.$message.error('请上传图片');
        return;
      }
      const rloading = this.openLoading();
      var saveData = JSON.parse(JSON.stringify(this.saveForm));
      saveData.backImage = saveData.backImage[0].objectName;
      UpdateWebSiteManageInfo(saveData).then(res => {
        rloading.close();
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.msgSuccess('成功!');
        this.getInfo();
      }).catch(err => {
        rloading.close();
        this.$message.error(err.msg);
      });
    }
  }
};
</script>

<style lang="scss" scoped>

</style>
