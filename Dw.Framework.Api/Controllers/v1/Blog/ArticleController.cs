using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Enums;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1.Blog
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ApiControllersBase
    {
        private readonly IBlogArticleAppService _blogArticleAppService;

        public ArticleController(IBlogArticleAppService blogArticleAppService)
        {
            this._blogArticleAppService = blogArticleAppService;
        }
        /// <summary>
        /// 获取-博文分页
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="status"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagIds"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.ArticleRead)]
        public async Task<Respbase<Page<ArticlePage>>> GetArticlePage(string articleTitle, int? status, Guid? categoryId, string tagIds, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            BlogArticleStatus? stateEnum = null;
            if (Enum.IsDefined(typeof(BlogArticleStatus), status))
            {
                stateEnum = (BlogArticleStatus)status;
            }
            var ids = tagIds?.Split(',').Select(s => Guid.Parse(s)).ToList();
            var res = await _blogArticleAppService.GetArticlePage(articleTitle,stateEnum, categoryId, ids, beginTime, endTime, pageIndex, pageSize);
            return RespOk(res);
        }
        /// <summary>
        /// 获取-博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.ArticleRead)]
        public async Task<Respbase<ArticleDto>> GetArticleInfo(Guid id)
        {
            var res = await _blogArticleAppService.GetArticleInfo(id);
            return RespOk(res);
        }
        /// <summary>
        /// 新增-博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.ArticleAdd)]
        [Log(Title = "博文管理", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> AddArticle(SaveArticleInput saveArticleInput)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Sid).Value;
            var res = await _blogArticleAppService.AddArticle(saveArticleInput, Guid.Parse(userId));
            return RespOk();
        }
        /// <summary>
        /// 修改-博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.ArticleEdit)]
        [Log(Title = "博文管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> UpdateArticle(SaveArticleInput saveArticleInput)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Sid).Value;

            var res = await _blogArticleAppService.UpdateArticle(saveArticleInput, Guid.Parse(userId));
            return RespOk();
        }
        /// <summary>
        /// 删除-博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_Blog.ArticleDelete)]
        [Log(Title = "博文管理", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> DeleteArticle(Guid id)
        {
            var res = await _blogArticleAppService.DeleteArticle(id);
            return RespOk();
        }
    }
}
