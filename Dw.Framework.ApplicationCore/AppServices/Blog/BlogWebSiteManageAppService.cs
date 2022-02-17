using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog
{
    /// <summary>
    /// 博文站点服务类
    /// </summary>
    public class BlogWebSiteManageAppService : IBlogWebSiteManageAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogWebSiteManageRepository _blogWebSiteManageRepository;

        public BlogWebSiteManageAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogWebSiteManageRepository blogWebSiteManageRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogWebSiteManageRepository = blogWebSiteManageRepository;
        }
        public async Task<WebSiteManageDto> GetBlogWebSiteManageInfo()
        {
            var data = await _blogWebSiteManageRepository.FirstOrDefaultAsync(w => !w.IsDelete);
            var res = new WebSiteManageDto();
            if (data != null)
            {
                res.Id = data.Id;
                res.WebSitName = data.WebSitName;
                if (!string.IsNullOrEmpty(data.BackImage))
                {
                    res.BackImage.Add(new FileOnlyUploadDto()
                    {
                        Name = data.BackImage.Split('/')[1],
                        objectName = data.BackImage,
                        Url = $"{GlobalConfig.PreviewEndpoint}/{GlobalConfig.BucketName}/{data.BackImage}"
                    });
                }
            }
            return res;
        }

        public async Task Save(SaveWebSiteManageInput input)
        {
            if (input.Id != null && input.Id != Guid.Empty)
            {
                var updata = _blogWebSiteManageRepository.Get(input.Id);
                if (updata == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
                }
                _mapper.Map(input, updata);
                _blogWebSiteManageRepository.Update(updata);
            }
            else
            {
                var data = _mapper.Map<BlogWebSiteManage>(input);
                await _blogWebSiteManageRepository.InsertAsync(data);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
