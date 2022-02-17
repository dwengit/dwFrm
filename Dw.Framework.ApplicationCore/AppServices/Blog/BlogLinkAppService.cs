using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog
{
    /// <summary>
    /// 友情连接服务类
    /// </summary>
    public class BlogLinkAppService : IBlogLinkAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogLinkRepository _blogLinkRepository;
        private readonly ISysFileBusinesRepository _sysFileBusinesRepository;

        public BlogLinkAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogLinkRepository blogLinkRepository, ISysFileBusinesRepository sysFileBusinesRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogLinkRepository = blogLinkRepository;
            this._sysFileBusinesRepository = sysFileBusinesRepository;
        }
        /// <summary>
        /// 获取博文友情链接页面
        /// </summary>
        /// <param name="LinkTitle"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<LinkPage>> GetLinkPage(string linkTitle)
        {
            var list = await _blogLinkRepository.QueryNoTracking(w => !w.IsDelete)
                        .WhereIf(w => w.LinkTitle.Contains(linkTitle), !string.IsNullOrWhiteSpace(linkTitle))
                        .OrderByDescending(w => w.SortNO)
                        .ThenByDescending(w => w.CreateTime)
                        .ToListAsync();
            var res = _mapper.Map<List<LinkPage>>(list);
            if (res.Any())
            {
                var ids = list.Select(w => w.Id).ToList();
                var files = _sysFileBusinesRepository.GetFileUploadDto(ConstFileBusinesType.BLOG_LINK_BACKIMG, null);
                foreach (var item in res)
                {
                    item.LinkImage = files.Where(w => w.BusinessId == item.Id).ToList();
                }
            }
            return res;
        }
        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task Add(SaveLinkInput input)
        {
            _unitOfWork.BeginTransaction();
            await VerifyLinkNameRepeat(input.LinkTitle, input.Id);
            var data = _mapper.Map<BlogLink>(input);
            await _blogLinkRepository.InsertAsync(data);
            await _sysFileBusinesRepository.SetFilesStatus(input.Id);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.CommitTransaction();
        }
        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task Update(SaveLinkInput input)
        {
            _unitOfWork.BeginTransaction();
            await VerifyLinkNameRepeat(input.LinkTitle, input.Id);
            var updata = _blogLinkRepository.Get(input.Id);
            if (updata == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            _mapper.Map(input, updata);
            _blogLinkRepository.Update(updata);
            await _sysFileBusinesRepository.SetFilesStatus(input.Id);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.CommitTransaction();
        }
        /// <summary>
        /// 删除博文友情链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            _unitOfWork.BeginTransaction();
            _blogLinkRepository.Delete(w => w.Id == id);
            //删除博文相关图片
            await _sysFileBusinesRepository.DeleteFileByBusinesId(id);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.CommitTransaction();
        }
        /// <summary>
        /// 校验博文友情链接名称重复将会引发异常
        /// </summary>
        /// <param name="LinkTitle"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task VerifyLinkNameRepeat(string linkTitle, Guid id)
        {
            if (await _blogLinkRepository.QueryNoTracking().WhereIf(w => w.Id != id, id != null && id != Guid.Empty)
                .AnyAsync(w => w.LinkTitle.Equals(linkTitle, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "友情链接名称不能重复");
            }
        }
    }
}
