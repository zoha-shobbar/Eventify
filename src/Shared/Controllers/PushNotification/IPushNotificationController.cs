﻿using Eventify.Shared.Dtos.PushNotification;

namespace Eventify.Shared.Controllers.PushNotification;

[Route("api/[controller]/[action]/")]
public interface IPushNotificationController : IAppController
{
    [HttpPost]
    Task Subscribe([Required] PushNotificationSubscriptionDto subscription, CancellationToken cancellationToken);

    [HttpPost("{deviceId}")]
    Task Unsubscribe([Required] string deviceId, CancellationToken cancellationToken);
}
