using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dw.Framework.Model.Enums
{
    public enum BlogArticleStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        DRAFT = 0,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        PUBLISHED = 1,
    }
    public enum EnumGetTagType
    {
        ALL = 0,
        HOT = 1,
    }
}
