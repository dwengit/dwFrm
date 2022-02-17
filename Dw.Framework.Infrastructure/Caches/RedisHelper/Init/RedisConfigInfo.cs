using Dw.Framework.Infrastructure.Utility;
using System.Configuration;

namespace Dw.Framework.Infrastructure.Caches.RedisHelper.Init
{
    /// <summary>
    /// redis配置文件信息
    /// 也可以放到配置文件去
    /// </summary>
    public class RedisConfigInfo
    {
        /// <summary>
        /// 可写的Redis链接地址
        /// format:ip1,ip2
        /// 
        /// 默认6379端口
        /// </summary>
        public static string WriteServerList = Appsettings.App("RedisConfig:WriteServerList");
        /// <summary>
        /// 可读的Redis链接地址
        /// format:ip1,ip2
        /// </summary>
        public static string ReadServerList = Appsettings.App("RedisConfig:ReadServerList");
        /// <summary>
        /// 最大写链接数
        /// </summary>
        public static int MaxWritePoolSize = 60;
        /// <summary>
        /// 最大读链接数
        /// </summary>
        public static int MaxReadPoolSize = 60;
        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        public static int LocalCacheTime = 180;
        /// <summary>
        /// 自动重启
        /// </summary>
        public static bool AutoStart = true;
        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,
        /// 如redis工作正常,请关闭该项
        /// </summary>
        public static bool RecordeLog = false;
    }
}