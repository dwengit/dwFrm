using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Enums;
using Dw.Framework.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dw.Framework.Blog.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Blog/[controller]/[action]")]
    public class ConfigController : ApiControllersBase
    {
        private readonly IBlogTagAppService _blogTagAppService;
        public ConfigController(IBlogTagAppService blogTagAppService)
        {
            this._blogTagAppService = blogTagAppService;
        }
        /// <summary>
        /// 获取热门标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Respbase<IList<DefaultSelectOption>> GetHotTags()
        {
            var res = _blogTagAppService.GetTagOption(EnumGetTagType.HOT);
            return RespOk(res);
        }
    }
}
