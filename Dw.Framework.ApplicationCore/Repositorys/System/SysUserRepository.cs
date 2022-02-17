using Dw.Framework.ApplicationCore.Repositorys.System;
using Dw.Framework.Infrastructure;
using Dw.Framework.Infrastructure.Caches.RedisHelper.Service;
using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Infrastructure.Helper;
using Dw.Framework.Model.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.Accounts
{
    public class SysUserRepository : Repository<SysUser, Guid>, ISysUserRepository
    {
        private readonly RedisStringService _redisStringService;
        public SysUserRepository(RedisStringService redisStringService)
        {
            this._redisStringService = redisStringService;
        }
        public const string USER_HEAD_NAME = "user:";
        public static string GetUserCatchName(string accountCode)
        {
            return USER_HEAD_NAME + accountCode;
        }
        public async Task<SysUser> InserUserToCache(string accountCode)
        {
            var user = await Query(w => w.AccountCode.Equals(accountCode)).FirstOrDefaultAsync();
            if(user != null)
            {
                InserUserToCache(user);
            }
            return user;
        }
        public void InserUserToCache(SysUser user)
        {
            _redisStringService.Remove(GetUserCatchName(user.AccountCode));
            _redisStringService.Set(GetUserCatchName(user.AccountCode), user, TimeSpan.FromMinutes(40));
        }
        public async Task<SysUser> GetUserFromCache(string accountCode)
        {
            var strUserJson = _redisStringService.Get(GetUserCatchName(accountCode));
            if (string.IsNullOrEmpty(strUserJson))
            {
                return await InserUserToCache(accountCode);
            }
            
            return strUserJson.StrJsonToObj<SysUser>();
        }
        /// <summary>
        /// 获取用户数据，如果为null，抛出异常
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<SysUser> GetUserFromCacheNotNull(string accountCode)
        {
            var user = await GetUserFromCache(accountCode);
            if (user == null)
            {
                throw new CustomException(ResultCodeEnums.CODE401);
            }
            return user;
        }
        public override void HardDelete(Guid id)
        {
            base.HardDelete(id);
            var user = Query(w => w.Id == id).FirstOrDefault();
            _redisStringService.Remove(GetUserCatchName(user.AccountCode));
        }
    }
}
