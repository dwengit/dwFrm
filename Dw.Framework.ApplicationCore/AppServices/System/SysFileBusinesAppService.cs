using AutoMapper;
using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.ObjectStorage;
using Dw.Framework.Infrastructure.Utility;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.System
{
    public class SysFileBusinesAppService : ISysFileBusinesAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MinioAPI _minioAPI;
        private readonly ISysFileBusinesRepository _sysFileBusinesRepository;
        public static string bucketName = GlobalConfig.BucketName;
        public static string[] imgExt = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        public SysFileBusinesAppService(IUnitOfWork unitOfWork, IMapper mapper, ISysDeptRepository sysDeptRepository, MinioAPI minioAPI, ISysFileBusinesRepository sysFileBusinesRepository)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._minioAPI = minioAPI;
            this._sysFileBusinesRepository = sysFileBusinesRepository;
        }
        public async Task<FileOnlyUploadDto> UploadOnlyFileAsync(FileOnlyUploadInput input)
        {
            long size = input.UpFile.Length;
            string fileName = input.UpFile.FileName;
            string extension = Path.GetExtension(fileName);
            string objectName = Guid.NewGuid().ToString() + extension;
            try
            {
                if (imgExt.Contains(extension))
                {
                    objectName = "images/" + objectName;
                }
                else
                {
                    objectName = "file/" + objectName;
                }
                await _minioAPI.UploadByStreamAsync(bucketName, objectName, input.UpFile.OpenReadStream(), size, input.UpFile.ContentType);
                
                return new FileOnlyUploadDto()
                {
                    Name = fileName,
                    objectName = objectName,
                    Url = $"{GlobalConfig.PreviewEndpoint}/{bucketName}/{objectName}"
                };
            }
            catch (Exception ex)
            {
                Logger.Error("上传文件到对象存储失败，错误信息：" + ex.Message);
                throw new CustomException(ResultCodeEnums.CODE500, "上传文件失败");
            }
        }
        public async Task RemoveUploadOnlyFile(string objectName)
        {
            await _minioAPI.removeObjectAsync(bucketName, objectName);
        }
        public async Task<FileUploadDto> UploadFileAsync(FileUploadInput input, string accountCode)
        {
            long size = input.UpFile.Length;
            string fileName = input.UpFile.FileName;
            string title = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string objectName = Guid.NewGuid().ToString() + extension;
            try
            {
                if (imgExt.Contains(extension))
                {
                    objectName = "images/" + objectName;
                }
                else
                {
                    objectName = "file/" + objectName;
                }
                await _minioAPI.UploadByStreamAsync(bucketName, objectName, input.UpFile.OpenReadStream(), size, input.UpFile.ContentType);
                SysFileBusines sysFile = new SysFileBusines()
                {
                    AccountCode = accountCode,
                    BucketName = bucketName,
                    BusinessCode = input.BusinessCode,
                    BusinessId = input.BusinessId,
                    Extension = extension,
                    Title = title,
                    Size = size,
                    ObjectName = objectName,
                };
                await _sysFileBusinesRepository.InsertAsync(sysFile);
                await _unitOfWork.SaveChangesAsync();
                return new FileUploadDto()
                {
                    Id = sysFile.Id,
                    Name = fileName,
                    BusinessId = input.BusinessId,
                    Url = $"{GlobalConfig.PreviewEndpoint}/{bucketName}/{objectName}"
                };
            }
            catch (Exception ex)
            {
                Logger.Error("上传文件到对象存储失败，错误信息：" + ex.Message);
                throw new CustomException(ResultCodeEnums.CODE500, "上传文件失败");
            }
        }
        public async Task RemoveUploadFile(Guid id)
        {
            var data = _sysFileBusinesRepository.FirstOrDefault(w => w.Id == id);
            if (data != null)
            {
                await _minioAPI.removeObjectAsync(bucketName, data.ObjectName);
                await _sysFileBusinesRepository.HardDeleteAsync(data);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
