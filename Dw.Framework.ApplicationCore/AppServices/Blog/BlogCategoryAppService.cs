using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Blog.IService;
using Dw.Framework.ApplicationCore.Repositorys.Blog;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Blog;
using Dw.Framework.Model.Dto.Blog;
using Dw.Framework.Model.Dto.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Blog
{
    /// <summary>
    /// 博文分类服务类
    /// </summary>
    public class BlogCategoryAppService : IBlogCategoryAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        public BlogCategoryAppService(IUnitOfWork unitOfWork, IMapper mapper, IBlogCategoryRepository blogCategoryRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._blogCategoryRepository = blogCategoryRepository;
        }
        public async Task<List<DefaultTreeOption>> GetCategoryTreeOption()
        {
            var list = await _blogCategoryRepository.QueryNoTracking().Where(w => !w.IsDelete).OrderBy(w => w.SortNO).ToListAsync();
            var tree = ConvertTree(list, Guid.Empty);
            return _mapper.Map<List<DefaultTreeOption>>(tree);
        }
        /// <summary>
        /// 转换为树形结构
        /// </summary>
        /// <param name="menuFunctionalLists"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private List<BlogCategory> ConvertTree(List<BlogCategory> list, Guid parentid)
        {
            List<BlogCategory> treeList = new List<BlogCategory>();
            var parentMenuFunctiona = list.Where(m => m.ParentId == parentid).ToList();
            if (!parentMenuFunctiona.Any())
            {
                return treeList;
            }
            foreach (var item in parentMenuFunctiona)
            {
                treeList.Add(item);
                item.Children = ConvertTree(list, item.Id);
            }
            return treeList;
        }
        /// <summary>
        /// 获取博文分类页面树
        /// </summary>
        /// <param name="categoryTitle"></param>
        /// <returns></returns>
        public async Task<List<CategoryPageTree>> GetCategoryPageTree(string categoryTitle)
        {
            var list = await _blogCategoryRepository.QueryNoTracking(w => !w.IsDelete)
                        .WhereIf(w => w.CategoryTitle.Contains(categoryTitle), !string.IsNullOrWhiteSpace(categoryTitle))
                        .OrderByDescending(w => w.SortNO)
                        .ThenByDescending(w => w.CreateTime)
                        .ToListAsync();
            if (list.Count == 0)
            {
                return new List<CategoryPageTree>();
            }
            //递归生成Tree结构
            var tree = ConvertTree(list, Guid.Empty);
            return _mapper.Map<List<CategoryPageTree>>(tree);
        }
        /// <summary>
        /// 保存分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task Save(SaveCategoryInput input)
        {
            await VerifyCategoryNameRepeat(input.CategoryTitle, input.Id);
            if (input.Id != null && input.Id != Guid.Empty)
            {
                var updata = _blogCategoryRepository.Get(input.Id);
                if(updata == null)
                {
                    throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
                }
                _mapper.Map(input, updata);
                _blogCategoryRepository.Update(updata);
            }
            else
            {
                var data = _mapper.Map<BlogCategory>(input);
                await _blogCategoryRepository.InsertAsync(data);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 删除博文分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            _blogCategoryRepository.Delete(w => w.Id == id);
            await _unitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 校验博文分类名称重复将会引发异常
        /// </summary>
        /// <param name="categoryTitle"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task VerifyCategoryNameRepeat(string categoryTitle, Guid id)
        {
            if (await _blogCategoryRepository.QueryNoTracking().WhereIf(w => w.Id != id, id != null && id != Guid.Empty)
                .AnyAsync(w => w.CategoryTitle.Equals(categoryTitle, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "分类名称不能重复");
            }
        }
    }
}
