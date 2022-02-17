using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.ObjectStorage;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public class SysFileBusinesRepository : Repository<SysFileBusines, Guid>, ISysFileBusinesRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MinioAPI _minioAPI;
        public SysFileBusinesRepository(IUnitOfWork unitOfWork, MinioAPI minioAPI)
        {
            this._unitOfWork = unitOfWork;
            this._minioAPI = minioAPI;
        }
        /// <summary>
        /// 获取上传文件集合
        /// </summary>
        /// <param name="businesCode">业务编码</param>
        /// <param name="businesIds">业务ids</param>
        /// <returns></returns>
        public List<FileUploadDto> GetFileUploadDto(string businesCode, List<Guid> businesIds)
        {
            return QueryNoTracking(w => w.BusinessCode.Equals(businesCode, StringComparison.InvariantCultureIgnoreCase))
                .WhereIf(w => businesIds.Contains(w.BusinessId), businesIds != null && businesIds.Any())
                .Select(s => new FileUploadDto()
                {
                    Id = s.Id,
                    BusinessId = s.BusinessId,
                    Name = s.Title + s.Extension,
                    Url = $"{GlobalConfig.PreviewEndpoint}/{GlobalConfig.BucketName}/{s.ObjectName}"
                }).ToList();
        }
        /// <summary>
        /// 获取上传文件集合
        /// </summary>
        /// <param name="businesCode"></param>
        /// <param name="businesId"></param>
        /// <returns></returns>
        public List<FileUploadDto> GetFileUploadDto(string businesCode, Guid businesId)
        {
            return GetFileUploadDto(businesCode, new List<Guid> { businesId });
        }
        /// <summary>
        /// 获取上传文件集合
        /// </summary>
        /// <param name="businesCode"></param>
        /// <param name="businesIds"></param>
        /// <returns></returns>
        public List<SysFileBusines> GetFileUploadUrl(string businesCode, IEnumerable<Guid> businesIds)
        {
            return QueryNoTracking(w => w.BusinessCode.Equals(businesCode, StringComparison.OrdinalIgnoreCase) && businesIds.Contains(w.BusinessId)).ToList();
        }
        /// <summary>
        /// 获取上传图片集合Url
        /// </summary>
        /// <param name="fileDatas"></param>
        /// <param name="businesId"></param>
        /// <returns></returns>
        public string GetFileUploadImgUrl(List<SysFileBusines> fileDatas, Guid businesId)
        {
            var fileData = fileDatas.FirstOrDefault(w => w.BusinessId == businesId);
            return fileData == null ? "" : $"{GlobalConfig.PreviewEndpoint}/{GlobalConfig.BucketName}/{fileData.ObjectName}";
        }
        /// <summary>
        /// 修改文件状态
        /// </summary>
        /// <param name="businesId"></param>
        /// <returns></returns>
        public async Task SetFilesStatus(Guid businesId)
        {
            var files = Query(w => w.BusinessId == businesId).ToList();
            if (files.Any())
            {
                foreach (var item in files)
                {
                    item.FileStatus = 1;
                    await UpdateAsync(item);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 根据业务ID删除文件
        /// </summary>
        /// <param name="businesId"></param>
        /// <returns></returns>
        public async Task DeleteFileByBusinesId(Guid businesId)
        {
            var files = Query(w => w.BusinessId == businesId);
            if (files.Any())
            {
                foreach (var file in files)
                {
                    await HardDeleteAsync(file);
                    await _minioAPI.removeObjectAsync(GlobalConfig.BucketName, file.ObjectName);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
