using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Infrastructure.Utility.WebApiUtility;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
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
    public class CommentController : ApiControllersBase
    {
        private readonly IBlogCommentAppService _blogCommentAppService;

        public CommentController(IBlogCommentAppService blogCommentAppService)
        {
            this._blogCommentAppService = blogCommentAppService;
        }
        /// <summary>
        /// 获取博文评论列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.CommentRead)]
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
        [PermissionFilter(_Blog.CommentAdd)]
        [Log(Title = "博文评论", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> SubComment(SubCommentInput input)
        {
            string ip = HttpContextExtension.GetClientUserIp(HttpContext);
            string accountCode = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid).Value;
            await _blogCommentAppService.SubComment(input, ip, accountCode);
            return RespOk();
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
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
        [PermissionFilter(_Blog.CommentDelete)]
        [Log(Title = "博文评论", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> Delete(Guid commentId)
        {
            await _blogCommentAppService.Delete(commentId);
            return RespOk();
        }

    }
}
