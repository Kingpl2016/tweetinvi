﻿using System.Threading.Tasks;
using Tweetinvi.Client;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Core.RateLimit
{
    /// <summary>
    /// Proxy used to access and refresh the rate limits cache.
    /// </summary>
    public interface IRateLimitCacheManager
    {
        IRateLimitCache RateLimitCache { get; }
        IRateLimitsClient RateLimitsClient { get; set; }

        /// <summary>
        /// Return the rate limits for a specific query.
        /// If the query url cannot be mapped, a new one is created in the OtherQueryRateLimits.
        /// If the credentials rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        Task<IEndpointRateLimit> GetQueryRateLimit(IGetEndpointRateLimitsParameters parameters, IReadOnlyTwitterCredentials credentials);


        /// <summary>
        /// Return the all the rate limits for a specific set of credentials.
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        Task<ICredentialsRateLimits> GetCredentialsRateLimits(IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Update the rate limit cache with a specific set of rate limits.
        /// </summary>
        Task UpdateCredentialsRateLimits(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits);

        Task<ICredentialsRateLimits> RefreshCredentialsRateLimits(IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Returns whether the rate limits should be refreshed to retrieve
        /// a specific endpoint information
        /// </summary>
        bool ShouldEndpointCacheBeUpdated(IEndpointRateLimit rateLimit);
    }
}