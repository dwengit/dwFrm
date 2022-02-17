import axios from 'utils/http-axios.js';

/** ********** 博文管理 ************/
// 获取-博文分页
export const GetGetArticlePage = (data) => axios.get('/Blog/Article/GetArticlePage', {
  params: { ...data }
});
// 获取-博文详情
export const GetGetArticleInfo = (data) => axios.get('/Blog/Article/GetArticleInfo', {
  params: { ...data }
});
// 获取-新增
export const AddArticle = (data) => axios.post('/Blog/Article/AddArticle', {
  ...data
});
// 获取-修改
export const UpdateArticle = (data) => axios.post('/Blog/Article/UpdateArticle', {
  ...data
});
// 删除
export const DeleteArticle = (data) => axios.delete('/Blog/Article/DeleteArticle', {
  params: { ...data }
});

/** ********** 博文分类管理 ************/
// 获取-分类下拉树
export const GetCategoryTreeOption = (data) => axios.get('/Blog/Category/GetCategoryTreeOption', {
  params: { ...data }
});
// 获取-博文分类页面树
export const GetCategoryPageTree = (data) => axios.get('/Blog/Category/GetCategoryPageTree', {
  params: { ...data }
});
// 获取-新增
export const AddCategory = (data) => axios.post('/Blog/Category/Add', {
  ...data
});
// 获取-修改
export const UpdateCategory = (data) => axios.post('/Blog/Category/Update', {
  ...data
});
// 删除
export const DeleteCategory = (data) => axios.delete('/Blog/Category/Delete', {
  params: { ...data }
});

/** ********** 博文标签管理 ************/
// 获取-标签下拉
export const GetTagOption = (data) => axios.get('/Blog/Tag/GetTagOption', {
  params: { ...data }
});
// 获取-博文标签页面树
export const GetTagPage = (data) => axios.get('/Blog/Tag/GetTagPage', {
  params: { ...data }
});
// 获取-新增
export const AddTag = (data) => axios.post('/Blog/Tag/Add', {
  ...data
});
// 获取-修改
export const UpdateTag = (data) => axios.post('/Blog/Tag/Update', {
  ...data
});
// 删除
export const DeleteTag = (data) => axios.delete('/Blog/Tag/Delete', {
  params: { ...data }
});

/** ********** 博文友情链接管理 ************/
// 获取-博文友情链接页面树
export const GetLinkPage = (data) => axios.get('/Blog/Link/GetLinkPage', {
  params: { ...data }
});
// 获取-新增
export const AddLink = (data) => axios.post('/Blog/Link/Add', {
  ...data
});
// 获取-修改
export const UpdateLink = (data) => axios.post('/Blog/Link/Update', {
  ...data
});
// 删除
export const DeleteLink = (data) => axios.delete('/Blog/Link/Delete', {
  params: { ...data }
});

/** ********** 博文站点管理 ************/
// 获取-博文友情链接页面树
export const GetBlogWebSiteManageInfo = (data) => axios.get('/Blog/WebSiteManage/GetBlogWebSiteManageInfo', {
  params: { ...data }
});
// 获取-新增
export const UpdateWebSiteManageInfo = (data) => axios.post('/Blog/WebSiteManage/Update', {
  ...data
});

/** ********** 博文评论管理 ************/
// 获取-博文评论列表
export const GetArticleCommentList = (data) => axios.get('/Blog/Comment/GetArticleCommentList', {
  params: { ...data }
});
// 提交博文评论/回复
export const SubComment = (data) => axios.post('/Blog/Comment/SubComment', {
  ...data
});
// 点赞
export const LikeComment = (data) => axios.put('/Blog/Comment/Like', {
  ...data
});
// 删除
export const DeleteComment = (data) => axios.delete('/Blog/Comment/Delete', {
  params: { ...data }
});
