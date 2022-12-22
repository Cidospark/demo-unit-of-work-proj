using System;
using UnitOfWork.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace UnitOfWork.Caching
{
	public interface ICacheProvider
	{
		Task<IEnumerable<User>> GetCachedResponse();
        Task<IEnumerable<User>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphoreSlim);
    } 
}

