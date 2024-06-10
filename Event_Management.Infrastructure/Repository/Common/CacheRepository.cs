using Event_Management.Domain.Repository.Common;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Event_Management.Infrastructure.Repository
{
	public class CacheRepository : ICacheRepository
	{
		private readonly IDistributedCache _distributedCache;

		public CacheRepository(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
			Console.WriteLine("CacheRepository");
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var cacheValue = await _distributedCache.GetStringAsync(key);

			// If cache has value, return cache data
			if (!string.IsNullOrEmpty(cacheValue))
			{
				return JsonConvert.DeserializeObject<T>(cacheValue)!;
			}

			return default!;
		}

		public async Task RemoveAsync(string key)
		{
			await _distributedCache.RemoveAsync(key);
		}

		public async Task SetAsync<T>(string key, T value)
		{
			var cacheOpt = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // Set cache time 1d
			};

			var jsonOpt = new JsonSerializerSettings() {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value, jsonOpt), cacheOpt);

		}
	}
}
