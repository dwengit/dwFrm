using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Blog.Front;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Enums;
using Dw.Framework.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Blog.Api.v1.Blog
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    public class ArticleController : ApiControllersBase
    {
        private readonly IBlogArticleAppService _blogArticleAppService;
        private readonly IBlogCommentAppService _blogCommentAppService;
        private readonly IBlogCategoryAppService _blogCategoryAppService;
        private readonly IBlogTagAppService _blogTagAppService;

        public ArticleController(IBlogArticleAppService blogArticleAppService, IBlogCommentAppService blogCommentAppService, IBlogCategoryAppService blogCategoryAppService, IBlogTagAppService blogTagAppService)
        {
            this._blogArticleAppService = blogArticleAppService;
            this._blogCommentAppService = blogCommentAppService;
            this._blogCategoryAppService = blogCategoryAppService;
            this._blogTagAppService = blogTagAppService;
        }
        /// <summary>
        /// 当前登录的账户
        /// </summary>
        public string accountCode
        {
            get { return HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid)?.Value; }
        }

        /// <summary>
        /// 获取前端博客列表
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Respbase<Page<ArticleShowPage>>> GetArticleShowPage(string articleTitle, Guid? categoryId, Guid? tagId, int pageIndex, int pageSize)
        {
            var res = await _blogArticleAppService.GetArticleShowPage(articleTitle, categoryId, tagId, pageIndex, pageSize,accountCode);
            return RespOk(res);
        }
        /// <summary>
        /// 获取前端博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Respbase<ArticleDetail>> GetArticleDetail(Guid id)
        {
            var res = await _blogArticleAppService.GetArticleDetail(id,accountCode);
            return RespOk(res);
        }
        /// <summary>
        /// 获取博文评论列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Respbase<IList<CommentList>>> GetArticleCommentList(Guid articleId)
        {
            string ip = HttpContextExtension.GetClientUserIp(HttpContext);
            var res = await _blogCommentAppService.GetArticleCommentList(articleId, ip);
            return RespOk(res);
        }
        /// <summary>
        /// 提交博文评论/回复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Respbase> SubComment(SubCommentInput input)
        {
            string ip = HttpContextExtension.GetClientUserIp(HttpContext);
            string accountCode = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid)?.Value;
            await _blogCommentAppService.SubComment(input, ip, accountCode);
            return RespOk();
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<Respbase> Like(CommentLike input)
        {
            string ip = HttpContextExtension.GetClientUserIp(HttpContext);
            await _blogCommentAppService.Like(input.CommentId, ip);
            return RespOk();
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Respbase> Delete(Guid commentId)
        {
            await _blogCommentAppService.Delete(commentId);
            return RespOk();
        }
        /// <summary>
        /// 获取博文分类页面树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Respbase<List<DefaultTreeOption>>> GetCategoryTree()
        {
            var res = await _blogCategoryAppService.GetCategoryTreeOption();
            return RespOk(res);
        }
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Respbase<IList<DefaultSelectOption>> GetTags()
        {
            var res = _blogTagAppService.GetTagOption();
            return RespOk(res);
        }
        // <summary>
        /// 获取博文归档数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Respbase<IList<BlogArchives>>> GetArchives()
        {
            var res = await _blogArticleAppService.GetArchives(accountCode);
            return RespOk(res);
        }
    }
}
