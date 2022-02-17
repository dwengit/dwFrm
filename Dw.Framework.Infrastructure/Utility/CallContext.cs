using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dw.Framework.Infrastructure.Utility
{
    public static class CallContext
    {
        static AsyncLocal<ConcurrentDictionary<string, object>> state = new AsyncLocal<ConcurrentDictionary<string, object>>();

        public static void SetData(string name, object data)
        {
            state.Value = new ConcurrentDictionary<string, object>();
            state.Value.AddOrUpdate(name, data, (a, b) => data);
        }

        public static object GetData(string name)
        {
            return state.Value.TryGetValue(name, out object data) ? data : null;
        }

        internal static bool DeleteData(string name)
        {
            return state.Value.TryRemove(name, out object data);
        }


        internal static IEnumerable<string> GetDictionaryKeys()
        {
            return state.Value.Keys;
        }

    }

}
