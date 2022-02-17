using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogCategoryAppService
    {
        Task<List<DefaultTreeOption>> GetCategoryTreeOption();
        Task<List<CategoryPageTree>> GetCategoryPageTree(string categoryTitle);
        Task Save(SaveCategoryInput input);
        Task Delete(Guid id);
    }
}
