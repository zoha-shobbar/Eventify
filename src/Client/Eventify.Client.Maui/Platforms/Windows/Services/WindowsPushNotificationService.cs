using Eventify.Shared.Dtos.PushNotification;

namespace Eventify.Client.Maui.Platforms.Windows.Services;

public partial class WindowsPushNotificationService : PushNotificationServiceBase
{
    public override Task<PushNotificationSubscriptionDto> GetSubscription(CancellationToken cancellationToken) => 
        throw new NotImplementedException();
}
