﻿namespace Application.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}