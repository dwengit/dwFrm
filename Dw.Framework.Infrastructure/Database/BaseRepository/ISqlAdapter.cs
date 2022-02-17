using System;
using System.Collections.Generic;
using System.Text;
using Dw.Framework.Infrastructure.Database.Impl.DapperAdapter;

namespace Dw.Framework.Infrastructure.Database
{
    public interface ISqlAdapter
    {
        string PagingBuild(ref PartedSql partedSql, object sqlArgs, long skip, long take);
    }
}
