using Eventify.Server.Domain.Entities.PushNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Server.Domain.Entities.Identity;

public partial class UserSession
{
    public Guid Id { get; set; }

    public string? IP { get; set; }

    /// <summary>
    /// <inheritdoc cref="UserSessionDto.DeviceInfo"/>
    /// </summary>
    public string? DeviceInfo { get; set; }

    public string? Address { get; set; }

    /// <summary>
    /// <inheritdoc cref="AuthPolicies.PRIVILEGED_ACCESS"/>
    /// </summary>
    public bool Privileged { get; set; }

    public DateTimeOffset StartedOn { get; set; }

    public DateTimeOffset? RenewedOn { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public PushNotificationSubscription? PushNotificationSubscription { get; set; }

}
public class PushNotificationSubscriptionConfiguration : IEntityTypeConfiguration<PushNotificationSubscription>
{
    public void Configure(EntityTypeBuilder<PushNotificationSubscription> builder)
    {
        builder
            .HasOne(sub => sub.UserSession)
            .WithOne(us => us.PushNotificationSubscription)
            .HasForeignKey<PushNotificationSubscription>(sub => sub.UserSessionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasIndex(b => b.UserSessionId)
            .HasFilter($"[{nameof(PushNotificationSubscription.UserSessionId)}] IS NOT NULL")
            .IsUnique();
    }
}
