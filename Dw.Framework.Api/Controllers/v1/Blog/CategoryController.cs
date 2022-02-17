using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
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
    public class CategoryController : ApiControllersBase
    {
        private readonly IBlogCategoryAppService _blogCategoryAppService;

        public CategoryController(IBlogCategoryAppService blogCategoryAppService)
        {
            this._blogCategoryAppService = blogCategoryAppService;
        }
        /// <summary>
        /// 获取博文下拉树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.CategoryRead)]
        public async Task<Respbase<List<DefaultTreeOption>>> GetCategoryTreeOption()
        {
            var res = await _blogCategoryAppService.GetCategoryTreeOption();
            return RespOk(res);
        }
        /// <summary>
        /// 获取博文分类页面树
        /// </summary>
        /// <param name="categoryTitle"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.CategoryRead)]
        public async Task<Respbase<List<CategoryPageTree>>> GetCategoryPageTree(string categoryTitle)
        {
            var res = await _blogCategoryAppService.GetCategoryPageTree(categoryTitle);
            return RespOk(res);
        }
        /// <summary>
        /// 新增博文分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.CategoryAdd)]
        [Log(Title = "博文分类", BusinessType = BusinessType.INSERT)]
        public async Task<Respbase> Add(SaveCategoryInput input)
        {
            await _blogCategoryAppService.Save(input);
            return RespOk();
        }
        /// <summary>
        /// 更新博文分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.CategoryEdit)]
        [Log(Title = "博文分类", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> Update(SaveCategoryInput input)
        {
            await _blogCategoryAppService.Save(input);
            return RespOk();
        }
        /// <summary>
        /// 删除博文分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [PermissionFilter(_Blog.CategoryEdit)]
        [Log(Title = "博文分类", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> Delete(Guid id)
        {
            await _blogCategoryAppService.Delete(id);
            return RespOk();
        }
    }
}
