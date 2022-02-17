using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
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
    /// 云标签服务类
    /// </summary>
    public class BlogTagAppService : IBlogTagAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IBlogArticleTagRepository _blogArticleTagRepository;
        public BlogTagAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogTagRepository blogTagRepository, IBlogArticleTagRepository blogArticleTagRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogTagRepository = blogTagRepository;
            this._blogArticleTagRepository = blogArticleTagRepository;
        }
        public IList<DefaultSelectOption> GetTagOption(EnumGetTagType tagType = EnumGetTagType.ALL)
        {
            var tagOptions = _blogTagRepository.QueryNoTracking().Where(w => !w.IsDelete)
                .WhereIf(w => w.IsQuickNav, tagType == EnumGetTagType.HOT).OrderByDescending(w => w.SortNO).Select(s => new DefaultSelectOption()
            {
                Label = s.TagTitle,
                Value = s.Id.ToString()
            }).ToList();
            return tagOptions;
        }
        /// <summary>
        /// 获取博文标签页面
        /// </summary>
        /// <param name="TagTitle"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<TagPage>> GetTagPage(string tagTitle)
        {
            var list = await _blogTagRepository.QueryNoTracking(w => !w.IsDelete)
                        .WhereIf(w => w.TagTitle.Contains(tagTitle), !string.IsNullOrWhiteSpace(tagTitle))
                        .OrderByDescending(w => w.SortNO)
                        .ThenByDescending(w => w.CreateTime)
                        .ToListAsync();
            
            return _mapper.Map<List<TagPage>>(list);
        }
        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task Save(SaveTagInput input)
        {
            await VerifyTagNameRepeat(input.TagTitle, input.Id);
            if (input.Id != null && input.Id != Guid.Empty)
            {
                var updata = _blogTagRepository.Get(input.Id);
                if (updata == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
                }
                _mapper.Map(input, updata);
                _blogTagRepository.Update(updata);
            }
            else
            {
                var data = _mapper.Map<BlogTag>(input);
                await _blogTagRepository.InsertAsync(data);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 删除博文标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            _blogTagRepository.Delete(w => w.Id == id);
            _blogArticleTagRepository.Delete(w => w.TagId == id);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 校验博文标签名称重复将会引发异常
        /// </summary>
        /// <param name="TagTitle"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task VerifyTagNameRepeat(string tagTitle, Guid id)
        {
            if (await _blogTagRepository.QueryNoTracking().WhereIf(w => w.Id != id, id != null && id != Guid.Empty)
                .AnyAsync(w => w.TagTitle.Equals(tagTitle, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "标签名称不能重复");
            }
        }
    }
}
