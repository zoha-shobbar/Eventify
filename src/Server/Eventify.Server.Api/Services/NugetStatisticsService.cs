﻿using Eventify.Shared.Dtos.Statistics;

namespace Eventify.Server.Api.Services;

public partial class NugetStatisticsService
{
    [AutoInject] protected HttpClient httpClient = default!;

    public virtual async ValueTask<NugetStatsDto> GetPackageStats(string packageId, CancellationToken cancellationToken)
    {
        var url = $"/query?q=packageid:{packageId}";

        var response = await httpClient.GetFromJsonAsync(url, ServerJsonContext.Default.Options.GetTypeInfo<NugetStatsDto>(), cancellationToken)
                                ?? throw new ResourceNotFoundException();

        return response;
    }
}
