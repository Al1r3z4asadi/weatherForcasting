﻿using weather.Common;

namespace weather.Core.AppService
{

    public interface IWeatherService
    {
        Task<ServiceResult<string>> GetLatestWeatherWithFallbackAsync(string key);
    }
}
