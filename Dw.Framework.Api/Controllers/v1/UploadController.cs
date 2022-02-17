using Dw.Framework.ApplicationCore.AppServices.Accounts;
using Dw.Framework.Infrastructure;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dw.Framework.Admin.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class UploadController : ApiControllersBase
    {
        private readonly ISysFileBusinesAppService _minioFileService;

        public UploadController(ISysFileBusinesAppService minioFileService)
        {
            this._minioFileService = minioFileService;
        }
        /// <summary>
        /// 简单上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<FileOnlyUploadDto>> PostOnlyFile([FromForm]FileOnlyUploadInput input)
        {
            var res = await _minioFileService.UploadOnlyFileAsync(input);
            return RespOk(res);
        }
        /// <summary>
        /// 上传业务文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<FileUploadDto>> PostFileAsync([FromForm]FileUploadInput input)
        {
            string accountCode = HttpContext.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.PrimarySid).Value;
            var res = await _minioFileService.UploadFileAsync(input, accountCode);
            return RespOk(res);
        }
        /// <summary>
        /// 删除业务文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Respbase> RemoveUploadFile(Guid id)
        {
            await _minioFileService.RemoveUploadFile(id);
            return RespOk();
        }
        /// <summary>
        /// 直接删除对象存储对象
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Respbase> RemoveUploadOnlyFile(string objectName)
        {
            await _minioFileService.RemoveUploadOnlyFile(objectName);
            return RespOk();
        }
    }
}
