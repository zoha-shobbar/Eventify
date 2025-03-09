using Eventify.Server.Api.Models.PushNotification;
using Eventify.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace Eventify.Server.Api.Mappers;

/// <summary>
/// More info at Server/Mappers/README.md
/// </summary>
[Mapper]
public static partial class PushNotificationMapper
{
    public static partial void Patch(this PushNotificationSubscriptionDto source, PushNotificationSubscription destination);
}
