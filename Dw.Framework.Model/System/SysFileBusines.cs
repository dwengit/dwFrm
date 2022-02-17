using Dw.Framework.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.System
{
    /// <summary>
    /// 业务文件存储信息
    /// </summary>
    public class SysFileBusines : Entity<Guid>
    {
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 存储桶
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 存储对象名称，MD5
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 0：临时文件，1：业务数据已关联minio存储，（默认为0，提交业务数据更新为1，如果业务数据没有关联minio存储，定期会自动清除为0的数据文件）
        /// </summary>
        public int FileStatus { get; set; }
        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessCode { get; set; }
        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BusinessId { get; set; }
        /// <summary>
        /// 上传文件用户Id
        /// </summary>
        public string AccountCode { get; set; }
    }
}
