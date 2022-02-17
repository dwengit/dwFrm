using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1.Blog
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    [ApiController]
    public class LinkController : ApiControllersBase
    {
        private readonly IBlogLinkAppService _blogLinkAppService;

        public LinkController(IBlogLinkAppService blogLinkAppService)
        {
            this._blogLinkAppService = blogLinkAppService;
        }
        /// <summary>
        /// 获取友情链接页面树
        /// </summary>
        /// <param name="linkTitle"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.LinkRead)]
        public async Task<Respbase<List<LinkPage>>> GetLinkPage(string linkTitle)
        {
            var res = await _blogLinkAppService.GetLinkPage(linkTitle);
            return RespOk(res);
        }
        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.LinkAdd)]
        [Log(Title = "友情链接", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> Add(SaveLinkInput input)
        {
            await _blogLinkAppService.Add(input);
            return RespOk();
        }
        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.LinkEdit)]
        [Log(Title = "友情链接", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> Update(SaveLinkInput input)
        {
            await _blogLinkAppService.Update(input);
            return RespOk();
        }
        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_Blog.LinkDelete)]
        [Log(Title = "友情链接", BusinessType = BusinessType.DELETE)]
        public async Task<Respbase> Delete(Guid id)
        {
            await _blogLinkAppService.Delete(id);
            return RespOk();
        }
    }
}
