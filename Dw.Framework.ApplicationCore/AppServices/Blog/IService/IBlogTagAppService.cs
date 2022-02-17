using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogTagAppService
    {
        IList<DefaultSelectOption> GetTagOption(EnumGetTagType tagType = EnumGetTagType.ALL);
        Task<List<TagPage>> GetTagPage(string tagTitle);
        Task Save(SaveTagInput input);
        Task Delete(Guid id);
    }
}
