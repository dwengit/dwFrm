using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dw.Framework.Infrastructure.Utility
{
    public static class AsyncLocalThreadFactory<T>
    {
        private static AsyncLocal<T> asyncLocal = new AsyncLocal<T>();
        public static T GetValue()
        {
            return asyncLocal.Value;
        }
        public static void SetValue(T value)
        {
            asyncLocal.Value = value;
        }
    }
}
