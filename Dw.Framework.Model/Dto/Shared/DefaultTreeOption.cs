using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Model.Dto.Shared
{
    public class DefaultTreeOption
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<DefaultTreeOption> Children { get; set; }
    }
}
