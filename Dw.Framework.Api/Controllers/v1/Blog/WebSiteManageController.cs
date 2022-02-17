using Dw.Framework.Admin.Api.Auth;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Shared.Enums;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Web.Infrastructure;
using Dw.Framework.Web.Infrastructure.Auth;
using Dw.Framework.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1.Blog
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    [ApiController]
    public class WebSiteManageController : ApiControllersBase
    {
        private readonly IBlogWebSiteManageAppService _blogWebSiteManageAppService;
        public WebSiteManageController(IBlogWebSiteManageAppService blogWebSiteManageAppService)
        {
            this._blogWebSiteManageAppService = blogWebSiteManageAppService;
        }
        /// <summary>
        /// 获取站点详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilter(_Blog.ManageRead)]
        public async Task<Respbase<WebSiteManageDto>> GetBlogWebSiteManageInfo()
        {
            var res = await _blogWebSiteManageAppService.GetBlogWebSiteManageInfo();
            return RespOk(res);
        }
        /// <summary>
        /// 更新站点信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionFilter(_Blog.ManageEdit)]
        [Log(Title = "站点管理", BusinessType = BusinessType.UPDATE)]
        public async Task<Respbase> Update(SaveWebSiteManageInput input)
        {
            await _blogWebSiteManageAppService.Save(input);
            return RespOk();
        }
    }
}
