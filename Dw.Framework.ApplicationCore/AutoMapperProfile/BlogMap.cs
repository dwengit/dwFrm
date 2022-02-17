using AutoMapper;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Blog.Front;
using Dw.Framework.Model.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.ApplicationCore.AutoMapperProfile
{
    public class BlogMap : Profile
    {
        public BlogMap()
        {
            CreateMap<BlogArticle, ArticleDetail>();
            CreateMap<Page<BlogArticle>, Page<ArticlePage>>();
            CreateMap<BlogArticle, ArticlePage>();
            CreateMap<BlogArticle, ArticleDto>();
            CreateMap<SaveArticleInput, BlogArticle>();
            CreateMap<BlogArticle, ArticleDto>();

            CreateMap<BlogCategory, DefaultTreeOption>().ForMember(dest => dest.Label, opt => opt.MapFrom(s => s.CategoryTitle));
            CreateMap<BlogCategory, CategoryPageTree>();
            CreateMap<SaveCategoryInput, BlogCategory>();

            CreateMap<BlogTag, TagPage>();
            CreateMap<SaveTagInput, BlogTag>();

            CreateMap<BlogLink, LinkPage>();
            CreateMap<SaveLinkInput, BlogLink>();

            CreateMap<SaveWebSiteManageInput, BlogWebSiteManage>();

            CreateMap<BlogComment, CommentList>();
            CreateMap<SubCommentInput, BlogComment>();

        }
    }
}
