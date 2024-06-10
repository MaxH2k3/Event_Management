using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Repository.Common
{
	public interface ICacheRepository
	{
		// Get cache value by key
		Task<T> GetAsync<T>(string key);
		// Set cache value by key
		Task SetAsync<T>(string key, T value);
		// Remove cache value by key
		Task RemoveAsync(string key);
	}
}
