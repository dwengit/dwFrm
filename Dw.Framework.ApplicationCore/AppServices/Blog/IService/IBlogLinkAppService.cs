using Dw.Framework.Model.Dto.Blog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogLinkAppService
    {
        Task<List<LinkPage>> GetLinkPage(string tagTitle);
        Task Add(SaveLinkInput input);
        Task Update(SaveLinkInput input);
        Task Delete(Guid id);
    }
}
