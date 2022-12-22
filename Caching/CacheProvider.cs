using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using UnitOfWork.Core.IConfiguration;
using UnitOfWork.Models;

namespace UnitOfWork.Caching
{
	public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;
        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);
        private readonly IUnitOfWork unitOfWork;

		public CacheProvider(IMemoryCache memoryCache, IUnitOfWork unitOfWork)
		{
            this.memoryCache = memoryCache;
            this.unitOfWork = unitOfWork;
		}

        public async Task<IEnumerable<User>> GetCachedResponse()
        {
            try
            {
                return await GetCachedResponse(CacheKeys.Employees, GetUsersSemaphore);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphoreSlim)
        {
            bool isAvaiable = memoryCache.TryGetValue(cacheKey, out List<User> users);
            if (isAvaiable) return users;
            try
            {
                await semaphoreSlim.WaitAsync();
                isAvaiable = memoryCache.TryGetValue(cacheKey, out users);
                if (isAvaiable) return users;

                // if data doesn't already exists in cache set it
                var result = await unitOfWork.Users.All();
                users = result.ToList();
                var memoryCacheEntryOpts = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Size = 1024
                };
                memoryCache.Set(cacheKey, users, memoryCacheEntryOpts);
            }
            catch 
            {
                throw;
            }
            finally
            {
                semaphoreSlim.Release();
            }

            return users;
        }
    }
}

