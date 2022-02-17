using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1.Blog
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    [ApiController]
    public class TagController : ApiControllersBase
    {
        private readonly IBlogTagAppService _blogTagAppService;

        public TagController(IBlogTagAppService blogTagAppService)
        {
            this._blogTagAppService = blogTagAppService;
        }
        [HttpGet]
        [PermissionFilter(_Blog.TagRead)]
        public Respbase<IList<DefaultSelectOption>> GetTagOption()
        {
            var res = _blogTagAppService.GetTagOption();
            return RespOk(res);
        }
        /// <summary>
        /// 获取博文标签页面树
        /// </summary>
        /// <param name="tagTitle"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.TagRead)]
        public async Task<Respbase<List<TagPage>>> GetTagPage(string tagTitle)
        {
            var res = await _blogTagAppService.GetTagPage(tagTitle);
            return RespOk(res);
        }
        /// <summary>
        /// 新增博文标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.TagAdd)]
        [Log(Title = "博文标签", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> Add(SaveTagInput input)
        {
            await _blogTagAppService.Save(input);
            return RespOk();
        }
        /// <summary>
        /// 更新博文标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.TagEdit)]
        [Log(Title = "博文标签", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> Update(SaveTagInput input)
        {
            await _blogTagAppService.Save(input);
            return RespOk();
        }
        /// <summary>
        /// 删除博文标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_Blog.TagDelete)]
        [Log(Title = "博文标签", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> Delete(Guid id)
        {
            await _blogTagAppService.Delete(id);
            return RespOk();
        }
    }
}
