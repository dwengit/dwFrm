using Dw.Framework.Model.Dto.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class WebSiteManageDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        public string WebSitName { get; set; }
        /// <summary>
        /// 站点背景图
        /// </summary>
        public IList<FileOnlyUploadDto> BackImage { get; set; } = new List<FileOnlyUploadDto>();
    }
}
