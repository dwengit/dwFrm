using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Blog.Front;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog.IService
{
    public interface IBlogArticleAppService
    {
        #region 博客前端接口
        /// <summary>
        /// 获取前端博文列表
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Page<ArticleShowPage>> GetArticleShowPage(string articleTitle, Guid? categoryId, Guid? tagId, int pageIndex, int pageSize, string accountCode);
        /// <summary>
        /// 获取前端博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        Task<ArticleDetail> GetArticleDetail(Guid id, string accountCode);
        // <summary>
        /// 获取博文归档数据
        /// </summary>
        /// <returns></returns>
        Task<IList<BlogArchives>> GetArchives(string accountCode);
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
        Task<Page<ArticlePage>> GetArticlePage(string articleTitle, BlogArticleStatus? articleStatus, Guid? categoryId, List<Guid> tagIds, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize);
        /// <summary>
        /// 获取博文详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleDto> GetArticleInfo(Guid id);
        /// <summary>
        /// 更新博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UpdateArticle(SaveArticleInput saveArticleInput, Guid userId);
        /// <summary>
        /// 添加博文
        /// </summary>
        /// <param name="saveArticleInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> AddArticle(SaveArticleInput saveArticleInput, Guid userId);
        /// <summary>
        /// 删除博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteArticle(Guid id);
    }
}
