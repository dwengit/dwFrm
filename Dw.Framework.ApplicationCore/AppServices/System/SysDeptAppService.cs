using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    public class SysDeptAppService : ISysDeptAppService
    {
        private readonly ISysDeptRepository _sysDeptRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SysDeptAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysDeptRepository sysDeptRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._sysDeptRepository = sysDeptRepository;
        }
        /// <summary>
        /// 获取-部门页面树
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<List<DeptPageTree>> GetDeptPageTree(string deptName, EnumSysDept? status)
        {
            //获取页面树需要展示的数据
            var list = await _sysDeptRepository
                                .QueryNoTracking()
                                .WhereIf(w => w.Status == status, status != null)
                                .WhereIf(w => w.DeptName.Contains(deptName), !string.IsNullOrWhiteSpace(deptName))
                                .Where(w => !w.IsDelete)
                                .OrderBy(w => w.SortNO)
                                .ThenBy(w => w.CreateTime)
                                .ToListAsync();

            if (list.Count == 0)
            {
                return new List<DeptPageTree>();
            }

            //把筛选后的数据的（父链Code）补充完整
            IEnumerable<string> ancestorsCodes = list.Select(w => w.AncestorsCode);
            HashSet<string> hs = new HashSet<string>();
            foreach (var ancestorsCode in ancestorsCodes)
            {
                string chainCode = default;
                string[] levelCode = ancestorsCode.Split('.');
                for (int i = 0; i < levelCode.Length; i++)
                {
                    chainCode += "." + levelCode[i];
                    hs.Add(chainCode.TrimStart('.'));
                }
            }
            if (hs.Count != 0)
            {
                //求差集并查出父链数据
                IEnumerable<string> exceptChain = hs.Except(ancestorsCodes);
                var exceptList = _sysDeptRepository.QueryNoTracking(w => exceptChain.Contains(w.AncestorsCode));

                //合并
                list = list.Union(exceptList).ToList();
            }

            //递归生成Tree结构
            var tree = ConvertTree(list, Guid.Empty);
            return _mapper.Map<List<DeptPageTree>>(tree);
        }
        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="saveDeptInput"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> SaveDept(SaveDeptInput saveDeptInput)
        {
            if (_sysDeptRepository.Query().WhereIf(w => w.Id != saveDeptInput.Id, saveDeptInput.Id != null).Any(w => w.DeptName.Equals(saveDeptInput.DeptName)))
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "部门名称不能重复");
            }
            var parentDept = _sysDeptRepository.Get(saveDeptInput.ParentId);
            if (parentDept == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "上级部门不存在，或已被删除");
            }
           
            if (saveDeptInput.Id != Guid.Empty)
            {
                await Update(saveDeptInput, parentDept);
            }
            else
            {
                SysDept dept = _mapper.Map<SysDept>(saveDeptInput);
                await Add(dept, parentDept);
            }
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteDept(Guid id)
        {
            var delDept = await _sysDeptRepository.GetAsync(id);
            if (delDept == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            if (delDept.Level == 1)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR, "不能删除根部门");
            }
            _sysDeptRepository.HardDelete(delDept);
            DeleteChild(delDept.Id);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 递归删除子集
        /// </summary>
        /// <param name="parentid"></param>
        private void DeleteChild(Guid parentid)
        {
            var parentSysModuleResource = _sysDeptRepository.Query(m => m.ParentId == parentid).ToList();
            if (!parentSysModuleResource.Any())
            {
                return;
            }
            foreach (var item in parentSysModuleResource)
            {
                DeleteChild(item.Id);
                _sysDeptRepository.HardDelete(item);
            }
        }
        /// <summary>
        /// 懒加载部门树
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<DeptTreeOptions> LazyDeptTreeOptions(Guid parentid)
        {
            var childs = _sysDeptRepository.QueryNoTracking(w => w.ParentId == parentid).Select(s => new DeptTreeOptions()
            {
                Id = s.Id,
                ParentId = s.ParentId,
                Label = s.DeptName
            }).ToList();
            foreach (var item in childs)
            {
                item.Children = _sysDeptRepository.Any(w => w.ParentId == item.Id) ? null : new List<DeptTreeOptions>();
            }
            return childs;
        }
        /// <summary>
        /// 全部加载-部门树
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeptTreeOptions>> AllDeptTreeOptions()
        {
            var list = await _sysDeptRepository.QueryNoTracking().Where(w => !w.IsDelete).OrderBy(w => w.SortNO).ToListAsync();
            var tree = ConvertTree(list, Guid.Empty);
            return _mapper.Map<List<DeptTreeOptions>>(tree);
        }
        /// <summary>
        /// 获取-部门详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeptDto> GetDeptInfo(Guid id)
        {
            var dept = await _sysDeptRepository.GetAsync(id);
            if (dept == null)
            {
                throw new CustomException(ResultCodeEnums.DATA_VALIDATE_ERR);
            }
            return _mapper.Map<DeptDto>(dept);
        }

        #region 方法
        public async Task Add(SysDept dept, SysDept parentDept)
        {
            //获取兄弟节点最大Code
            int maxNodeCode = GetMaxBrotherNodeCode(dept.ParentId);
            dept.CurrentLevelCode = maxNodeCode;
            dept.AncestorsCode = parentDept.AncestorsCode + "." + maxNodeCode;
            dept.AncestorsFullName = (parentDept.AncestorsFullName + "." + parentDept.DeptName).Trim('.');
            await _sysDeptRepository.InsertAsync(dept);
        }
        public async Task Update(SaveDeptInput saveDept, SysDept parentDept)
        {
            var oldDept = _sysDeptRepository.Get(saveDept.Id);
            if(oldDept.ParentId != saveDept.ParentId)
            {
                //如果重新更换了上级部门，就迁移当前部门和子部门的（祖链）
                int newLevelCode = GetMaxBrotherNodeCode(parentDept.Id);
                string oldAncestorsChildCode = oldDept.AncestorsCode;
                string oldAncestorsChildFullName = oldDept.AncestorsFullName + "." + oldDept.DeptName;

                oldDept.AncestorsCode = parentDept.AncestorsCode + "." + newLevelCode;
                oldDept.AncestorsFullName = parentDept.AncestorsFullName + "." + parentDept.DeptName;
                oldDept.CurrentLevelCode = newLevelCode;
                //更新前的旧链下的子节点数据
                var oldChain = _sysDeptRepository.GetAll(w => w.Id != saveDept.Id && w.AncestorsCode.StartsWith(oldAncestorsChildCode));

                foreach (var oldChainDept in oldChain)
                {
                    oldChainDept.AncestorsCode = oldChainDept.AncestorsCode.Replace(oldAncestorsChildCode, oldDept.AncestorsCode);
                    oldChainDept.AncestorsFullName = oldChainDept.AncestorsFullName.Replace(oldAncestorsChildFullName, oldDept.AncestorsFullName + "." + saveDept.DeptName);
                    _sysDeptRepository.Update(oldChainDept);
                }
            }
            _mapper.Map(saveDept, oldDept);
            await _sysDeptRepository.UpdateAsync(oldDept);
        }
        /// <summary>
        /// 获取兄弟节点最大Code
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private int GetMaxBrotherNodeCode(Guid parentid)
        {
            int levelCode = _sysDeptRepository.QueryNoTracking(w => w.ParentId == parentid).OrderByDescending(w => w.CurrentLevelCode).Select(s => s.CurrentLevelCode).FirstOrDefault();
            return levelCode > 0 ? ++levelCode : 101;
        }
        /// <summary>
        /// 转换为树形结构
        /// </summary>
        /// <param name="menuFunctionalLists"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private List<SysDept> ConvertTree(List<SysDept> menuSysDeptList, Guid parentid)
        {
            List<SysDept> treeList = new List<SysDept>();
            var parentMenuFunctiona = menuSysDeptList.Where(m => m.ParentId == parentid).ToList();
            if (!parentMenuFunctiona.Any())
            {
                return treeList;
            }
            foreach (var item in parentMenuFunctiona)
            {
                treeList.Add(item);
                item.Children = ConvertTree(menuSysDeptList, item.Id);
            }
            return treeList;
        }
        #endregion
    }
}
