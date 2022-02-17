using Dw.Framework.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dw.Framework.Infrastructure.Database
{
    internal class ThreadDbContextFactory
    {
        public static DbContext GetDbContext()
        {
            var value = AsyncLocalThreadFactory<DbContext>.GetValue();
            if (value == null)
                throw new ArgumentNullException(nameof(DbContext));
            return value;
        }
        internal static void SetDbContext(DbContext dbContext)
        {
            AsyncLocalThreadFactory<DbContext>.SetValue(dbContext);
        }
    }
}
