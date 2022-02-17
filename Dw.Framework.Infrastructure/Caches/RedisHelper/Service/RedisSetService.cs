 
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Interface;

namespace Dw.Framework.Infrastructure.Caches.RedisHelper.Service
{
    /// <summary>
    /// Set：用哈希表来保持字符串的唯一性，没有先后顺序，存储一些集合性的数据
    /// 1.共同好友、二度好友
    /// 2.利用唯一性，可以统计访问网站的所有独立 IP
    /// </summary>
    public class RedisSetService : RedisBase
    {
        #region 添加
        /// <summary>
        /// key集合中添加value值
        /// </summary>
        public void Add(string key, string value)
        {
            base.iClient.AddItemToSet(key, value);
        }
        /// <summary>
        /// key集合中添加list集合
        /// </summary>
        public void Add(string key, List<string> list)
        {
            base.iClient.AddRangeToSet(key, list);

        }
        #endregion

        #region 获取
        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        public string GetRandomItemFromSet(string key)
        {
            return base.iClient.GetRandomItemFromSet(key);
        }
        /// <summary>
        /// 获取key集合值的数量
        /// </summary>
        public long GetCount(string key)
        {
            return base.iClient.GetSetCount(key);
        }
        /// <summary>
        /// 获取所有key集合的值
        /// </summary>
        public HashSet<string> GetAllItemsFromSet(string key)
        {
            
            return base.iClient.GetAllItemsFromSet(key);
        }
        /// <summary>
        ///
        /// </summary>
        public string GetAndSetValue(string key,string value)
        {
            return base.iClient.GetAndSetValue(key, value);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 随机删除key集合中的一个值
        /// </summary>
        public string RandomRemoveItemFromSet(string key)
        {
            return base.iClient.PopItemFromSet(key);
        }
        /// <summary>
        /// 删除key集合中的value
        /// </summary>
        public void RemoveItemFromSet(string key, string value)
        {
            base.iClient.RemoveItemFromSet(key, value);
        }
        #endregion

        #region 其它
        /// <summary>
        /// 从fromkey集合中移除值为value的值，并把value添加到tokey集合中
        /// </summary>
        public void MoveBetweenSets(string fromkey, string tokey, string value)
        {
            base.iClient.MoveBetweenSets(fromkey, tokey, value);
        }
        /// <summary>
        /// 返回keys多个集合中的并集，返还hashset
        /// </summary>
        public HashSet<string> GetUnionFromSets(params string[] keys)
        {
            return base.iClient.GetUnionFromSets(keys);
        }
        /// <summary>
        /// 返回keys多个集合中的交集，返还hashset
        /// </summary>
        public HashSet<string> GetIntersectFromSets(params string[] keys)
        {
            return base.iClient.GetIntersectFromSets(keys);
        }
        /// <summary>
        /// 返回keys多个集合中的差集，返还hashset
        /// </summary>
        /// <param name="fromKey">原集合</param>
        /// <param name="keys">其他集合</param>
        /// <returns>出现在原集合，但不包含在其他集合</returns>
        public HashSet<string> GetDifferencesFromSet(string fromKey, params string[] keys)
        {
            return base.iClient.GetDifferencesFromSet(fromKey,keys);
        }
        /// <summary>
        /// keys多个集合中的并集，放入newkey集合中
        /// </summary>
        public void StoreUnionFromSets(string newkey, string[] keys)
        {
            base.iClient.StoreUnionFromSets(newkey, keys);
        }
        /// <summary>
        /// 把fromkey集合中的数据与keys集合中的数据对比，fromkey集合中不存在keys集合中，则把这些不存在的数据放入newkey集合中
        /// </summary>
        public void StoreDifferencesFromSet(string newkey, string fromkey, string[] keys)
        {
            base.iClient.StoreDifferencesFromSet(newkey, fromkey, keys);
        }
        #endregion
    }
}
