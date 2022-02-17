using Dw.Framework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Dw.Framework.Model.Dto.System
{
    public class AssignMenuFunctionalTree
    {
        public List<MenuFunctionalTreeOptions> AssignMenuFunctionalTreeOptions { get; set; } = new List<MenuFunctionalTreeOptions>();
        
        public List<Guid> CheckedKeys { get; set; }
    }
}
