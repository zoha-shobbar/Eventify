using Microsoft.AspNetCore.OutputCaching;

namespace Eventify.Server.Api.Services;

/// <summary>
/// 
/// The default Eventify project template includes:
/// 1. `Static` file caching on browsers and CDN edge servers.
/// 2. Caching JSON and dynamic files responses on CDN edge servers and ASP.NET Core's Output Cache by using `AppResponseCache` attribute in controllers like `StatisticsController`, `AttachmentController` and minimal apis.
/// 3. Caching pre-rendered HTML results of Blazor pages on CDN edge servers and ASP.NET Core's Output by using `AppResponseCache` attribute in pages like HomePage.razor
/// 
/// - Note: The request URL must exactly match the URL passed to <see cref="PurgeCache(string[])"/> for successful purging.  
/// </summary>
public partial class ResponseCacheService
{
    [AutoInject] private HttpClient httpClient = default!;
    [AutoInject] private IOutputCacheStore outputCacheStore = default!;
    [AutoInject] private ServerApiSettings serverApiSettings = default!;
    [AutoInject] private IHttpContextAccessor httpContextAccessor = default!;

    public async Task PurgeCache(params string[] relativePaths)
    {
        foreach (var relativePath in relativePaths)
        {
            await outputCacheStore.EvictByTagAsync(relativePath, default);
        }
        // If you're using CDN like GCore or others, make sure to purge the Edge Cache of your CDN.
        // The Cloudflare Cache API is already integrated into the Eventify, but for other CDNs, 
        // you'll need to implement the caching logic yourself.
        if (httpContextAccessor.HttpContext!.Request.IsFromCDN())
        {
            throw new NotImplementedException();
        }
    }

}
