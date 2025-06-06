using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Application.Caching
{
    public interface ICachingService
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
        
    }
}