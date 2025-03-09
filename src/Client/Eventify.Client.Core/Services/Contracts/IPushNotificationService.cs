using Eventify.Shared.Dtos.PushNotification;

namespace Eventify.Client.Core.Services.Contracts;

public interface IPushNotificationService
{
    string Token { get; set; }
    Task<bool> IsPushNotificationSupported(CancellationToken cancellationToken);
    Task<PushNotificationSubscriptionDto> GetSubscription(CancellationToken cancellationToken);
    Task Subscribe(CancellationToken cancellationToken);
}
