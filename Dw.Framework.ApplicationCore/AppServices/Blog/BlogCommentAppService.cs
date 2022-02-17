using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using IPTools.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog
{
    public class BlogCommentAppService : IBlogCommentAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogCommentRepository _blogCommentRepository;
        private readonly RedisHashService _redisHashService;
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly ISysUserRepository _sysUserRepository;
        public readonly string basehashid = "blog:article:comment";

        public BlogCommentAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogCommentRepository blogCommentRepository, RedisHashService redisHashService, IBlogArticleRepository blogArticleRepository, ISysUserRepository sysUserRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogCommentRepository = blogCommentRepository;
            this._redisHashService = redisHashService;
            this._blogArticleRepository = blogArticleRepository;
            this._sysUserRepository = sysUserRepository;
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task<IList<CommentList>> GetArticleCommentList(Guid articleId, string ip)
        {
            var list = await _blogCommentRepository.QueryNoTracking(w => w.ArticleId == articleId).OrderBy(o => o.CreateTime).ToListAsync();
            var res = _mapper.Map<List<CommentList>>(list);
            if (res.Any())
            {
                string hashid = basehashid + ":" + articleId.ToString();
                var keys = list.Select(s => s.Id.ToString()).ToArray();
                var kv = _redisHashService.GetAllEntriesFromHash(hashid);
                res.ForEach(item =>
                {
                    string keyId = item.Id.ToString();
                    if (kv.ContainsKey(keyId))
                    {
                        item.IsLike = kv[keyId].Equals(ip, StringComparison.OrdinalIgnoreCase);
                    }
                });
            }
            return res;
        }
        /// <summary>
        /// 提交评论
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ip"></param>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task SubComment(SubCommentInput input, string ip, string accountCode)
        {
            if(input.ParentCommentId != null && input.ParentCommentId != Guid.Empty)
            {
                var data = _blogCommentRepository.FirstOrDefault(f => f.Id == input.ParentCommentId && !f.IsDelete);
                if (data == null)
                {
                    throw new CustomException(ResultCodeEnums.FAIL, "回复的评论不存在或已被删除~");
                }
            }
            var comment = _mapper.Map<BlogComment>(input);
            var ip_info = IpTool.Search(ip);
            comment.CommentLocation = ip_info.Province + " " + ip_info.City;
            comment.CommentIp = ip;
            if (!string.IsNullOrEmpty(accountCode) && string.IsNullOrWhiteSpace(input.CommentNickName))
            {
                var userInfo = await _sysUserRepository.GetUserFromCache(accountCode);
                comment.CommentNickName = $"作者({userInfo.NickName})";
                comment.CommenterType = 1;
            }
            await _blogCommentRepository.InsertAsync(comment);
            // 文章评论数加1
            await _blogArticleRepository.ArticleCommentIncr(input.ArticleId);
            _unitOfWork.SaveChanges();
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task Like(Guid commentId, string ip)
        {
            var data = await _blogCommentRepository.GetAsync(commentId);
            if (data != null)
            {
                string key = commentId.ToString();
                string hashid = basehashid + ":" + data.ArticleId.ToString();
                var hashkey = _redisHashService.GetValueFromHash(hashid, key);
                if (!string.IsNullOrEmpty(hashkey))
                {
                    throw new CustomException(ResultCodeEnums.FAIL, "已经点过赞了~");
                }
                _redisHashService.SetEntryInHashIfNotExists(hashid, key, ip);
                data.CommentLikeNum += 1;
                _unitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task Delete(Guid commentId)
        {
            var data = await _blogCommentRepository.GetAsync(commentId);
            if (data != null)
            {
                await _blogCommentRepository.DeleteAsync(commentId);
                // 文章评论数加1
                await _blogArticleRepository.ArticleCommentReduce(data.ArticleId);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
