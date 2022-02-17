using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Blog.Front;
using Dw.Framework.Model.Dto.Shared;
using Dw.Framework.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog
{
    /// <summary>
    /// 博文服务类
    /// </summary>
    public class BlogArticleAppService : IBlogArticleAppService
    {
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IBlogArticleTagRepository _blogArticleTagRepository;
        private readonly ISysFileBusinesRepository _sysFileBusinesRepository;
        private readonly ISysUserRepository _sysUserRepository;

        public BlogArticleAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogArticleRepository blogArticleRepository, IBlogCategoryRepository blogCategoryRepository, IBlogTagRepository blogTagRepository, ISysFileBusinesRepository sysFileBusinesRepository, ISysUserRepository sysUserRepository, IBlogArticleTagRepository blogArticleTagRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogArticleRepository = blogArticleRepository;
            this._blogCategoryRepository = blogCategoryRepository;
            this._blogTagRepository = blogTagRepository;
            this._blogArticleTagRepository = blogArticleTagRepository;
            this._sysFileBusinesRepository = sysFileBusinesRepository;
            this._sysUserRepository = sysUserRepository;
        }
        
        #region 博客前端方法
        /// <summary>
        /// 获取前端博文列表
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Page<ArticleShowPage>> GetArticleShowPage(string articleTitle, Guid? categoryId, Guid? tagId, int pageIndex, int pageSize, string accountCode)
        {
            var articleIds = new List<Guid>();
            if (tagId != null)
            {
                articleIds = _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && w.TagId == tagId).Select(s => s.ArticleId).Distinct().ToList();
            }
            var articlQuery = _blogArticleRepository
                             .QueryNoTracking(w => !w.IsDelete && w.ArticleStatus == BlogArticleStatus.PUBLISHED)
                             .WhereIf(w => w.ArticleTitle.Contains(articleTitle), !string.IsNullOrEmpty(articleTitle))
                             .WhereIf(w => articleIds.Contains(w.Id), articleIds.Any())
                             .WhereIf(w => w.Auth == 0, string.IsNullOrEmpty(accountCode));

            // 博文分类暂时只做2级
            if (categoryId != null)
            {
                var category = _blogCategoryRepository.QueryNoTracking(w => !w.IsDelete && w.Id == categoryId).FirstOrDefault();
                if (category != null && category.ParentId == Guid.Empty)
                {
                    var ids = _blogCategoryRepository.QueryNoTracking(w => w.ParentId == category.Id).Select(w => w.Id).ToList();
                    articlQuery = articlQuery.Where(w => w.CategoryId == categoryId || ids.Contains(w.Id));
                }
                else
                {
                    articlQuery = articlQuery.Where(w => w.CategoryId == categoryId);
                }
            }
            var query = from articl in articlQuery
                        join user in _sysUserRepository.QueryNoTracking() on articl.UserId equals user.Id
                        join files in _sysFileBusinesRepository.QueryNoTracking(w => w.BusinessCode.Equals(ConstFileBusinesType.ARTICLE_COVERIMAGE, StringComparison.Ordinal)) on articl.Id equals files.BusinessId into temp1
                        from t1 in temp1.DefaultIfEmpty()
                        select new ArticleShowPage()
                        {
                            ArticleTitle = articl.ArticleTitle,
                            Introduction = articl.Introduction,
                            Auth = articl.Auth,
                            CategoryId = articl.CategoryId,
                            CommentNum = articl.CommentNum,
                            CoverImage = $"{GlobalConfig.PreviewEndpoint}/{GlobalConfig.BucketName}/{t1.ObjectName}",
                            CreateTime = articl.CreateTime,
                            Id = articl.Id,
                            IsDisableComment = articl.IsDisableComment,
                            LikeNum = articl.LikeNum,
                            Top = articl.Top,
                            ViewNum = articl.ViewNum,
                            UserName = user.NickName
                        };

            var res = await query.OrderBy(w => w.Top).ThenByDescending(w => w.CreateTime).ToPagedListAsync(pageIndex, pageSize);
            if (res.Total > 0)
            {
                var ids = res.Items.Select(w => w.Id);
                var allCategorys = _blogCategoryRepository.QueryNoTracking(w => !w.IsDelete);
                var articleTags = _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && ids.Contains(w.ArticleId)).ToList();
                var allTags = _blogTagRepository.QueryNoTracking();
                var files = _sysFileBusinesRepository.GetFileUploadUrl(ConstFileBusinesType.ARTICLE_COVERIMAGE, ids);
                res.Items.ForEach(item =>
                {
                    item.CoverImage = _sysFileBusinesRepository.GetFileUploadImgUrl(files, item.Id);
                    item.CategoryTitle = allCategorys.FirstOrDefault(w => w.Id == item.CategoryId)?.CategoryTitle;
                    var tags = articleTags.Where(w => w.ArticleId == item.Id).Select(w => w.TagId);
                    item.Tags = allTags.Where(w => tags.Contains(w.Id)).Select(s => new DefaultSelectOption()
                    {
                        Label = s.TagTitle,
                        Value = s.Id.ToString()
                    }).ToList();
                });
            }
            return res;
        }
        /// <summary>
        /// 获取前端博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<ArticleDetail> GetArticleDetail(Guid id, string accountCode)
        {
            var article = await _blogArticleRepository
                .Query()
                .WhereIf(w => w.Auth == 0, string.IsNullOrEmpty(accountCode))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete && x.ArticleStatus == BlogArticleStatus.PUBLISHED);
            if (article == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            var res = _mapper.Map<ArticleDetail>(article);

            int i = 0;
            var articleRowList = _blogArticleRepository.QueryNoTracking(w => !w.IsDelete)
                                    .OrderBy(w => w.Top).ThenByDescending(w => w.CreateTime)
                                    .Select(w => new { w.Id, w.ArticleTitle })
                                    .ToList()
                                    .Select(w => new ArticlePrevNext() { Id = w.Id, Title = w.ArticleTitle, RowIndex = i++ }).ToList();
            var currentRow = articleRowList.FirstOrDefault(w => w.Id == article.Id);
            if (currentRow.RowIndex - 1 > 0)
            {
                res.Prev = articleRowList.FirstOrDefault(w => w.RowIndex == currentRow.RowIndex - 1);
            }
            if (currentRow.RowIndex + 1 < articleRowList.Count())
            {
                res.Next = articleRowList.FirstOrDefault(w => w.RowIndex == currentRow.RowIndex + 1);
            }
            res.Tags = (from at in _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && w.ArticleId == res.Id)
                        join tag in _blogTagRepository.QueryNoTracking() on at.TagId equals tag.Id
                        select new DefaultSelectOption()
                        {
                            Label = tag.TagTitle,
                            Value = tag.Id.ToString()
                        }).ToList();

            res.CoverImage = _sysFileBusinesRepository.GetFileUploadDto(ConstFileBusinesType.ARTICLE_COVERIMAGE, article.Id).FirstOrDefault()?.Url;
            //博文预览加一
            article.ViewNum += 1;
            _blogArticleRepository.Update(article);
            _unitOfWork.SaveChanges();

            return res;
        }
        /// <summary>
        /// 获取博文归档数据
        /// </summary>
        /// <param name="accountCode">用户账号</param>
        /// <returns></returns>
        public async Task<IList<BlogArchives>> GetArchives(string accountCode)
        {
            var allArticle = await _blogArticleRepository
                .QueryNoTracking(w => !w.IsDelete && w.ArticleStatus == BlogArticleStatus.PUBLISHED)
                .WhereIf(w => w.Auth == 0, string.IsNullOrEmpty(accountCode))
                .Select(s => new
            {
                s.Id,
                s.ArticleTitle,
                s.CreateTime
            }).OrderByDescending(w => w.CreateTime).ToListAsync();
            var years = (from article in allArticle
                         group article by article.CreateTime.ToString("yyyy")
                        into g
                         select new BlogArchives()
                         {
                             YearTime = g.Key
                         }).ToList();
            foreach (var year in years)
            {
                year.ArchiveStages = allArticle.Where(w => w.CreateTime.ToString("yyyy") == year.YearTime).Select(s => new BlogArchiveStage()
                {
                    ArticleId = s.Id,
                    ArticleTitle = s.ArticleTitle,
                    TimeLine = s.CreateTime.ToString("MM-dd")
                }).ToList();
            }
            return years;
        }
        #endregion
        /// <summary>
        /// 获取博文分页
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="articleStatus"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagIds"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Page<ArticlePage>> GetArticlePage(string articleTitle, BlogArticleStatus? articleStatus, Guid? categoryId, List<Guid> tagIds, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            var articleIds = new List<Guid>();
            if (tagIds != null)
            {
                articleIds = _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && tagIds.Contains(w.TagId)).Select(s => s.ArticleId).Distinct().ToList();
            }

            var articlQuery = _blogArticleRepository.QueryNoTracking()
                             .WhereIf(w => w.ArticleTitle.Contains(articleTitle), !string.IsNullOrEmpty(articleTitle))
                             .WhereIf(w => articleIds.Contains(w.Id), articleIds.Any())
                             .WhereIf(w => w.ArticleStatus == articleStatus, articleStatus != null)
                             .WhereIf(w => w.CreateTime >= beginTime && w.CreateTime <= endTime, beginTime != null && endTime != null);

            // 博文分类暂时只做2级
            if (categoryId != null)
            {
                var category = _blogCategoryRepository.QueryNoTracking(w => !w.IsDelete && w.Id == categoryId).FirstOrDefault();
                if (category != null && category.ParentId == Guid.Empty)
                {
                    var ids = _blogCategoryRepository.QueryNoTracking(w => w.ParentId == category.Id).Select(w => w.Id).ToList();
                    articlQuery = articlQuery.Where(w => w.CategoryId == categoryId || ids.Contains(w.Id));
                }
                else
                {
                    articlQuery = articlQuery.Where(w => w.CategoryId == categoryId);
                }
            }
            var query = from articl in articlQuery
                        join user in _sysUserRepository.QueryNoTracking() on articl.UserId equals user.Id
                        join files in _sysFileBusinesRepository.QueryNoTracking(w => w.BusinessCode.Equals(ConstFileBusinesType.ARTICLE_COVERIMAGE, StringComparison.Ordinal)) on articl.Id equals files.BusinessId into temp1
                        from t1 in temp1.DefaultIfEmpty()
                        select new ArticlePage()
                        {
                            ArticleStatus = articl.ArticleStatus,
                            ArticleTitle = articl.ArticleTitle,
                            Introduction = articl.Introduction,
                            Auth = articl.Auth,
                            CategoryId = articl.CategoryId,
                            CommentNum = articl.CommentNum,
                            CoverImage = $"{GlobalConfig.PreviewEndpoint}/{GlobalConfig.BucketName}/{t1.ObjectName}",
                            CreateTime = articl.CreateTime,
                            Id = articl.Id,
                            IsDisableComment = articl.IsDisableComment,
                            LikeNum = articl.LikeNum,
                            Top = articl.Top,
                            ViewNum = articl.ViewNum,
                            UserName = user.NickName
                        };

            var res = await query.OrderBy(w => w.Top).ThenByDescending(w => w.CreateTime).ToPagedListAsync(pageIndex, pageSize);
            if (res.Total > 0)
            {
                var ids = res.Items.Select(w => w.Id);
                var allCategorys = _blogCategoryRepository.QueryNoTracking(w => !w.IsDelete);
                var articleTags = _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && ids.Contains(w.ArticleId)).ToList();
                var allTags = _blogTagRepository.QueryNoTracking();
                var files = _sysFileBusinesRepository.GetFileUploadUrl(ConstFileBusinesType.ARTICLE_COVERIMAGE, ids);
                res.Items.ForEach(item =>
                {
                    item.CoverImage = _sysFileBusinesRepository.GetFileUploadImgUrl(files, item.Id);
                    item.CategoryTitle = allCategorys.FirstOrDefault(w => w.Id == item.CategoryId)?.CategoryTitle;
                    var tags = articleTags.Where(w => w.ArticleId == item.Id).Select(w => w.TagId);
                    item.TagTitles = string.Join(",", allTags.Where(w => tags.Contains(w.Id)).Select(s => s.TagTitle));
                });
            }
            return res;
        }
        /// <summary>
        /// 获取博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<ArticleDto> GetArticleInfo(Guid id)
        {
            var article = await _blogArticleRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            var res = _mapper.Map<ArticleDto>(article);
            res.TagIds = _blogArticleTagRepository.QueryNoTracking(w => !w.IsDelete && w.ArticleId == res.Id).Select(w => w.TagId).ToList();
            res.CoverImage = _sysFileBusinesRepository.GetFileUploadDto(ConstFileBusinesType.ARTICLE_COVERIMAGE, article.Id);
            return res;
        }
        /// <summary>
        /// 更新博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> UpdateArticle(SaveArticleInput saveArticleInput, Guid userId)
        {
            if (saveArticleInput.Id == null) throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            var updateArticle = _blogArticleRepository.Get(saveArticleInput.Id);
            if (updateArticle == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            _mapper.Map(saveArticleInput, updateArticle);
            updateArticle.UserId = userId;
            _blogArticleTagRepository.HardDelete(w => w.ArticleId == updateArticle.Id);
            AddArticleTags(updateArticle.Id, saveArticleInput.TagIds);
            _unitOfWork.BeginTransaction();
            _blogArticleRepository.Update(updateArticle);
            await _sysFileBusinesRepository.SetFilesStatus(saveArticleInput.Id);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _unitOfWork.CommitTransaction();
            return res;
        }
        /// <summary>
        /// 添加博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> AddArticle(SaveArticleInput saveArticleInput, Guid userId)
        {
            _unitOfWork.BeginTransaction();
            var addArticle = _mapper.Map<BlogArticle>(saveArticleInput);
            addArticle.UserId = userId;
            AddArticleTags(addArticle.Id, saveArticleInput.TagIds);
            _blogArticleRepository.Insert(addArticle);
            await _sysFileBusinesRepository.SetFilesStatus(saveArticleInput.Id);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _unitOfWork.CommitTransaction();
            return res;
        }
        /// <summary>
        /// 删除博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteArticle(Guid id)
        {
            _unitOfWork.BeginTransaction();
            //删除博文
            await _blogArticleRepository.HardDeleteAsync(id);
            //删除博文相关图片
            await _sysFileBusinesRepository.DeleteFileByBusinesId(id);
            //删除博文的标签
            _blogArticleTagRepository.Delete(w => w.ArticleId == id);
            var res = await _unitOfWork.SaveChangesAsync() > 0;
            _unitOfWork.CommitTransaction();
            return res;
        }
        /// <summary>
        /// 添加博文标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        private void AddArticleTags(Guid articleId, IList<Guid> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                _blogArticleTagRepository.Insert(new BlogArticleTag()
                {
                    ArticleId = articleId,
                    TagId = tagId
                });
            }
        }
    }
}
