using Eventify.Server.Api.Services;
using Eventify.Shared.Dtos.PushNotification;
using Eventify.Shared.Controllers.PushNotification;

namespace Eventify.Server.Api.Controllers.PushNotification;

[Route("api/[controller]/[action]")]
[ApiController, AllowAnonymous]
public partial class PushNotificationController : AppControllerBase, IPushNotificationController
{
    [AutoInject] PushNotificationService pushNotificationService = default!;

    [HttpPost]
    public async Task Subscribe([Required] PushNotificationSubscriptionDto subscription, CancellationToken cancellationToken)
    {
        await pushNotificationService.Subscribe(subscription, cancellationToken);
    }

    [HttpPost("{deviceId}")]
    public async Task Unsubscribe([Required] string deviceId, CancellationToken cancellationToken)
    {
        await pushNotificationService.Unsubscribe(deviceId, cancellationToken);
    }

#if Development // This action is for testing purposes only.
    [HttpPost]
    public async Task RequestPush([FromQuery] string? title = null, [FromQuery] string? message = null, [FromQuery] string? action = null, CancellationToken cancellationToken = default)
    {
        await pushNotificationService.RequestPush(title, message, action, false, null, cancellationToken);
    }
#endif
}
