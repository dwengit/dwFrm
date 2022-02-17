import axios from 'utils/http-axios.js';

// 简单文件上传
export const PostOnlyFile = (formData) => axios.post('/Upload/PostOnlyFile', formData, {
  'Content-type': 'multipart/form-data'
});
// 业务文件上传
export const PostFile = (formData) => axios.post('/Upload/PostFile', formData, {
  'Content-type': 'multipart/form-data'
});
// 业务文件删除
export const RemoveUploadFile = (data) => axios.delete('/Upload/RemoveUploadFile', {
  params: { ...data }
});
// 直接删除对象存储对象
export const RemoveUploadOnlyFile = (data) => axios.delete('/Upload/RemoveUploadOnlyFile', {
  params: { ...data }
});
