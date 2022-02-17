using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.System
{
    public class FileUploadDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SysFileBusines表Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BusinessId { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string Url { get; set; }
    }
    public class FileOnlyUploadDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 存储对象名称
        /// </summary>
        public string objectName { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string Url { get; set; }
    }
}
