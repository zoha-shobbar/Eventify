using System.Text;
using Microsoft.AspNetCore.SignalR;
using Eventify.Server.Api.SignalR;
using Eventify.Server.Api.Services;
using Eventify.Server.Api.Models.Identity;
using Eventify.Shared.Controllers.Diagnostics;

namespace Eventify.Server.Api.Controllers.Diagnostics;

[ApiController, AllowAnonymous]
[Route("api/[controller]/[action]")]
public partial class DiagnosticsController : AppControllerBase, IDiagnosticsController
{
    [AutoInject] private PushNotificationService pushNotificationService = default!;
    [AutoInject] private IHubContext<AppHub> appHubContext = default!;

    [HttpPost]
    public async Task<string> PerformDiagnostics(CancellationToken cancellationToken)
    {
        StringBuilder result = new();

        result.AppendLine($"Client IP: {HttpContext.Connection.RemoteIpAddress}");

        result.AppendLine($"Trace => {Request.HttpContext.TraceIdentifier}");

        var isAuthenticated = User.IsAuthenticated();
        Guid? userSessionId = null;
        UserSession? userSession = null;

        if (isAuthenticated)
        {
            userSessionId = User.GetSessionId();
            userSession = await DbContext
                .UserSessions.SingleAsync(us => us.Id == userSessionId, cancellationToken);
        }

        result.AppendLine($"IsAuthenticated: {isAuthenticated.ToString().ToLowerInvariant()}");

        if (isAuthenticated)
        {
            var subscription = await DbContext.UserSessions.Include(us => us.PushNotificationSubscription)
                .FirstOrDefaultAsync(us => us.Id == userSessionId, cancellationToken);

            result.AppendLine($"Subscription exists: {(subscription?.PushNotificationSubscription is not null).ToString().ToLowerInvariant()}");

            await pushNotificationService.RequestPush("Test Push", DateTimeOffset.Now.ToString("HH:mm:ss"), "Test action", userRelatedPush: true, u => u.UserSessionId == userSessionId, cancellationToken);
        }

        if (isAuthenticated && userSession!.SignalRConnectionId is not null)
        {
            await appHubContext.Clients.Client(userSession.SignalRConnectionId).SendAsync(SignalREvents.SHOW_MESSAGE, DateTimeOffset.Now.ToString("HH:mm:ss"), cancellationToken);
        }

        result.AppendLine($"Culture => C: {CultureInfo.CurrentCulture.Name}, UC: {CultureInfo.CurrentUICulture.Name}");

        foreach (var header in Request.Headers)
        {
            result.AppendLine($"{header.Key}: {header.Value}");
        }

        return result.ToString();
    }
}
