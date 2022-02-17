using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dw.Framework.Infrastructure.Caches
{
    public class MemoryCacheHelper
    {
        private static IMemoryCache _memoryCache = null;
        static MemoryCacheHelper()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expMinute"></param>
        public static void SetCache(string key, object value,int expMinute)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(expMinute));
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return _memoryCache.Get(key);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
