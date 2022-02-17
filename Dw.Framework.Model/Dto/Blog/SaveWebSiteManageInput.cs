using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Blog
{
    public class SaveWebSiteManageInput
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
        public string BackImage { get; set; }
    }
}
