using Dw.Framework.Model.Dto.Blog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogWebSiteManageAppService
    {
        Task<WebSiteManageDto> GetBlogWebSiteManageInfo();
        Task Save(SaveWebSiteManageInput input);
    }
}
