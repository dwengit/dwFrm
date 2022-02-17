using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class FileUploadInput
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public IFormFile UpFile { get; set; }
        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessCode { get; set; }
        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BusinessId { get; set; }
    }
    public class FileOnlyUploadInput
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public IFormFile UpFile { get; set; }
    }
}
