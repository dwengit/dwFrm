using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.AppServices.Accounts
{
    public interface ISysFileBusinesAppService
    {
        /// <summary>
        /// 上传业务文件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        Task<FileUploadDto> UploadFileAsync(FileUploadInput input, string accountCode);
        /// <summary>
        /// 删除业务文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveUploadFile(Guid id);
        /// <summary>
        /// 单一上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileOnlyUploadDto> UploadOnlyFileAsync(FileOnlyUploadInput input);
        /// <summary>
        /// 直接删除对象存储对象
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Task RemoveUploadOnlyFile(string objectName);
    }
}
