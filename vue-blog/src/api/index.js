import axios from 'utils/http-axios.js';
// 登录获取Token
export const LoginApi = (data) => axios.post('/Login/Login', {
  ...data
});
// 获取热门标签
export const GetHotTags = (data) => axios.get('/Blog/Config/GetHotTags', {
  params: { ...data }
});
// 获取前端博文列表
export const GetArticleShowPage = (data) => axios.get('/Blog/Article/GetArticleShowPage', {
  params: { ...data }
});
// 获取前端博文详情
export const GetArticleDetail = (data) => axios.get('/Blog/Article/GetArticleDetail', {
  params: { ...data }
});
// 获取博文评论列表
export const GetArticleCommentList = (data) => axios.get('/Blog/Article/GetArticleCommentList', {
  params: { ...data }
});
// 提交博文评论/回复
export const SubComment = (data) => axios.post('/Blog/Article/SubComment', {
  ...data
});
// 点赞
export const LikeComment = (data) => axios.put('/Blog/Article/Like', {
  ...data
});
// 删除
export const DeleteComment = (data) => axios.delete('/Blog/Article/Delete', {
  params: { ...data }
});
// 获取博文分类页面树
export const GetCategoryTree = (data) => axios.get('/Blog/Article/GetCategoryTree', {
  params: { ...data }
});
// 获取所有标签
export const GetTags = (data) => axios.get('/Blog/Article/GetTags', {
  params: { ...data }
});
// 获取所有标签
export const GetArchives = (data) => axios.get('/Blog/Article/GetArchives', {
  params: { ...data }
});
