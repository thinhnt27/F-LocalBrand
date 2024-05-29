﻿namespace F_LocalBrand.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCachedResponseAsync(string cacheKey);

        Task RemoveCacheRepsonseAsync(string partern);
    }
}
